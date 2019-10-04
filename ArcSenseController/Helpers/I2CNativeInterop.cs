using System;
using System.Runtime.InteropServices;

namespace ArcSenseController.Helpers
{
    public static class I2CNativeInterop
    {
        public const int OPEN_READ_WRITE = 2;
        public const int I2C_SLAVE = 0x0703;

        [DllImport("libc.so.6", EntryPoint = "open")]
        public static extern IntPtr Open(string fileName, int mode);

        [DllImport("libc.so.6", EntryPoint = "ioctl", SetLastError = true)]
        public static extern int Ioctl(IntPtr fd, int request, int data);

        [DllImport("libc.so.6", EntryPoint = "read", SetLastError = true)]
        public static extern int Read(IntPtr handle, byte[] data, int length);

        [DllImport("libc.so.6", EntryPoint = "write", SetLastError = true)]
        public static extern int Write(IntPtr handle, byte[] data, int length);
    }
}