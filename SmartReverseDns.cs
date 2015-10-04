using System;
using System.Collections.Generic;
using System.Text;
using JHSoftware.SimpleDNS.Plugin;

namespace SmartReverseDnsPlugIn
{
    public class SmartReverseDns : IGetAnswerPlugIn 
    {
        DNSRRType RecTypePTR = DNSRRType.Parse("PTR");
        DNSRRType RecTypeA = DNSRRType.Parse("A");
        DNSRRType RecTypeAAAA = DNSRRType.Parse("AAAA");
        SrdConfig Cfg;
        bool CfgIPv6;
        DomainName PtrMatchSuffix;
        string HostIPMatchPrefix;

        public IPlugInBase.PlugInTypeInfo GetPlugInTypeInfo()
        {
            var rv = new IPlugInBase.PlugInTypeInfo();
            rv.Name = "Smart Reverse DNS";
            rv.Description = "Synthesizes reverse DNS records";
            rv.InfoURL = "http://www.simpledns.com/plugin-smartreversedns";
            rv.ConfigFile = false;
            rv.MultiThreaded = false;
            return rv;
        }

        public void LoadConfig(string config, Guid instanceID, string dataPath, ref int maxThreads)
        {
            Cfg = SrdConfig.DeSerialize(config);
            var ip = System.Net.IPAddress.Parse(Cfg.FirstIP);
            CfgIPv6 =ip.AddressFamily==System.Net.Sockets.AddressFamily.InterNetworkV6 ;
            var ipb = ip.GetAddressBytes();
            if (CfgIPv6)
            {
                var sb = new System.Text.StringBuilder(32);
                for (var i = 0; i < 16; i++) sb.Append(ByteToHex2( ipb[i]));
                var x = ".ip6.arpa";
                for (var i = 0; i< Cfg.Subnet/4 ; i++) x = sb[i] + "." + x;
                PtrMatchSuffix = DomainName.Parse(x);
                HostIPMatchPrefix = sb.ToString(0, Cfg.Subnet / 4);
            }
            else
            {
                var x = ipb[0].ToString() + ".in-addr.arpa";
                HostIPMatchPrefix = ipb[0].ToString();
                if (Cfg.Subnet >= 16) { x = ipb[1].ToString() + "." + x; HostIPMatchPrefix += "." + ipb[1].ToString(); }
                if (Cfg.Subnet == 24) { x = ipb[2].ToString() + "." + x; HostIPMatchPrefix += "." + ipb[2].ToString(); }
                PtrMatchSuffix = DomainName.Parse(x);
            }
        }

        public DNSAskAbout GetDNSAskAbout()
        {
            var rv = new DNSAskAbout();
            if(Cfg.HostReq) {
                rv.Domain = null;
                rv.RRTypes=new DNSRRType[2];
                rv.RRTypes[1] = CfgIPv6 ? RecTypeAAAA : RecTypeA;
            } else {
                rv.Domain = JHSoftware.SimpleDNS.Plugin.DomainName.Parse("*." + PtrMatchSuffix.ToString());
                rv.RRTypes = new DNSRRType[1];
            }
            rv.RRTypes[0] = RecTypePTR;
            return rv;
        }

        public DNSAnswer Lookup(IDNSRequest request)
        {
            if (request.QType == RecTypePTR)
            {
                if (!request.QName.EndsWith(PtrMatchSuffix)) return null;
                return CfgIPv6 ? LookupPTR6(request) : LookupPTR4(request);
            }
            if (!Cfg.HostReq) return null;
            if (request.QType == RecTypeA) return LookupHost4(request);
            if (request.QType == RecTypeAAAA) return LookupHost6(request);
            return null;
        }

        private DNSAnswer LookupHost4(IDNSRequest request)
        {
            if (CfgIPv6) return null;
            var HostName = request.QName.ToString().ToLower();
            if (Cfg.Prefix.Length > 0 && !HostName.StartsWith(Cfg.Prefix)) return null;
            if (!HostName.EndsWith(Cfg.Suffix)) return null;
            var IPPart = HostName.Substring(Cfg.Prefix.Length);
            IPPart = IPPart.Substring(0, IPPart.Length - Cfg.Suffix.Length);
            IPPart = IPPart.Replace("-", ".");
            if (!Cfg.FullIP) IPPart = HostIPMatchPrefix + "." + IPPart;
            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse( IPPart,out ip)) return null;
            return MakeAnswer(request,IPPart );
        }

