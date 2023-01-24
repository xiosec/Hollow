using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Hollow
{
    public class Ntifs
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern uint NtReadVirtualMemory(
            IntPtr ProcessHandle,
            IntPtr BaseAddress,
            IntPtr Buffer,
            UInt32 NumberOfBytesToRead,
            ref UInt32 liRet
        );

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern uint NtWriteVirtualMemory(
            IntPtr ProcessHandle,
            IntPtr BaseAddress,
            IntPtr BufferAddress,
            UInt32 nSize,
            ref UInt32 lpNumberOfBytesWritten
        );
    }
}
