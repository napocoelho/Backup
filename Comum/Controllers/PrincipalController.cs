using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.IO;

using Comum.Utils;
using Comum.Models;

namespace Comum.Controllers
{
    public class PrincipalController : BindableBase
    {
        private Agenda Agenda { get; set; }
        //public AgendaService AgendaServiceRef { get; private set; }
        public BindingList<LogDisplay> ListaLog { get; private set; }

        public AgendaService AgendaService { get { return AgendaService.GetInstance; } }


        private Evento _EventoSelecionado;
        public Evento EventoSelecionado
        {
            get { return this._EventoSelecionado; }
            set
            {
                if (value != this._EventoSelecionado)
                {
                    OnPropertyChanging();
                    this._EventoSelecionado = value;
                    OnPropertyChanged();
                }
            }
        }

        public BindingList<Evento> Eventos
        {
            get { return this.Agenda.Eventos; }
        }

        public PrincipalController(Log log)
        {
            this.Agenda = Repository.GetInstance.Agenda;
            this.ListaLog = new BindingList<LogDisplay>();

            if (this.Agenda == null)
            {
                Repository.GetInstance.Agenda = new Agenda();
            }

            Comum.Models.Parâmetros.GetInstance.Log.CompleteLog += Log_CompleteLog;
            AgendaService.CreateInstance(log);
        }

        public void RemoverEventoDeBancoSelecionado()
        {
            if (this.EventoSelecionado != null)
            {
                this.Eventos.Remove(this.EventoSelecionado);
                Repository.GetInstance.Agenda = this.Agenda;
            }
        }

        public void ExecutarBackup(Evento evento)
        {
            if (AgendaService.EmExecução)
            {
                throw new Exception("Já existe uma tarefa sendo executada neste momento!");
            }

            AgendaService.ExecutarBackup(evento);

            /*
            Task.Run(() =>
                {
                    AgendaService.ExecutarBackup(evento);
                });
            */
        }

        public void ExecutarBackup(Tarefa tarefa)
        {
            if (AgendaService.EmExecução)
            {
                throw new Exception("Já existe uma tarefa sendo executada neste momento!");
            }

            AgendaService.ExecutarBackup(tarefa);

            /*
            Task.Run(() =>
            {
                AgendaService.ExecutarBackup(tarefa);
            });
            */
        }

        /*
        public void IniciarAgenda()
        {
            this.AgendaService.IniciarExecução();
        }

        public void PararAgenda()
        {
            this.AgendaService.PararExecução();
        }
        */

        private void Log_CompleteLog(object sender, StatusLog status, string message, DateTime time)
        {
            try
            {
                string fileName = Parâmetros.GetInstance.DiretórioDoExecutável + @"\LogCompleto.txt";
                bool gravou = false;



                string[] content = {"{0} - {1}     {2}     {3}".FormatTo(  time.ToShortDateString(),
                                                                    time.ToShortTimeString(),
                                                                    status.ToString(),
                                                                    message
                                                                    )};

                lock (Repository.GetInstance)
                {
                    while (!gravou)
                    {
                        try
                        {
                            File.AppendAllLines(fileName, content, Encoding.UTF8);
                            //File.AppendAllText(fileName, content, Encoding.UTF8);

                            gravou = true;
                            System.Threading.Thread.Sleep(50);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            { }


            try
            {
                if (status != StatusLog.Debug)
                {
                    this.ListaLog.Add(new LogDisplay(status, message, time));
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}