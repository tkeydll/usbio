using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;

namespace usbio.usbio_console
{
    class Options
    {
        [Option('c', "control-time", Required=true, DefaultValue=10000, HelpText="制御時間[msec]")]
        public int ControlTime { get; set; }

        [Option('n', "power-on-time", Required=false, DefaultValue=500, HelpText="電源on時間[msec]")]
        public int PowerOnTime { get; set; }

        [Option('i', "interval", Required=false, DefaultValue=500, HelpText="電源off時間[msec]")]
        public int Interval { get; set; }

        //[Option('j', "channel", Required=false, DefaultValue=0, HelpText="出力チャネル")]
        //public int Port { get; set; }
    }
}
