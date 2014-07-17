using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace usbio_web.Controllers
{
    public class UsbioController : ApiController
    {
        /// <summary>
        /// POST api/Usbio/5
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string Post(int time)
        {
            string ret = time.ToString();

            return ret;
        }
    }
}
