using System;
using System.Collections.Generic;
using System.Text;
using JHSoftware.SimpleDNS.Plugin;

namespace SmartReverseDnsPlugIn
{
    public class SmartReverseDnsGH : IGetHostPlugIn 
    {
        SrdConfig Cfg;
        bool CfgIPv6;
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
                HostIPMatchPrefix = sb.ToString(0, Cfg.Subnet / 4);
            }
            else
            {
                HostIPMatchPrefix = ipb[0].ToString();
                if (Cfg.Subnet >= 16) { HostIPMatchPrefix += "." + ipb[1].ToString(); }
                if (Cfg.Subnet == 24) { HostIPMatchPrefix += "." + ipb[2].ToString(); }
            }
        }

        public DNSAskAboutGH GetDNSAskAbout()
        {
            var rv = new DNSAskAboutGH();
            rv.ForwardIPv4 = !CfgIPv6 && Cfg.HostReq;
            rv.ForwardIPv6 = CfgIPv6 && Cfg.HostReq;
            rv.RevIPv4Addr = CfgIPv6 ? null : IPAddressV4.Parse(Cfg.FirstIP);
            rv.RevIPv6Addr = CfgIPv6 ? IPAddressV6.Parse(Cfg.FirstIP) : null;
            rv.RevIPv4MaskSize = (byte)Cfg.Subnet;
            rv.RevIPv6MaskSize = (byte)Cfg.Subnet;
            var i = Cfg.Suffix.IndexOf('.');
            rv.Domain = i > 0 ? DomainName.Parse("*" + Cfg.Suffix.Substring(i)) : null;
            rv.TXT = false;
            return rv;
        }

        public void Lookup(IDNSRequest req, ref IPAddress resultIP, ref int resultTTL)
        {
            resultIP = null;
            var HostName = req.QName.ToString().ToLower();
            if (Cfg.Prefix.Length > 0 && !HostName.StartsWith(Cfg.Prefix)) return;
            if (!HostName.EndsWith(Cfg.Suffix)) return;
            var IPPart = HostName.Substring(Cfg.Prefix.Length);
            IPPart = IPPart.Substring(0, IPPart.Length - Cfg.Suffix.Length);
            if (CfgIPv6)
            {
                IPPart = IPPart.Replace("-", "");
                if (!Cfg.FullIP) IPPart = HostIPMatchPrefix + IPPart;
                if (IPPart.Length != 32) return;
                IPPart = IPPart.Substring(0, 4) + ":" +
                       IPPart.Substring(4, 4) + ":" +
                       IPPart.Substring(8, 4) + ":" +
                       IPPart.Substring(12, 4) + ":" +
                       IPPart.Substring(16, 4) + ":" +
                       IPPart.Substring(20, 4) + ":" +
                       IPPart.Substring(24, 4) + ":" +
                       IPPart.Substring(28, 4);
                if (!IPAddressV6.TryParse(IPPart, ref resultIP)) { resultIP = null; return; }
            }
            else
            {
                IPPart = IPPart.Replace("-", ".");
                if (!Cfg.FullIP) IPPart = HostIPMatchPrefix + "." + IPPart;
                if (!IPAddressV4.TryParse(IPPart, ref resultIP)) { resultIP = null; return; }
            }
            resultTTL = Cfg.TTL;
        }

        public void LookupReverse(IDNSRequest req, ref DomainName resultName, ref int resultTTL)
        {
            resultName = null;
            var ipb = req.QNameIP.GetBytes();
            var sn = Cfg.FullIP ? 0 : Cfg.Subnet;
            string x;
            if (CfgIPv6)
            {
                var sb = new System.Text.StringBuilder(32);
                for (var i = 0; i < 16; i++) sb.Append(ByteToHex2(ipb[i]));
                var z = sb.ToString();
                if (!Cfg.FullIP) z = z.Substring(Cfg.Subnet / 4);
                //insert hypens instead of colons
                x = "";
                int p;
                while (z.Length > 0)
                {
                    if (x.Length > 0) x = "-" + x;
                    if (z.Length <= 4) p = 0; else p = z.Length - 4;
                    x = z.Substring(p) + x;
                    z = z.Substring(0, p);
                }
            }
            else
            {   //IPv4
                x = ipb[3].ToString();
                if (sn <= 16) x = ipb[2].ToString() + "-" + x;
                if (sn <= 8) x = ipb[1].ToString() + "-" + x;
                if (sn == 0) x = ipb[0].ToString() + "-" + x;
            }
            resultName = DomainName.Parse(Cfg.Prefix + x + Cfg.Suffix);
            resultTTL = Cfg.TTL;
        }

        private string ByteToHex2(byte val)
        {
            return "0123456789abcdef".Substring(val >> 4, 1) + "0123456789abcdef".Substring(val & 15, 1);
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

        public void LookupTXT(IDNSRequest req, ref string resultText, ref int resultTTL)
        {
            throw new NotImplementedException();
        }
    }
}
