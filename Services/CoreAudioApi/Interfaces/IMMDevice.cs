﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quick_Sound_Output_Change_Windows_11.Services.CoreAudioApi.Interfaces
{
    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComImport]
    interface IMMDevice
    {
        int Activate(ref Guid id, ClsCtx clsCtx, IntPtr activationParams, [MarshalAs(UnmanagedType.IUnknown)] out object interfacePointer);

        int OpenPropertyStore(StorageAccessMode stgmAccess, out IPropertyStore properties);

        int GetId([MarshalAs(UnmanagedType.LPWStr)] out string id);

        int GetState(out DeviceState state);

    }
}
