using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;

namespace usbio_console
{
    class Options
    {
        [Option('t', "term", Required=true, HelpText="制御時間[sec]")]
        public int Term { get; set; }

        [Option('i', "interval", Required=false, DefaultValue=500, HelpText="点滅する間隔[msec]初期値：500ms")]
        public int Interval { get; set; }

    }
}
