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
        [Option('t', "time", Required=false, DefaultValue=3000, HelpText="電源on時間[msec]")]
        public int Time { get; set; }

        [Option('r', "repeat", Required=false, DefaultValue=0, HelpText="繰り返し回数")]
        public int Repeat { get; set; }

        [Option('i', "interval", Required=false, DefaultValue=500, HelpText="繰り返し実行時のインターバル[msec]")]
        public int Interval { get; set; }

        //[Option('j', "channel", Required=false, DefaultValue=0, HelpText="出力チャネル")]
        //public int Port { get; set; }
    }
}
