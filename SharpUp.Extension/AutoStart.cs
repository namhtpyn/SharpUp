using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpUp.Extension
{
    public static class AutoStart
    {
        private static string ExecutablePath { get { return Assembly.GetEntryAssembly().Location; } }
        private static string ExecutableName { get { return System.IO.Path.GetFileNameWithoutExtension(ExecutablePath); } }

        private static RegistryKey _registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public static bool IsRegistered { get { return ((string)_registryKey.GetValue(ExecutableName) == ExecutablePath); } }

        public static void Register()
        {
            if (!IsRegistered) _registryKey.SetValue(ExecutableName, ExecutablePath);
        }

        public static void Deregister()
        {
            if (IsRegistered) _registryKey.DeleteValue(ExecutableName, false);
        }
    }
}