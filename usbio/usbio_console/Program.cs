using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace usbio_cons
{
    class Program
    {
        static void Main(string[] args)
        {
            usbiolib.usbiolib io = new usbiolib.usbiolib();

            try
            {
                if (io.openDevice() == false)
                {
                    Console.WriteLine("Cannot open device.");
                    return;
                }
                Console.WriteLine("Device is opened.");

                // 点滅
                for (int i = 0; i < 10; i++ )
                {
                    SendRecv(io, 0x01);
                    System.Threading.Thread.Sleep(100);
                    SendRecv(io, 0x00);
                    System.Threading.Thread.Sleep(100);
                }
                
            }
            finally
            {
                if (io != null)
                {
                    io.closeDevice();
                    Console.WriteLine("Device is closed.");
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
