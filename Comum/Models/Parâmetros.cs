using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

using Comum.Utils;

namespace Comum.Models
{
    public class Parâmetros : BindableBase 
    {
        private static  Parâmetros _INSTANCE_ = null;

        #region Propriedades

        public string NomeDoExecutável
        {
            get
            {
                return System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath);
            }
        }

        public string DiretórioDoExecutável
        {
            get
            {
                return System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            }
        }

        public string CaminhoDoExecutável
        {
            get
            {
                return System.Windows.Forms.Application.ExecutablePath;
                
            }
        }


        /// <summary>
        /// Iniciar ao logar no Windows.
        /// </summary>
        private bool _iniciarComWindows;
        public bool IniciarComWindows
        {
            get
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);
                bool isMarked = (registryKey.GetValue(this.NomeDoExecutável) != null);

                if (this._iniciarComWindows != isMarked)
                {
                    base.OnPropertyChanging();
                    this._iniciarComWindows = isMarked;
                    base.OnPropertyChanged();
                }

                return this._iniciarComWindows;
            }
            set
            {
                if (this.IniciarComWindows != value)
                {
                    base.OnPropertyChanging();
                    this._iniciarComWindows = value;


                    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                    if (value)
                    {
                        registryKey.SetValue(this.NomeDoExecutável, this.CaminhoDoExecutável);
                    }
                    else
                    {
                        if (registryKey.GetValue(this.NomeDoExecutável) != null)
                        {
                            registryKey.DeleteValue(this.NomeDoExecutável);
                        }
                    }

                    base.OnPropertyChanged();
                }
            }
        }


        private  Log log = null;
        public Log Log
        {
            get
            {
                if (log == null)
                {
                    log = new Log();
                }

                return log;
            }
        }


        #endregion Propriedades


        private Parâmetros()
        {
        }

        public static Parâmetros GetInstance 
        {
            get
            {
                if (_INSTANCE_ == null)
                {
                    _INSTANCE_ = new Parâmetros();
                }
                return _INSTANCE_;
            }
        }
    }
}
