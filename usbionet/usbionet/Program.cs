using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using USB_IO_Family;

namespace usbionet
{
    class Program
    {

        static void Main(string[] args)
        {
            ioCtl usbio = new ioCtl();

            try
            {
                int ret = usbio.openDevice();
                if (ret == 0)
                {
                    Console.WriteLine(string.Format("Device cannot open. [{0}]", ret));
                    return;
                }

                Console.WriteLine("USB Device is opened.");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (usbio != null)
                {
                    usbio.closeDevice();
                    Console.WriteLine("USB Device is closed correctly.");
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

    }
}
