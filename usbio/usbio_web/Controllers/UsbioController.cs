using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using tkeydll.usbio.usbiolib;

namespace usbio_web.Controllers
{
    public class UsbioController : ApiController
    {
        /// <summary>
        /// usbio制御ラッパー
        /// </summary>
        static UsbioWrapper _io = new UsbioWrapper();

        /// <summary>
        /// POST api/Usbio
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string Post([FromBody]Usbio usb)
        {
            string ret = string.Format("入力パラメータ：[Time]{0}, [Number]{1}, [Interval]{2}", usb.Time, usb.Number, usb.Interval);

            // Open USB-IO.
            if (_io.OpenDevice() == false)
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
                    _io.SendRecv(true);
                    System.Threading.Thread.Sleep(usb.Time);

                    // power off
                    _io.SendRecv(false);
                }
            }
            finally
            {
                _io.CloseDevice();
            }
            
            return ret;
        }
    }
}
