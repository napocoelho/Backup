using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Comum.Models
{
    public abstract class Tarefa : IComparable<TarefaDeBancos>
    {

        public Guid Id { get; set; }
        public DiaDaSemana Dia { get; set; }
        public Horário Horário { get; set; }



        public Tarefa(DiaDaSemana dia, Horário horário)
        {
            this.Id = Guid.NewGuid();
            this.Dia = dia;
            this.Horário = horário;
        }


        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            TarefaDeBancos other = obj as TarefaDeBancos;

            if (this.Id == other.Id)
                return true;

            return false;
        }

        public int CompareTo(TarefaDeBancos other)
        {
            if (object.ReferenceEquals(this, other))
                return 0;

            if (object.ReferenceEquals(this, null) && object.ReferenceEquals(other, null))
                return 0;

            if (object.ReferenceEquals(this, null) && !object.ReferenceEquals(other, null))
                return -1;

            if (!object.ReferenceEquals(this, null) && object.ReferenceEquals(other, null))
                return 1;

            if (this.Dia.GetHashCode() < other.Dia.GetHashCode())
            {
                return -1;
            }
            else if (this.Dia.GetHashCode() > other.Dia.GetHashCode())
            {
                return 1;
            }
            else
            {
                return this.Horário.CompareTo(other);
            }
        }

        public static List<Tarefa> GetTarefas(Evento evento)
        {
            List<Tarefa> tarefas = new List<Tarefa>();

            foreach (DiaDaSemana dia in evento.Dias)
            {
                foreach (Horário horário in evento.Horários)
                {
                    Tarefa tarefa = null;

                    if (evento.GetType() == typeof(EventoDeBancos))
                    {
                        tarefa = new TarefaDeBancos((EventoDeBancos)evento, dia, horário);
                    }
                    else if (evento.GetType() == typeof(EventoDeArquivos))
                    {
                        tarefa = new TarefaDeArquivos((EventoDeArquivos)evento, dia, horário);
                    }

                    tarefas.Add(tarefa);
                }
            }

            return tarefas;
        }

        public static List<TarefaDeBancos> GetTarefas(EventoDeBancos eventos)
        {
            List<TarefaDeBancos> tarefas = new List<TarefaDeBancos>();

            foreach (DiaDaSemana dia in eventos.Dias)
            {
                foreach (Horário horário in eventos.Horários)
                {
                    TarefaDeBancos tarefa = new TarefaDeBancos(eventos, dia, horário);
                    tarefas.Add(tarefa);
                }
            }

            return tarefas;
        }

        public static List<TarefaDeArquivos> GetTarefas(EventoDeArquivos eventos)
        {
            List<TarefaDeArquivos> tarefas = new List<TarefaDeArquivos>();

            foreach (DiaDaSemana dia in eventos.Dias)
            {
                foreach (Horário horário in eventos.Horários)
                {
                    TarefaDeArquivos tarefa = new TarefaDeArquivos(eventos, dia, horário);
                    tarefas.Add(tarefa);
                }
            }

            return tarefas;
        }
    }
}