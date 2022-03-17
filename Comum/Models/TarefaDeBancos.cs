using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Comum.Utils;

namespace Comum.Models
{
    [Serializable]
    public class TarefaDeBancos : Tarefa
    {
        private EventoDeBancos EventoRef { get; set; }


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







        public string Servidor
        {
            get { return this.EventoRef.Servidor; }
        }

        public BindingList<string> Bancos
        {
            get { return this.EventoRef.Bancos; }
        }

        public bool OtimizaçãoDiária
        {
            get { return this.EventoRef.OtimizaçãoDiária; }
        }

        public bool OtimizaçãoSemanal
        {
            get { return this.EventoRef.OtimizaçãoSemanal; }
        }

        public bool ExclusãoSemanal
        {
            get { return this.EventoRef.ExclusãoSemanal; }
        }

        public bool ExclusãoMensal
        {
            get { return this.EventoRef.ExclusãoMensal; }
        }




        public TarefaDeBancos(EventoDeBancos evento, DiaDaSemana dia, Horário horário)
            : base(dia, horário)
        {
            this.EventoRef = ObjectCopier.Clone<EventoDeBancos>(evento);
        }
    }
}