using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

using Comum.Utils;
using Comum.Models;

namespace Comum.Controllers
{
    public class ConfigArquivosController : BindableBase
    {
        //private TarefaBackupDeArquivos Tarefa { get; set; }
        private EventoDeArquivos Eventos { get; set; }
        //private BindingList<EventoBase> Eventos { get; set; }

        public BindingList<Horário> Horários { get { return this.Eventos.Horários; } }
        public BindingList<DiaDaSemana> Dias { get { return this.Eventos.Dias; } }

        public string Nome
        {
            get { return this.Eventos.Nome; }
            set
            {
                if (value != this.Eventos.Nome)
                {
                    OnPropertyChanging();
                    this.Eventos.Nome = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Extensões
        {
            get { return this.Eventos.Extensões; }
            set
            {
                if (value != this.Eventos.Extensões)
                {
                    OnPropertyChanging();
                    this.Eventos.Extensões = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DiretórioDestino
        {
            get { return this.Eventos.DiretórioDestino; }
            set
            {
                if (this.Eventos.DiretórioDestino != value)
                {
                    base.OnPropertyChanging();
                    this.Eventos.DiretórioDestino = value;
                    base.OnPropertyChanged();
                }
            }
        }

        public bool ExcluirBackupsAnteriores
        {
            get { return this.Eventos.ExcluirBackupsAnteriores; }
            set
            {
                if (this.Eventos.ExcluirBackupsAnteriores != value)
                {
                    base.OnPropertyChanging();
                    this.Eventos.ExcluirBackupsAnteriores = value;
                    base.OnPropertyChanged();
                }
            }
        }

        public bool EfetuarBackup
        {
            get { return this.Eventos.EfetuarBackup; }
            set
            {
                if (this.Eventos.EfetuarBackup != value)
                {
                    base.OnPropertyChanging();
                    this.Eventos.EfetuarBackup = value;
                    base.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Diretórios que serão armazenados no backup;
        /// </summary>
        public BindingList<DiretórioDeBackup> Diretórios { get { return this.Eventos.Diretórios; } }


        private DiretórioDeBackup _DiretórioSelecionado;
        public DiretórioDeBackup DiretórioSelecionado
        {
            get { return this._DiretórioSelecionado; }
            set
            {
                if (this._DiretórioSelecionado != value)
                {
                    base.OnPropertyChanging();
                    this._DiretórioSelecionado = value;
                    base.OnPropertyChanged();
                }
            }
        }
        
        public ConfigArquivosController()
            : this(null)
        {
            this.DiretórioSelecionado = null;
        }

        public ConfigArquivosController(EventoDeArquivos evento)
        {
            if (evento != null)
            {
                // O config temporário deve ser clonado apartir do original:
                this.Eventos = ObjectCopier.Clone<EventoDeArquivos>(evento);
            }
            else
            {
                this.Eventos = new EventoDeArquivos();
            }
        }

        public void Salvar()
        {
            if (this.Eventos.Nome.Trim() == string.Empty)
            {
                throw new FieldException("O campo nome não foi preenchido!", "Nome");
            }
            
            if (!System.IO.Directory.Exists(this.Eventos.DiretórioDestino))
            {
                throw new FieldException("O campo Destino deve conter um diretório válido!", "Destino");
            }

            EventoDeArquivos evento = Repository.GetInstance.Agenda.Eventos.Where(x => x.Id == this.Eventos.Id).FirstOrDefault() as EventoDeArquivos;

            if (evento == null)
            {
                Repository.GetInstance.Agenda.Eventos.Add(this.Eventos);
            }
            else
            {
                this.Eventos.CopyPropertiesTo(evento);
            }
            
            Repository.Save();
        }


        public bool Verificar()
        {
            return false;
        }
    }

}