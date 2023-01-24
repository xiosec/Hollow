using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Hollow
{
    public class Wdm
    {
        [DllImport("kernel32.dll")]
        public static extern void RtlZeroMemory(
            IntPtr pBuffer,
            int length
        );
        [DllImport("ntdll.dll")]
        public static extern uint ZwUnmapViewOfSection(
            IntPtr ProcessHandle,
            IntPtr BaseAddress
        );
    }
}
