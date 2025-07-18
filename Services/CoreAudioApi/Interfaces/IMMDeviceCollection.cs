using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quick_Sound_Output_Change_Windows_11.Services.CoreAudioApi.Interfaces
{
    [Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComImport]
    interface IMMDeviceCollection
    {
        int GetCount(out int numDevices);
        int Item(int deviceNumber, out IMMDevice device);
    }
}
