using System;
using System.Runtime.InteropServices;
using static Hollow.Winnt;

namespace Hollow
{
    public class PEParser
    {
        public static (
            IMAGE_DOS_HEADER,
            IMAGE_NT_HEADER64,
            IMAGE_FILE_HEADER,
            IntPtr
        ) PEHeaders(IntPtr FilePointer)
        {
            IMAGE_DOS_HEADER IDH = (IMAGE_DOS_HEADER)Marshal.PtrToStructure(
               FilePointer,
               typeof(IMAGE_DOS_HEADER));

            IntPtr INH_address = FilePointer + IDH.e_lfanew;
            IMAGE_NT_HEADER64 INH = (IMAGE_NT_HEADER64)Marshal.PtrToStructure(
                INH_address,
                typeof(IMAGE_NT_HEADER64));

            IntPtr IFH_address = (IntPtr)(INH_address + Marshal.SizeOf(INH.Signature));
            IMAGE_FILE_HEADER IFH = (IMAGE_FILE_HEADER)Marshal.PtrToStructure(
                IFH_address,
                typeof(IMAGE_FILE_HEADER));

            IntPtr ISH_address = (
                FilePointer + IDH.e_lfanew +
                Marshal.SizeOf(typeof(IMAGE_NT_HEADER64)));

            return (IDH, INH, IFH, ISH_address);
        }
    }
}
