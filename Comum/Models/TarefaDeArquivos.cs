using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Comum.Utils;

namespace Comum.Models
{
    public class TarefaDeArquivos: Tarefa 
    {
        private EventoDeArquivos EventoRef { get; set; }

        public string Nome
        {
            get { return this.EventoRef.Nome; }
        }

        public string DiretórioDestino
        {
            get { return this.EventoRef.DiretórioDestino; }
        }

        public bool EfetuarBackup
        {
            get { return this.EventoRef.EfetuarBackup; }
        }
        //-------------------------------------------------------------------------//


        public BindingList<DiretórioDeBackup> Diretórios
        {
            get { return this.EventoRef.Diretórios ; }
        }

        public bool ExcluirBackupsAnteriores
        {
            get { return this.EventoRef.ExcluirBackupsAnteriores; }
        }

        public string Extensões
        {
            get { return this.EventoRef.Extensões ; }
        }


        public TarefaDeArquivos(EventoDeArquivos evento, DiaDaSemana dia, Horário horário)
            : base(dia, horário)
        {
            this.EventoRef = ObjectCopier.Clone<EventoDeArquivos>(evento);
        }
    }
}