        private DNSAnswer LookupHost6(IDNSRequest request)
        {
            if (!CfgIPv6) return null;
            var HostName = request.QName.ToString().ToLower();
            if (Cfg.Prefix.Length > 0 && !HostName.StartsWith(Cfg.Prefix)) return null;
            if (!HostName.EndsWith(Cfg.Suffix)) return null;
            var IPPart = HostName.Substring(Cfg.Prefix.Length);
            IPPart = IPPart.Substring(0, IPPart.Length - Cfg.Suffix.Length);
            IPPart= IPPart.Replace("-", "");
            if (!Cfg.FullIP) IPPart = HostIPMatchPrefix + IPPart;
            if (IPPart.Length!=32) return null;
            IPPart = IPPart.Substring(0, 4) + ":" +
                   IPPart.Substring(4, 4) + ":" +
                   IPPart.Substring(8, 4) + ":" +
                   IPPart.Substring(12, 4) + ":" +
                   IPPart.Substring(16, 4) + ":" +
                   IPPart.Substring(20, 4) + ":" +
                   IPPart.Substring(24, 4) + ":" +
                   IPPart.Substring(28, 4);
            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse(IPPart, out ip)) return null;
            return MakeAnswer(request, IPPart);
        }

        private DNSAnswer LookupPTR4(IDNSRequest request)
        {
            var x = request.QName.ToString();
            x = x.Substring(0, x.Length - 13); // strip .in-addr.arpa
            var segs = x.Split('.');
            if (segs.Length != 4) return null;
            var ipb=new byte[4];
            byte tmp;
            for (var i = 0; i < 4; i++)
            {
                if (!byte.TryParse(segs[3 - i],out tmp)) return null;
                ipb[i] = tmp;
            }
            return MakeAnswer(request, MakeHostStr(ipb) +"." );
        }

        private DNSAnswer LookupPTR6(IDNSRequest request)
        {
            var x = request.QName.ToString().ToLower();
            x = x.Substring(0, x.Length - 8); // strip ip6.arpa
            if (x.Length != 64) return null;
            var ipb = new byte[16];
            int p=0;
            int h1, h2;
            for (var i = 60; i >=0; i-=4)
            {
                if (x[i + 1] != '.') return null;
                if (x[i + 3] != '.') return null;
                h1=HexCharValue(x[i]);
                if( h1<0 ) return null;
                h2=HexCharValue(x[i+2]);
                if( h2<0 ) return null;
                ipb[p++]=(byte) (h1+(h2<<4));
            }
            return MakeAnswer(request, MakeHostStr(ipb) + ".");
        }

        private int HexCharValue(char c)
        {
            if (c < '0') return -1;
            if (c <= '9') return c - 48; // ascii for '0' = 48
            if (c < 'a') return -1;
            if (c <= 'f') return c - 87; // ascii for 'a' = 97 
            return -1;
        }

        private string ByteToHex2(byte val)
        {
            return "0123456789abcdef".Substring(val >> 4, 1) + "0123456789abcdef".Substring(val & 15, 1);
        }


        private string MakeHostStr(byte[] ipb)
        {
            var sn = Cfg.FullIP ? 0 : Cfg.Subnet;
            if (CfgIPv6)
            {
                var sb = new System.Text.StringBuilder(32);
                for (var i = 0; i<16  ; i++) sb.Append( ByteToHex2 ( ipb[i]));
                var x = sb.ToString();
                if (!Cfg.FullIP) x = x.Substring(Cfg.Subnet / 4);
                //insert hypens instead of colons
                var y = "";
                int p;
                while (x.Length > 0)
                {
                    if (y.Length > 0) y = "-" + y;
                    if(x.Length<=4) p=0; else p=x.Length-4;
                    y = x.Substring(p) + y;
                    x = x.Substring(0, p);
                }
                return Cfg.Prefix + y + Cfg.Suffix;
            }
            else
            {
                //IPv4
                var x = ipb[3].ToString();
                if (sn <= 16) x = ipb[2].ToString() + "-" + x;
                if (sn <= 8) x = ipb[1].ToString() + "-" + x;
                if (sn == 0) x = ipb[0].ToString() + "-" + x;
                return Cfg.Prefix + x + Cfg.Suffix;
            }
        }

        private DNSAnswer MakeAnswer( IDNSRequest request, string dataStr)
        {
            var rec = new DNSRecord();
            rec.Name = request.QName;
            rec.RRType = request.QType;
            rec.Data = dataStr;
            rec.TTL =Cfg.TTL; 
            rec.AnswerSection = DNSAnswerSection.Answer;
            var rv = new DNSAnswer();
            rv.Records.Add(rec);
            return rv;
        }

        public OptionsUI GetOptionsUI(Guid instanceID, string dataPath)
        {
            return new SettingsUI();
        }


        // ----------- Not used --------------

        public event IPlugInBase.LogLineEventHandler LogLine;
        public event IPlugInBase.SaveConfigEventHandler SaveConfig;
        public event IPlugInBase.AsyncErrorEventHandler AsyncError;

        public bool InstanceConflict(string config1, string config2, ref string errorMsg)
        {
            return false;
        }

        public void LoadState(string state)
        {
            return;
        }

        public string SaveState()
        {
            return null;
        }

        public void StartService()
        {
            return;
        }

        public void StopService()
        {
            return;
        }
    }
}
