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
    public class ConfigBancosController : BindableBase
    {
        private EventoDeBancos Eventos { get; set; }            //--> config que poderá ser alterado sem afetar o último config salvo;
        //private EventoDeBanco EventosSalvados { get; set; }    //--> último config que foi salvo;


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

        public string Servidor
        {
            get { return this.Eventos.Servidor; }
            set
            {
                if (value != this.Eventos.Servidor)
                {
                    OnPropertyChanging();
                    this.Eventos.Servidor = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DiretórioDestino
        {
            get { return this.Eventos.DiretórioDestino; }
            set
            {
                if (value != this.Eventos.DiretórioDestino)
                {
                    OnPropertyChanging();
                    this.Eventos.DiretórioDestino = value;
                    OnPropertyChanged();
                }
            }
        }

        public BindingList<string> Bancos { get { return this.Eventos.Bancos; } }
        public BindingList<Horário> Horários { get { return this.Eventos.Horários; } }
        public BindingList<DiaDaSemana> Dias { get { return this.Eventos.Dias; } }


        public bool OtimizaçãoDiária
        {
            get { return this.Eventos.OtimizaçãoDiária; }
            set
            {
                if (this.Eventos.OtimizaçãoDiária != value)
                {
                    base.OnPropertyChanging();
                    this.Eventos.OtimizaçãoDiária = value;
                    base.OnPropertyChanged();
                }
            }
        }




        public bool OtimizaçãoSemanal
        {
            get { return this.Eventos.OtimizaçãoSemanal; }
            set
            {
                if (this.Eventos.OtimizaçãoSemanal != value)
                {
                    base.OnPropertyChanging();
                    this.Eventos.OtimizaçãoSemanal = value;
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

        public bool ExclusãoSemanal
        {
            get { return this.Eventos.ExclusãoSemanal; }
            set
            {
                if (this.Eventos.ExclusãoSemanal != value)
                {
                    base.OnPropertyChanging();
                    this.Eventos.ExclusãoSemanal = value;
                    base.OnPropertyChanged();
                }
            }
        }

        public bool ExclusãoMensal
        {
            get { return this.Eventos.ExclusãoMensal; }
            set
            {
                if (this.Eventos.ExclusãoMensal != value)
                {
                    base.OnPropertyChanging();
                    this.Eventos.ExclusãoMensal = value;
                    base.OnPropertyChanged();
                }
            }
        }



        public ConfigBancosController()
            : this(null)
        { }

        public ConfigBancosController(EventoDeBancos configBanco)
        {
            if (configBanco != null)
            {
                //this.EventosSalvados = configBanco;

                // O config temporário deve ser clonado apartir do original:
                this.Eventos = ObjectCopier.Clone<EventoDeBancos>(configBanco);
            }
            else
            {
                this.Eventos = new EventoDeBancos();
            }
        }

        /// <summary>
        /// Faz um teste básico com a configuração definida. Evita falhas de configuração.
        /// </summary>
        public bool Verificar()
        {
            /*
            Tarefa tarefa = new Tarefa();
            tarefa.ConfigBanco = this.Eventos;
            tarefa.Dia = Semana.Hoje;
            tarefa.Horário = Horário.Agora;


            //Log log = new Log();
            OperaçõesDeBanco operação = new OperaçõesDeBanco(this, new Log());
            bool válido = true;
            operação.Log.ErrorLog += (sender, status, message, time) =>
            {
                válido = false;
            };

            //AgendaService agenda = new AgendaService(log);

            //operação.
            */
            
            return true;
        }


        /// <summary>
        /// Salvando as configurações em arquivo tipo XML.
        /// </summary>
        public void Salvar()
        {
            if (this.Eventos.Nome.Trim() == string.Empty)
            {
                throw new FieldException("O campo nome não foi preenchido!", "Nome");
            }

            if (this.Eventos.Bancos.Count() == 0)
            {
                throw new FieldException("Ao menos um Banco deve ser selecionado!", "Bancos");
            }

            if (this.Eventos.Servidor.Trim() == string.Empty)
            {
                throw new FieldException("O campo Servidor não foi preenchido!", "Servidor");
            }

            if (!System.IO.Directory.Exists(this.Eventos.DiretórioDestino))
            {
                throw new FieldException("O campo Destino deve conter um diretório válido!", "Destino");
            }



            EventoDeBancos evento = Repository.GetInstance.Agenda.Eventos.Where(x => x.Id == this.Eventos.Id).FirstOrDefault() as EventoDeBancos;

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

    }
}