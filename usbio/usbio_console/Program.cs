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

        static void Main(string[] args)
        {
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
                int i = 0;
                while (i < o.ControlTime)
                {
                    SendRecv(true);
                    System.Threading.Thread.Sleep(o.PowerOnTime);
                    SendRecv(false);
                    System.Threading.Thread.Sleep(o.Interval);

                    i += o.PowerOnTime + o.Interval;
                }

            }
            finally
            {
                CloseDevice();
            }

        }

        const byte USBIO_PWR_ON = 0x01;
        const byte USBIO_PWR_OFF = 0x00;

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
        /// 電源を落として制御を終了します
        /// </summary>
        /// <param name="io"></param>
        private static void CloseDevice()
        {
            if (io != null)
            {
                SendRecv(false);
                io.closeDevice();
            }
        }

    }
}
