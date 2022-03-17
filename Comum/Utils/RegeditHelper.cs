using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

/*
namespace Comum.Utils
{
    public static class RegeditHelper
    {
        public static bool IsSetWindowsStartup(string appName)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);

            if (registryKey.GetValue(appName) != null)
            {
                return true;
            }

            return false;
        }

        public static void UnsetWindowsStartup(string appName)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (registryKey.GetValue(appName) != null)
            {
                registryKey.DeleteValue(appName);
            }
        }

        public static void SetWindowsStartup(string appName, string executablePath = null)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (executablePath != null)
            {
                registryKey.SetValue(appName, executablePath);
            }
        }

        private static  string diretórioRaiz = null;
        public static string  DiretórioRaiz
        {
            get
            {
                if (diretórioRaiz == null)
                {
                    RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"Software\GData\Backup", false);

                    if (rk != null && rk.Contains("AppDir"))
                    {
                        string caminhoPadrão = rk.GetValue("AppDir").ToString();

                        if (Directory.Exists(caminhoPadrão))
                        {
                            diretórioRaiz = caminhoPadrão;
                        }
                        else
                        {
                            System.Diagnostics.EventLog.WriteEntry("GData Ferramenta de Backup", "O diretório raiz informado no registro do windows é inválido", System.Diagnostics.EventLogEntryType.Warning);
                            Comum.Models.Parâmetros.GetInstance.Log.Warning(null, "O diretório raiz informado no registro do windows é inválido");
                        }
                    }
                    else
                    {
                        System.Diagnostics.EventLog.WriteEntry("GData Ferramenta de Backup", "O diretório raiz informado no registro do windows é inválido", System.Diagnostics.EventLogEntryType.Warning);
                        Comum.Models.Parâmetros.GetInstance.Log.Warning(null, "O diretório raiz informado no registro do windows é inválido");
                        diretórioRaiz = Directory.GetCurrentDirectory();
                    }
                }

                return diretórioRaiz;
            }
            set
            {
                if (value == null || !Directory.Exists(value))
                {
                    throw new DirectoryNotFoundException("Diretório [" + value + "] não encontrado");
                }


                RegistryKey rk = Registry.LocalMachine.CreateSubKey(@"Software\GData\Backup", RegistryKeyPermissionCheck.ReadWriteSubTree);
                rk.SetValue("AppDir", value);
                diretórioRaiz = value;
            }
        }
    }
}
*/