using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace usbio.usbio_console
{
    class Program
    {
        private static usbiolib io;
        private static Options o;

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

            io = new usbiolib();

            // Open USB-IO.
            if (io.openDevice() == false)
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
                    SendRecv(true);
                    System.Threading.Thread.Sleep(o.Time);

                    // power off
                    SendRecv(false);
                }
            }
            finally
            {
                CloseDevice();
            }

        }

        const byte USBIO_PWR_ON = 0x01;
        const byte USBIO_PWR_OFF = 0x00;

        /// <summary>
        /// JP1電源のon/offを制御します。
        /// </summary>
        /// <param name="powerOn">
        /// true: 電源on, false: 電源off
        /// </param>
        private static void SendRecv(bool powerOn)
        {
            byte[] sendData = new byte[64];
            byte[] recvData = new byte[64];

            sendData[0] = 0x20;
            sendData[1] = 0x01;
            sendData[2] = powerOn ? USBIO_PWR_ON : USBIO_PWR_OFF;

            sendData[63] = 0x00;

            io.SendRecv(sendData, ref recvData);
        }

        /// <summary>
        /// 電源を落として制御を終了します。
        /// </summary>
        private static void CloseDevice()
        {
            if (io != null)
            {
                SendRecv(false);
                io.closeDevice();
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
            SendRecv(false);
        }
    }
}
