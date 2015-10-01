using System;
using System.Collections.Generic;
using System.Text;

namespace SmartReverseDnsPlugIn
{
    public class SrdConfig
    {
        public string FirstIP;
        public int Subnet;
        public string Prefix;
        public bool FullIP;
        public string Suffix;
        public int TTL;
        public bool HostReq;

        public string Serialize()
        {
            return "1|" +
                 FirstIP + '|' +
                 Subnet + '|' +
                 Prefix + '|' +
                 (FullIP ? "Y" : "N") + "|" +
                 Suffix + "|" +
                 TTL.ToString() + "|" +
                 (HostReq ? "Y" : "N");
        }

        static public SrdConfig DeSerialize(string cfgStr)
        {
            var itm = cfgStr.Split('|');
            if (itm[0] != "1") throw new Exception("Unknown configuration data version");
            var rv = new SrdConfig();
            rv.FirstIP = itm[1];
            rv.Subnet = int.Parse(itm[2]);
            rv.Prefix = itm[3];
            rv.FullIP = (itm[4] == "Y");
            rv.Suffix = itm[5];
            rv.TTL = int.Parse(itm[6]);
            rv.HostReq = (itm[7] == "Y");
            return rv;
        }
    }
}
