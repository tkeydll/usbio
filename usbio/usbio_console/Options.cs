using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

namespace usbio_console
{
    class Options
    {
        [Option('t', "exec-time", Required=false, DefaultValue=3000, HelpText="電源on時間[msec]")]
        public int Time { get; set; }

        [Option('n',"exec-number", Required=false, DefaultValue=1, HelpText="実行回数")]
        public int Number { get; set; }

        [Option('i', "exec-interval", Required=false, DefaultValue=500, HelpText="繰り返し実行時のインターバル[msec]")]
        public int Interval { get; set; }

        [Option('d', "debug", Required=false, DefaultValue=false, HelpText="デバッグ情報を表示")]
        public bool Debug { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            //return HelpText.AutoBuild(this).ToString();
            return string.Empty;
        }

    }
}
