using Quick_Sound_Output_Change_Windows_11.Services.CoreAudioApi;
using Quick_Sound_Output_Change_Windows_11.Services.CoreAudioApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Quick_Sound_Output_Change_Windows_11.Services
{
    public static class AudioManager
    {
        private static readonly List<AudioDeviceInfo> _cachedDevices = new();

        public static void SafeRelease(object? obj)
        {
            if (obj != null && Marshal.IsComObject(obj))
            {
                Marshal.ReleaseComObject(obj);
            }
        }
        public static bool SetDefaultAudioDevice(string deviceId)
        {
            try
            {
                Debug.WriteLine("Creating PolicyConfigClient...");
                var policyConfig = new PolicyConfigClient() as IPolicyConfig;

                if (policyConfig == null)
                {
                    #if DEBUG
                    Debug.WriteLine("Failed to cast to IPolicyConfig.");
                    #endif
                    return false;
                }

                Debug.WriteLine("Calling SetDefaultEndpoint...");
                policyConfig.SetDefaultEndpoint(deviceId, Role.Multimedia);
                policyConfig.SetDefaultEndpoint(deviceId, Role.Console);
                policyConfig.SetDefaultEndpoint(deviceId, Role.Communications);

                #if DEBUG
                Debug.WriteLine("Success setting default audio device.");
                #endif
                return true;
            }
            catch (Exception ex)
            {
                #if DEBUG
                Debug.WriteLine($"Exception in SetDefaultAudioDevice: {ex}");
                #endif
                return false;
            }
        }

        public static List<AudioDeviceInfo> GetOutputAudioDevices()
        {
            _cachedDevices.Clear();

            var enumerator = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;

            if (enumerator == null)
            {
                #if DEBUG
                Debug.WriteLine("Failed to get IMMDeviceEnumerator instance.");
                #endif

                return _cachedDevices;
            }

            if (enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active, out var devices) != 0 || devices == null)
            {
                #if DEBUG
                Debug.WriteLine("Failed to enumerate audio endpoints.");
                #endif

                return _cachedDevices;
            }

            if (enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia, out var defaultDevice) != 0 || defaultDevice == null)
            {
                #if DEBUG
                Debug.WriteLine("Failed to get default audio endpoint.");
                #endif

                return _cachedDevices;
            }

            defaultDevice.GetId(out string defaultId);

            devices.GetCount(out int count);

            for (int i = 0; i < count; i++)
            {
                devices.Item(i, out var device);
                device.OpenPropertyStore(StorageAccessMode.Read, out var store);
                store.GetValue(PropertyKeys.PKEY_Device_FriendlyName, out var prop);
                device.GetId(out string deviceId);

                _cachedDevices.Add(new AudioDeviceInfo
                {
                    Name = prop.GetValue() ?? string.Empty,
                    Id = deviceId,
                    IsDefault = (deviceId == defaultId)
                });
                SafeRelease(store);
                SafeRelease(prop);
                SafeRelease(device);
            }

            return _cachedDevices;
        }

        public class AudioDeviceInfo
        {
            public string Name { get; set; } = string.Empty;
            public string Id { get; set; } = string.Empty;
            public bool IsDefault { get; set; }
        }

    }
}
