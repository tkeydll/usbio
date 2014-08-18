using System;
using System.Collections.Generic;
using USB_IO_Family32;

namespace libusbio
{
    public class UsbIoWrapper
    {
        public ioCtl _io = new ioCtl();

        public bool OpenDevice()
        {
            return (_io.openDevice() > 0) ? true : false;
        }

        public void CloseDevice()
        {
            _io.closeDevice();
        }

        public int SendRecv(int port, string data)
        {
            ioCtl.ST_CTL_OUTPUT[] stOut = new ioCtl.ST_CTL_OUTPUT[2];
            ioCtl.ST_CTL_INPUT stIn = new ioCtl.ST_CTL_INPUT();

            stOut[0].Port = (byte)port;
            stOut[0].Data = Convert.ToByte(data, 2);

            return _io.ctlInOut(ref stIn, stOut);
        }
    }
}
