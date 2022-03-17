using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;



namespace Comum.Models
{
    [Serializable]
    public abstract class Evento : BindableBase
    {
        private Guid _id;
        public Guid Id
        {
            get { return this._id; }
            set
            {
                if (this._id != value)
                {
                    base.OnPropertyChanging();
                    this._id = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private string _nome;

        /// <summary>
        /// Nome do evento criado pelo usuário.
        /// </summary>
        public string Nome
        {
            get { return this._nome; }
            set
            {
                if (this._nome != value)
                {
                    base.OnPropertyChanging();
                    this._nome = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private string _diretórioDestino;
        public string DiretórioDestino
        {
            get { return this._diretórioDestino; }
            set
            {
                if (this._diretórioDestino != value)
                {
                    base.OnPropertyChanging();
                    this._diretórioDestino = value;
                    base.OnPropertyChanged();
                }
            }
        }

        /*
        public BindingList<DiaDaSemana> Dias { get; private set; }
        public BindingList<Horário> Horários { get; private set; }
        */

        
        private BindingList<DiaDaSemana> _dias;
        public BindingList<DiaDaSemana> Dias
        {
            get { return this._dias; }
            set
            {
                if (this._dias != value)
                {
                    base.OnPropertyChanging();
                    this._dias = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private BindingList<Horário> _horários;
        public BindingList<Horário> Horários
        {
            get { return this._horários; }
            set
            {
                if (this._horários != value)
                {
                    base.OnPropertyChanging();
                    this._horários = value;
                    base.OnPropertyChanged();
                }
            }
        }
        

        private bool _efetuarBackup;
        public bool EfetuarBackup
        {
            get { return this._efetuarBackup; }
            set
            {
                if (this._efetuarBackup != value)
                {
                    base.OnPropertyChanging();
                    this._efetuarBackup = value;
                    base.OnPropertyChanged();
                }
            }
        }




        public Evento()
        {
            this.Id = Guid.NewGuid();
            this.Nome = string.Empty;
            this.DiretórioDestino = String.Empty;
            this.Dias = new BindingList<DiaDaSemana>();
            this.Horários = new BindingList<Horário>();
        }



        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Evento other = obj as Evento;

            if (object.ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            if (this.GetType() != other.GetType())
                return false;

            if (this.Id == other.Id)
                return true;

            return false;
        }

        public TipoDeEvento Tipo { get; set; }
    }

    public enum TipoDeEvento
    {
        Arquivo,
        Banco
    }
}