using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;


namespace Comum.Models
{
    [Serializable]
    public class EventoDeArquivos : Evento
    {
        private string _extensões;
        public string Extensões
        {
            get { return this._extensões; }
            set
            {
                if (this._extensões != value)
                {
                    base.OnPropertyChanging();
                    this._extensões = value;
                    base.OnPropertyChanged();
                }
            }
        }


        private bool _excluirBackupsAnteriores;

        /// <summary>
        /// Exclui todos backups feitos anteriormente. Apenas o último backup será mantido.
        /// </summary>
        public bool ExcluirBackupsAnteriores
        {
            get { return this._excluirBackupsAnteriores; }
            set
            {
                if (this._excluirBackupsAnteriores != value)
                {
                    base.OnPropertyChanging();
                    this._excluirBackupsAnteriores = value;
                    base.OnPropertyChanged();
                }
            }
        }
        

        /// <summary>
        /// Diretórios que serão armazenados no backup;
        /// </summary>
        public BindingList<DiretórioDeBackup> Diretórios { get; private set; }
        

        public EventoDeArquivos()
            : base()
        {
            base.Tipo = TipoDeEvento.Arquivo;
            this.DiretórioDestino = String.Empty;
            this.Diretórios = new BindingList<DiretórioDeBackup>();
            this.Extensões = string.Empty;
        }
    }
}