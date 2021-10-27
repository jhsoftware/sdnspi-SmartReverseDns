using System;
using System.Collections.Generic;
using System.Text;
using JHSoftware.SimpleDNS.Plugin;
using JHSoftware.SimpleDNS;

namespace SmartReverseDnsPlugIn
{
    public class SmartReverseDnsGH : ILookupHost, ILookupReverse, IOptionsUI
    {
        SrdConfig Cfg;
        SdnsIP FirstIP;

        string HostIPMatchPrefix;

        IHost _Host;
        IHost IPlugInBase.Host { get => _Host; set => _Host=value; }

        public TypeInfo GetTypeInfo()
        {
            var rv = new TypeInfo();
            rv.Name = "Smart Reverse DNS";
            rv.Description = "Synthesizes reverse DNS records";
            rv.InfoURL = "https://simpledns.plus/plugin-smartreversedns";
            return rv;
        }

        public void LoadConfig(string config, Guid instanceID, string dataPath)
        {
            Cfg = SrdConfig.DeSerialize(config);
            FirstIP = SdnsIP.Parse(Cfg.FirstIP);
            var ip = System.Net.IPAddress.Parse(Cfg.FirstIP);
            var ipb = ip.GetAddressBytes();
            if (FirstIP.IsIPv6())
            {
                var sb = new System.Text.StringBuilder(32);
                for (var i = 0; i < 16; i++) sb.Append(ByteToHex2(ipb[i]));
                HostIPMatchPrefix = sb.ToString(0, Cfg.Subnet / 4);
            }
            else
            {
                HostIPMatchPrefix = ipb[0].ToString();
                if (Cfg.Subnet >= 16) { HostIPMatchPrefix += "." + ipb[1].ToString(); }
                if (Cfg.Subnet == 24) { HostIPMatchPrefix += "." + ipb[2].ToString(); }
            }
        }

        public System.Threading.Tasks.Task<LookupResult<SdnsIP>> LookupHost(DomName name, bool ipv6, IRequestContext req)
        {
            return System.Threading.Tasks.Task.FromResult(LookupHost2(name, ipv6, req));
        }
        private LookupResult<SdnsIP> LookupHost2(DomName name, bool ipv6, IRequestContext req)
        {
            if (!Cfg.HostReq) return null;
            if (FirstIP.IsIPv6() != ipv6) return null;
            var HostName = name.ToString().ToLower();
            if (Cfg.Prefix.Length > 0 && !HostName.StartsWith(Cfg.Prefix)) return null;
            if (!HostName.EndsWith(Cfg.Suffix)) return null;
            var IPPart = HostName.Substring(Cfg.Prefix.Length);
            IPPart = IPPart.Substring(0, IPPart.Length - Cfg.Suffix.Length);
            SdnsIP resultIP=null;
            if (FirstIP.IsIPv6())
            {
                IPPart = IPPart.Replace("-", "");
                if (!Cfg.FullIP) IPPart = HostIPMatchPrefix + IPPart;
                if (IPPart.Length != 32) return null;
                IPPart = IPPart.Substring(0, 4) + ":" +
                       IPPart.Substring(4, 4) + ":" +
                       IPPart.Substring(8, 4) + ":" +
                       IPPart.Substring(12, 4) + ":" +
                       IPPart.Substring(16, 4) + ":" +
                       IPPart.Substring(20, 4) + ":" +
                       IPPart.Substring(24, 4) + ":" +
                       IPPart.Substring(28, 4);
                if (!SdnsIP.TryParse(IPPart, ref resultIP)) return null;
            }
            else
            {
                IPPart = IPPart.Replace("-", ".");
                if (!Cfg.FullIP) IPPart = HostIPMatchPrefix + "." + IPPart;
                if (!SdnsIP.TryParse(IPPart, ref resultIP)) return null;
            }
            return new LookupResult<SdnsIP> { Value = resultIP, TTL = Cfg.TTL };
        }

        public System.Threading.Tasks.Task<LookupResult<DomName>> LookupReverse(SdnsIP ip, IRequestContext req)
        {
            return System.Threading.Tasks.Task.FromResult(LookupReverse2(ip, req));
        }
        private LookupResult<DomName> LookupReverse2(SdnsIP ip, IRequestContext req)
        {
            if (FirstIP.IsIPv6() != ip.IsIPv6()) return null;
            if (FirstIP.MaskFirst(Cfg.Subnet) != ip.MaskFirst(Cfg.Subnet)) return null;

            var ipb = ip.GetBytes();
            var sn = Cfg.FullIP ? 0 : Cfg.Subnet;
            string x;
            if (FirstIP.IsIPv6())
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
            return new LookupResult<DomName> { Value = DomName.Parse(Cfg.Prefix + x + Cfg.Suffix), TTL = Cfg.TTL };
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

        public bool InstanceConflict(string config1, string config2, ref string errorMsg)
        {
            return false;
        }

        public System.Threading.Tasks.Task StartService()
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public void StopService()
        {
            return;
        }
    }
}
