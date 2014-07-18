using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using usbio;

namespace usbio_web.Controllers
{
    public class UsbioController : ApiController
    {
        static usbiolib io;

        /// <summary>
        /// POST api/Usbio
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string Post([FromBody]Usbio usb)
        {
            string ret = string.Format("入力パラメータ：[Time]{0}, [Number]{1}, [Interval]{2}", usb.Time, usb.Number, usb.Interval);

            io = new usbiolib();

            // Open USB-IO.
            if (io.openDevice() == false)
            {
                return "Cannot open device.";
            }

            try
            {
                for (int i = 0; i < usb.Number; i++)
                {
                    // interval
                    if (i > 0)
                    {
                        System.Threading.Thread.Sleep(usb.Interval);
                    }

                    // power on
                    SendRecv(true);
                    System.Threading.Thread.Sleep(usb.Time);

                    // power off
                    SendRecv(false);
                }
            }
            finally
            {
                CloseDevice();
            }
            
            return ret;
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

    }
}
