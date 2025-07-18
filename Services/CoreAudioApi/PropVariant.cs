using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quick_Sound_Output_Change_Windows_11.Services.CoreAudioApi
{
    [StructLayout(LayoutKind.Explicit)]
    public struct PropVariant
    {
        [FieldOffset(0)]
        public ushort vt;
        [FieldOffset(2)]
        public ushort wReserved1;
        [FieldOffset(4)]
        public ushort wReserved2;
        [FieldOffset(6)]
        public ushort wReserved3;
        [FieldOffset(8)]
        public IntPtr p;
        [FieldOffset(16)]
        public int p2;

        public string? GetValue()
        {
            return Marshal.PtrToStringUni(p);
        }
    }
}
