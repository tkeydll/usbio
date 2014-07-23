using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace usbio.usbio_console
{
    class Program
    {
        private static Options o;

        /// <summary>
        /// usbio制御ラッパー
        /// </summary>
        private static UsbioWrapper _io = new UsbioWrapper();

        /// <summary>
        /// エントリポイント
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Ctrl_C_Pressed);

            o = new Options();
            bool result = CommandLine.Parser.Default.ParseArguments(args, o);
            if (!result)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(o).ToString());
                return;
            }

            // Open USB-IO.
            if (_io.OpenDevice() == false)
            {
                Console.WriteLine("Cannot open device.");
                return;
            }

            try
            {
                for (int i = 0; i < o.Number; i++)
                {
                    // interval
                    if (i > 0)
                    {
                        System.Threading.Thread.Sleep(o.Interval);
                    }

                    // power on
                    _io.SendRecv(true);
                    System.Threading.Thread.Sleep(o.Time);

                    // power off
                    _io.SendRecv(false);
                }
            }
            finally
            {
                _io.CloseDevice();
            }

        }

        /// <summary>
        /// Ctrl-C で強制終了された場合に電源をOffにします
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Ctrl_C_Pressed(object sender, ConsoleCancelEventArgs e)
        {
            // power off
            _io.SendRecv(false);
        }
    }
}
