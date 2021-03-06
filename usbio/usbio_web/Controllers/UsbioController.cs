﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using libusbio;

namespace usbio_web.Controllers
{
    public class UsbioController : ApiController
    {
        /// <summary>
        /// usbio制御ラッパー
        /// </summary>
        static UsbIoWrapper _io = new UsbIoWrapper();

        public string Get(int time, int number, int interval)
        {
            string ret = string.Format("入力パラメータ：[Time]{0}, [Number]{1}, [Interval]{2}", time, number, interval);
            try
            {
                this.SendRecv(time, number, interval);
            }
            catch (Exception ex)
            {
                ret += Environment.NewLine + ex.ToString();
            }
            return ret;
        }

        /// <summary>
        /// POST api/Usbio
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string Post([FromBody]Usbio usb)
        {
            string ret = string.Format("入力パラメータ：[Time]{0}, [Number]{1}, [Interval]{2}", usb.Time, usb.Number, usb.Interval);
            try
            {
                this.SendRecv(usb.Time, usb.Number, usb.Interval);
            }
            catch (Exception ex)
            {
                ret += Environment.NewLine + ex.ToString();
            }
            return ret;
        }

        /// <summary>
        /// USB-IO制御
        /// </summary>
        /// <param name="time"></param>
        /// <param name="number"></param>
        /// <param name="interval"></param>
        private void SendRecv(int time, int number, int interval)
        {
            // Open USB-IO.
            if (_io.OpenDevice() == false)
            {
                throw new ApplicationException("Cannot open device.");
            }

            try
            {
                for (int i = 0; i < number; i++)
                {
                    // interval
                    if (i > 0)
                    {
                        System.Threading.Thread.Sleep(interval);
                    }

                    // power on
                    _io.SendRecv(1, "11111111");
                    System.Threading.Thread.Sleep(time);

                    // power off
                    _io.SendRecv(1, "00000000");
                }
            }
            finally
            {
                _io.CloseDevice();
            }
        }
    }
}
