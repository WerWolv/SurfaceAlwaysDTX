using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceAlwaysDTX
{
    /// <summary>
    /// Windows API / syscall wrappers
    /// </summary>
    public class WinAPI
    {
        public static readonly int INVALID_HANDLE_VALUE = -1;

        /// <summary>
        /// NtCreateFile syscall
        /// </summary>
        /// <param name="filename">Name of file</param>
        /// <param name="access">Requested file access</param>
        /// <param name="share">Requested file share</param>
        /// <param name="securityAttributes">Security attributes</param>
        /// <param name="creationDisposition">Creation disposition</param>
        /// <param name="flagsAndAttributes">File attributes</param>
        /// <param name="templateFile">Template file</param>
        /// <returns>Handle to file</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateFileW(
            [MarshalAs(UnmanagedType.LPWStr)] string filename,
            [MarshalAs(UnmanagedType.U4)] FileAccess access,
            [MarshalAs(UnmanagedType.U4)] FileShare share,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
            IntPtr templateFile);

        /// <summary>
        /// NtCloseHandle syscall
        /// </summary>
        /// <param name="hObject">Handle to close</param>
        /// <returns>If operation was successful</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// NtDeviceIoControlFile syscall
        /// </summary>
        /// <param name="hDevice">Device handle</param>
        /// <param name="dwIoControlCode">Io control code</param>
        /// <param name="lpInBuffer">In buffer</param>
        /// <param name="nInBufferSize">In buffer size</param>
        /// <param name="lpOutBuffer">Out buffer</param>
        /// <param name="nOutBufferSize">Out buffer size</param>
        /// <param name="lpBytesReturned">Number of bytes returned</param>
        /// <param name="lpOverlapped">Overlapped struct</param>
        /// <returns>If operation was successful</returns>
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeviceIoControl(
            IntPtr hDevice, uint dwIoControlCode,
            IntPtr lpInBuffer, uint nInBufferSize,
            IntPtr lpOutBuffer, uint nOutBufferSize,
            out uint lpBytesReturned, IntPtr lpOverlapped);

    }
}
