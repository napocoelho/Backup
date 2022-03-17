using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Xml.Serialization;

namespace Comum.Models
{
    /// <summary>
    /// Representa a configuração de todos os eventos de uma agenda
    /// </summary>
    [Serializable]
    public class Agenda : BindableBase
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



        /*
        private BindingList<EventoDeBanco> _eventosDeBanco;
        public BindingList<EventoDeBanco> EventosDeBanco
        {
            get { return this._eventosDeBanco; }
            private set
            {
                if (this._eventosDeBanco != value)
                {
                    base.OnPropertyChanging();
                    this._eventosDeBanco = value;
                    base.OnPropertyChanged();
                }
            }
        }
        */


        private BindingList<Evento> _eventos;

        [XmlArrayItem(Type = typeof(Evento)), XmlArrayItem(Type = typeof(EventoDeBancos)), XmlArrayItem(Type = typeof(EventoDeArquivos))]   //==> por causa da hierarquia;
        public BindingList<Evento> Eventos
        {
            get { return this._eventos; }
            private set
            {
                if (this._eventos != value)
                {
                    base.OnPropertyChanging();
                    this._eventos = value;
                    base.OnPropertyChanged();
                }
            }
        }
        



        public Agenda()
        {
            this.Id = Guid.NewGuid();
            this.Eventos = new BindingList<Evento>();
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            Agenda other = obj as Agenda;

            if (other == null)
                return false;

            if (this.Id == other.Id)
                return true;

            return false;
        }
    }
}