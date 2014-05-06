using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;

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

            try
            {
                if (io.openDevice() == false)
                {
                    Console.WriteLine("Cannot open device.");
                    return;
                }

                // 点滅
                int t = 0;
                while (t < (o.Term * 1000))
                {
                    SendRecv(io, 0x01);
                    System.Threading.Thread.Sleep(o.Interval);
                    SendRecv(io, 0x00);
                    System.Threading.Thread.Sleep(o.Interval);

                    t += o.Interval * 2;
                }
            }
            finally
            {
                if (io != null)
                {
                    io.closeDevice();
                }
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

    }
}
