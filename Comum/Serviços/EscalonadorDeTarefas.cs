using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Comum.Models;

namespace Comum.Serviços
{
    public class EscalonadorDeTarefas
    {
        private Horário ÚltimoHorárioEscalonado { get; set; }

        /// <summary>
        /// Dia corrente;
        /// </summary>
        private DiaDaSemana Dia { get { return Semana.Hoje; } }

        /// <summary>
        /// Todas as tarefas da semana;
        /// </summary>
        private List<Tarefa> TarefasDaSemana { get; set; }

        /// <summary>
        /// Todas as tarefas do dia;
        /// </summary>
        private List<Tarefa> TarefasDoDia
        {
            get
            {
                return TarefasDaSemana.Where(x => x.Dia == this.Dia).OrderBy(x => x.Horário).ToList();
            }
        }

        /// <summary>
        /// Obtém as tarefas escaladas para este exato momento. Só podem ser obtidas uma única vez para cada horário.
        /// </summary>
        public bool TentarObterTarefasEscaladas(out List<Tarefa> tarefas)
        {
            if (this.ÚltimoHorárioEscalonado != Horário.Agora)
            {
                this.ÚltimoHorárioEscalonado = Horário.Agora;
                tarefas = this.TarefasDoDia.Where(x => x.Horário == this.ÚltimoHorárioEscalonado).ToList();
                return (tarefas.Count > 0);
            }

            tarefas = null;
            return false;
        }



        public EscalonadorDeTarefas(List<Tarefa> tarefasDaSemana)
        {
            this.TarefasDaSemana = tarefasDaSemana.OrderBy(x => x.Dia).OrderBy(x => x.Horário).ToList();
            this.ÚltimoHorárioEscalonado = null;
        }
    }
}