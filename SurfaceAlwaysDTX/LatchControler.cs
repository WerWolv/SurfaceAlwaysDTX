using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceAlwaysDTX
{
    public class LatchControler
    {
        /// <summary>
        /// Latch device driver path
        /// </summary>
        private static readonly string LatchDeviceDriver = "\\\\?\\ACPI#MSHW0133#2&daba3ff&1#{f49e75f6-f869-4346-9eb8-ded248275916}";

        /// <summary>
        /// Latch command ioctl command
        /// </summary>
        private static readonly UInt32 LatchCommandIoctl = 0x8000A044;

        /// <summary>
        /// Latch command types
        /// </summary>
        private enum LatchCommandType
        {
            Invalid,
            Open,
            Close_DEPRECATED,
            ButtonPress,
            Cancel,
            MaximumValue,
        }

        /// <summary>
        /// Structure of input data expected by the LatchCommandIoctl command
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct LatchCommandInArgs
        {
            public LatchCommandType LatchCommand;
            public uint TimeoutMs;
        }

        /// <summary>
        /// Unlocks the latch so it may be opened in any case, even when the battery is bellow the required treashold
        /// </summary>
        public static void UnlockLatch()
        {
            var hLatchDriver = WinAPI.CreateFileW(LatchDeviceDriver, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, IntPtr.Zero, System.IO.FileMode.Open, System.IO.FileAttributes.Normal, IntPtr.Zero);

            if (hLatchDriver == IntPtr.Zero || hLatchDriver.ToInt32() == WinAPI.INVALID_HANDLE_VALUE)
                return;

            var args = new LatchCommandInArgs() { LatchCommand = LatchCommandType.Open, TimeoutMs = 5000 };
            var argsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(args));
            Marshal.StructureToPtr(args, argsPtr, false);

            uint bytesReturned = 0;

            WinAPI.DeviceIoControl(hLatchDriver, LatchCommandIoctl, argsPtr, (uint)Marshal.SizeOf(args), IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);

            WinAPI.CloseHandle(hLatchDriver);
        }

        /// <summary>
        /// Opens the latch to detach the screen
        /// </summary>
        public static void OpenLatch()
        {
            var hLatchDriver = WinAPI.CreateFileW(LatchDeviceDriver, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, IntPtr.Zero, System.IO.FileMode.Open, System.IO.FileAttributes.Normal, IntPtr.Zero);

            if (hLatchDriver == IntPtr.Zero || hLatchDriver.ToInt32() == WinAPI.INVALID_HANDLE_VALUE)
                return;

            var args = new LatchCommandInArgs() { LatchCommand = LatchCommandType.ButtonPress, TimeoutMs = 5000 };
            var argsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(args));
            Marshal.StructureToPtr(args, argsPtr, false);

            uint bytesReturned = 0;

            WinAPI.DeviceIoControl(hLatchDriver, LatchCommandIoctl, argsPtr, (uint)Marshal.SizeOf(args), IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);

            WinAPI.CloseHandle(hLatchDriver);
        }
    }
}
