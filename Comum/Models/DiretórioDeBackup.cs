using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comum.Models
{
    [Serializable]
    public class DiretórioDeBackup : BindableBase
    {
        private string _Caminho;

        /// <summary>
        /// Caminho completo do diretório;
        /// </summary>
        public string Caminho
        {
            get { return this._Caminho; }
            set
            {
                if (this._Caminho != value)
                {
                    base.OnPropertyChanging();
                    bool adicionarBarra = (value.LastOrDefault() != '\\');
                    this._Caminho = value + (adicionarBarra ? "\\" : string.Empty);
                    base.OnPropertyChanged();
                }
            }
        }

        private bool _SubPastas;

        /// <summary>
        /// Inclui subpastas ao backup;
        /// </summary>
        public bool SubPastas
        {
            get { return this._SubPastas; }
            set
            {
                if (this._SubPastas != value)
                {
                    base.OnPropertyChanging();
                    this._SubPastas = value;
                    base.OnPropertyChanged();
                }
            }
        }



        public DiretórioDeBackup()
        { }

        public DiretórioDeBackup(string caminho, bool incluirSubPastas = true)
            : this()
        {
            this.Caminho = caminho;
            this.SubPastas = incluirSubPastas;
        }
    }
}