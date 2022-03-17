using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using System.Threading;

using Comum.Models;
using Comum.Utils;

namespace Comum.Controllers
{
    public class AgendaService : BindableBase
    {   
        private Task ProvedorDeTarefas { get; set; }
        private Task ConsumidorDeTarefas { get; set; }
        private static AgendaService INSTANCE = null;

        public ConcurrentQueue<Tarefa> FilaDeExecução { get; private set; }


        /*******************************************************/
        private Log _Log = null;
        public Log Log
        {
            get { return this._Log; }
            private set
            {
                if (this._Log != value)
                {
                    base.OnPropertyChanging();
                    this._Log = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private StatusServiço _status;
        public StatusServiço Status
        {
            get
            {
                return this._status;
            }
            private set
            {
                if (this._status != value)
                {
                    base.OnPropertyChanging();
                    this._status = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private OrdemServiço ordem;
        public OrdemServiço Ordem
        {
            get { return this.ordem; }
            private set
            {
                if (this.ordem != value)
                {
                    OnPropertyChanging();
                    this.ordem = value;
                    OnPropertyChanged();
                }
            }
        }

        /*******************************************************/


        private BindingList<Tarefa> _Tarefas;
        public BindingList<Tarefa> Tarefas
        {
            get
            {
                return this._Tarefas;
            }
            private set
            {
                if (this._Tarefas != value)
                {
                    base.OnPropertyChanging();
                    this._Tarefas = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private DateTime inícioDoProcesso;
        public DateTime InícioDoProcesso
        {
            get { return this.inícioDoProcesso; }
            private set
            {
                if (this.inícioDoProcesso != value)
                {
                    base.OnPropertyChanging();
                    this.inícioDoProcesso = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private bool _EmExecução;
        public bool EmExecução
        {
            get { return this._EmExecução; }
            private set
            {
                if (this._EmExecução != value)
                {
                    base.OnPropertyChanging();
                    this._EmExecução = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private AgendaService(Log log)
        {
            this.Log = log;
            this.FilaDeExecução = new ConcurrentQueue<Tarefa>();
            this.Status = StatusServiço.Nenhum;
            this.Ordem = OrdemServiço.Executar;

            this.Tarefas = new BindingList<Tarefa>();

            //this.GerarAgenda();
        }

        public void GerarAgenda()
        {
            this.Log.Debug(this, "GerarAgenda");

            this.InícioDoProcesso = DateTime.Now;

            Agenda agenda = Repository.GetInstance.Agenda;

            List<Tarefa> eventosForaDaOrdem = new List<Tarefa>();

            // Obtendo tarefas a serem executadas;
            foreach (Evento evento in agenda.Eventos)
            {
                eventosForaDaOrdem.AddRange(Tarefa.GetTarefas(evento));
            }

            // Ordenando horário das tarefas (importantíssimo);
            this.Tarefas.Clear();
            this.Tarefas.AddRange(eventosForaDaOrdem.OrderBy(x => x.Dia.GetHashCode()).ThenBy(x => x.Horário));
        }

        /***********************************************************************************************/

        public void VerificarExecutar()
        {
            
            try
            {
                

                this.ProvedorDeTarefas = null;
                this.ConsumidorDeTarefas = null;

                //this.Log.Event(this, "Iniciando serviço de backup");
                this.Ordem = OrdemServiço.Executar;
                this.Status = StatusServiço.Iniciando;

                //this.GerarAgenda();
            }
            catch (Exception ex)
            {
                this.Ordem = OrdemServiço.Nenhum;
                this.Status = StatusServiço.Parado;
                this.Log.Error(this, "Falha ao iniciar serviço. Mensagem: " + ex.Message);
            }

            ////////////////////////////////////////////////////////////////////


            Comum.Serviços.EscalonadorDeTarefas escalonador = new Serviços.EscalonadorDeTarefas(this.Tarefas.ToList());

            //while (this.Ordem != OrdemServiço.Parar)
            {
                try
                {
                    //if (this.Status == StatusServiço.Parado)
                    {
                        //break;
                    }

                    //while (this.Ordem == OrdemServiço.Pausar)
                    {
                    //    this.Status = StatusServiço.Pausado;
                    //    Thread.Sleep(5000);
                    }

                    this.Status = StatusServiço.Rodando;

                    // Verifica se já é dia e hora de executar a tarefa, considerando atrasos de execução:
                    List<Tarefa> tarefasEscaladas = null;

                    if (escalonador.TentarObterTarefasEscaladas(out tarefasEscaladas))
                    {
                        foreach (Tarefa tarefa in tarefasEscaladas)
                        {
                            try
                            {
                                this.ExecutarBackup(tarefa);
                            }
                            catch (Exception ex)
                            {
                                this.Log.Error(this, "Erro ao executar 'ConsumidorDeTarefas'. Mensagem: " + ex.Message);
                            }
                        }
                    }

                    //Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    this.Log.Error(this, "Erro ao executar 'ProvedorDeTarefas'. Mensagem:  " + ex.Message);
                    System.Diagnostics.EventLog.WriteEntry("ProvedorDeTarefas", "Erro ao executar 'ProvedorDeTarefas'. Mensagem:  " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                }
            }

            //this.Status = StatusServiço.Parado;
            //this.Log.Event(this, "Serviço de backup parado");
        }

        /*
        public void IniciarExecução()
        {
            try
            {
                if (this.Status != StatusServiço.Parado && this.Status != StatusServiço.Nenhum)
                {
                    return;
                }

                this.ProvedorDeTarefas = null;
                this.ConsumidorDeTarefas = null;

                this.Log.Event(this, "Iniciando serviço de backup");
                this.Ordem = OrdemServiço.Executar;
                this.Status = StatusServiço.Iniciando;

                this.GerarAgenda();
            }
            catch (Exception ex)
            {
                this.Ordem = OrdemServiço.Nenhum;
                this.Status = StatusServiço.Parado;
                this.Log.Error(this, "Falha ao iniciar serviço. Mensagem: " + ex.Message);
            }

            // Thread 1:
            this.ProvedorDeTarefas = Task.Run(() =>
            {
                Comum.Serviços.EscalonadorDeTarefas escalonador = new Serviços.EscalonadorDeTarefas(this.Tarefas.ToList());

                while (this.Ordem != OrdemServiço.Parar)
                {
                    try
                    {
                        if (this.Status == StatusServiço.Parado)
                        {
                            break;
                        }

                        while (this.Ordem == OrdemServiço.Pausar)
                        {
                            this.Status = StatusServiço.Pausado;
                            Thread.Sleep(5000);
                        }

                        this.Status = StatusServiço.Rodando;

                        // Verifica se já é dia e hora de executar a tarefa, considerando atrasos de execução:
                        List<Tarefa> tarefasEscaladas = null;

                        if (escalonador.TentarObterTarefasEscaladas(out tarefasEscaladas))
                        {
                            foreach (Tarefa tarefa in tarefasEscaladas)
                            {
                                this.FilaDeExecução.Enqueue(tarefa);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log.Error(this, "Erro ao executar 'ProvedorDeTarefas'. Mensagem:  " + ex.Message);
                        System.Diagnostics.EventLog.WriteEntry("ProvedorDeTarefas", "Erro ao executar 'ProvedorDeTarefas'. Mensagem:  " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }

                    Thread.Sleep(5000); // 15 seg.
                }

                this.Status = StatusServiço.Parado;
                this.Log.Event(this, "Agendador parado");

            });


            // Thread 2:
            this.ConsumidorDeTarefas = Task.Run(() =>
            {
                this.Status = StatusServiço.Rodando;

                while (this.Ordem != OrdemServiço.Parar)
                {
                    try
                    {
                        if (this.Status == StatusServiço.Parado)
                        {
                            break;
                        }

                        while (this.Ordem == OrdemServiço.Pausar)
                        {
                            this.Status = StatusServiço.Pausado;
                            Thread.Sleep(5000);
                        }

                        this.Status = StatusServiço.Rodando;

                        Tarefa tarefa = null;

                        if (this.FilaDeExecução.TryDequeue(out tarefa))
                        {
                            //this.ExecutarBackup(this.Agenda.Eventos.First());
                            this.ExecutarBackup(tarefa);
                        }
                        else
                        {
                            Thread.Sleep(5000);
                        }

                    }
                    catch (Exception ex)
                    {
                        this.Log.Error(this, "Erro ao executar 'ConsumidorDeTarefas'. Mensagem: " + ex.Message);
                    }
                }


                this.Status = StatusServiço.Parado;
                this.Log.Event(this, "Serviço de backup parado");
            });
        }
        */

            /*
        public void PararExecução()
        {
            this.Log.Event(this, "Parando serviço de backup");
            this.Ordem = OrdemServiço.Parar;
            this.Status = StatusServiço.Parando;
        }
        */

        /*
        public void ReiniciarExecução()
        {
            this.PararExecução();
            this.IniciarExecução();
        }
        */

        /*
        public void PausarExecução()
        {
            this.Log.Debug(this, "PausarExecução");

            this.Ordem = OrdemServiço.Pausar;
            this.Status = StatusServiço.Pausando;

            while (this.Status != StatusServiço.Pausado)
            {
                Thread.Sleep(500);
            }
        }
        */
        /***********************************************************************************************/


        /// <summary>
        /// Executa procedimentos de backup de um evento. Este método é totalmente monitorado através do Log.
        /// </summary>
        public void ExecutarBackup(Evento evento, Log log = null)
        {
            Tarefa tarefa = Tarefa.GetTarefas(evento).FirstOrDefault();
            this.ExecutarBackup(tarefa, log);
        }





        /// <summary>
        /// Executa procedimentos de backup de uma tarefa. Este método é totalmente monitorado através do Log.
        /// </summary>
        public void ExecutarBackup(Tarefa tarefa, Log log = null)
        {
            if (tarefa != null)
            {
                this.EmExecução = true;

                Comum.Serviços.IExecutor executor = null;
                log = (log == null ? this.Log : log);

                if (tarefa.GetType() == typeof(TarefaDeBancos))
                {
                    executor = new Serviços.ExecutorDeTarefasDeBancos((TarefaDeBancos)tarefa, log, this);
                }
                else if (tarefa.GetType() == typeof(TarefaDeArquivos))
                {
                    executor = new Serviços.ExecutorDeTarefasDeArquivos((TarefaDeArquivos)tarefa, log, this);
                }

                if (executor != null)
                {
                    executor.Executar();
                }

                this.EmExecução = false;
            }
        }
        
        public static AgendaService CreateInstance(Log log)
        {
            AgendaService.INSTANCE = new AgendaService(log);
            return AgendaService.INSTANCE;
        }

        public static AgendaService GetInstance
        {
            get
            {
                if (AgendaService.INSTANCE == null)
                {
                    AgendaService.CreateInstance(Comum.Models.Parâmetros.GetInstance.Log);
                }

                return AgendaService.INSTANCE;
            }
        }
    }
}