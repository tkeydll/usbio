using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace usbio_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Options o = new Options();
            bool result = CommandLine.Parser.Default.ParseArguments(args, o);
            if (!result)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(o).ToString());
                return;
            }

            usbiolib.usbiolib io = new usbiolib.usbiolib();

            // Open USB-IO.
            if (io.openDevice() == false)
            {
                Console.WriteLine("Cannot open device.");
                return;
            }

            try
            {
                int i = 0;
                while (i < o.ControlTime)
                {
                    SendRecv(io, 0x01);
                    System.Threading.Thread.Sleep(o.PowerOnTime);
                    SendRecv(io, 0x00);
                    System.Threading.Thread.Sleep(o.Interval);

                    i += o.PowerOnTime + o.Interval;
                }

            }
            finally
            {
                CloseDevice(io);
            }

        }

        private static void SendRecv(usbiolib.usbiolib io, byte sendJ1)
        {
            byte[] sendData = new byte[64];
            byte[] recvData = new byte[64];

            sendData[0] = 0x20;     // コマンド：デジタル入出力
            sendData[1] = 0x01;     // 出力１：J1
            sendData[2] = sendJ1;     // 値

            sendData[63] = 0x00;     // シーケンス

            io.SendRecv(sendData, ref recvData);
        }

        /// <summary>
        /// 電源を落として制御を終了します
        /// </summary>
        /// <param name="io"></param>
        private static void CloseDevice(usbiolib.usbiolib io)
        {
            if (io != null)
            {
                SendRecv(io, 0x00);
                io.closeDevice();
            }
        }

    }
}
