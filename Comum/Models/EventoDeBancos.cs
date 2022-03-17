using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;


// http://www.codeproject.com/Articles/483055/XML-Serialization-and-Deserialization-Part


namespace Comum.Models
{
    //[XmlRoot("Root")]
    [Serializable]
    public class EventoDeBancos : Evento  //: IXmlSerializable        // antigo ConfigBanco
    {


        private string _servidor;
        public string Servidor
        {
            get { return this._servidor; }
            set
            {
                if (this._servidor != value)
                {
                    base.OnPropertyChanging();
                    this._servidor = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private BindingList<string> _bancos;
        public BindingList<string> Bancos
        {
            get { return this._bancos; }
            private set
            {
                if (this._bancos != value)
                {
                    base.OnPropertyChanging();
                    this._bancos = value;
                    base.OnPropertyChanged();
                }
            }
        }



        private bool _otimizaçãoDiária;
        public bool OtimizaçãoDiária
        {
            get { return this._otimizaçãoDiária; }
            set
            {
                if (this._otimizaçãoDiária != value)
                {
                    base.OnPropertyChanging();
                    this._otimizaçãoDiária = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private bool _otimizaçãoSemanal;
        public bool OtimizaçãoSemanal
        {
            get { return this._otimizaçãoSemanal; }
            set
            {
                if (this._otimizaçãoSemanal != value)
                {
                    base.OnPropertyChanging();
                    this._otimizaçãoSemanal = value;
                    base.OnPropertyChanged();
                }
            }
        }


        private bool _exclusãoSemanal;
        public bool ExclusãoSemanal
        {
            get { return this._exclusãoSemanal; }
            set
            {
                if (this._exclusãoSemanal != value)
                {
                    base.OnPropertyChanging();
                    this._exclusãoSemanal = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private bool _exclusãoMensal;
        public bool ExclusãoMensal
        {
            get { return this._exclusãoMensal; }
            set
            {
                if (this._exclusãoMensal != value)
                {
                    base.OnPropertyChanging();
                    this._exclusãoMensal = value;
                    base.OnPropertyChanged();
                }
            }
        }


        public EventoDeBancos()
        {
            this.Tipo = TipoDeEvento.Banco;
            this.Servidor = String.Empty;
            this.Bancos = new BindingList<string>();
        }
    }
}