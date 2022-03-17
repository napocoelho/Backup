using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.Xml.Serialization;
using Comum.Utils;


namespace Comum.Models
{
    [Serializable]
    public class Horário : BindableBase, IComparable, IComparable<Horário>
    {
        private int horas, minutos;

        public static Horário Agora { get { return new Horário(DateTime.Now.Hour, DateTime.Now.Minute); } }
        public TimeSpan TimeSpan { get { return TimeSpan.FromHours(this.Horas).Add(TimeSpan.FromMinutes(this.Minutos)); } }

        public Horário()
        {
            this.Horas = 0;
            this.Minutos = 0;
        }

        public Horário(int horas, int minutos)
        {
            this.horas = horas;
            this.minutos = minutos;
        }



        public override string ToString()
        {
            return this.HorasText + ":" + this.MinutosText;
        }

        [XmlIgnore]
        public string HorasText
        {
            get { return ("00" + this.Horas).TakeRight(2); }
            set
            {
                int convertedValue;
                bool itWorks = int.TryParse(value, out convertedValue);

                if (itWorks && convertedValue != this.Horas)
                {
                    OnPropertyChanging();
                    this.Horas = convertedValue;
                    OnPropertyChanged();
                }
            }
        }

        [XmlIgnore]
        public string MinutosText
        {
            get { return ("00" + this.Minutos).TakeRight(2); }
            set
            {
                int convertedValue;
                bool itWorks = int.TryParse(value, out convertedValue);

                if (itWorks && convertedValue != this.Minutos)
                {
                    OnPropertyChanging();
                    this.Minutos = convertedValue;
                    OnPropertyChanged();
                }
            }
        }

        public int Horas
        {
            get { return this.horas; }
            set
            {
                if (this.horas != value)
                {
                    if (value >= 0 && value <= 23)
                    {
                        OnPropertyChanging();
                        this.horas = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        public int Minutos
        {
            get { return this.minutos; }
            set
            {
                if (this.minutos != value)
                {
                    if (value >= 0 && value <= 59)
                    {
                        OnPropertyChanging();
                        this.minutos = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        public void AddHoras(int value = 1)
        {
            TimeSpan time = TimeSpan.FromHours(this.Horas).Add(TimeSpan.FromHours(value));
            this.Horas = time.Hours;
        }

        public void AddMinutos(int value = 1)
        {
            TimeSpan time = TimeSpan.FromHours(this.Horas).Add(TimeSpan.FromMinutes(this.Minutos));
            time = time.Add(TimeSpan.FromMinutes(value));

            this.Horas = time.Hours;
            this.Minutos = time.Minutes;
        }

        /// <summary>
        /// Retorna a diferença "corrida" entre dois horários. Se o valor do 1o parâmetro for maior que o do 2o parâmetro, o 2o será considerado do dia posterior.
        /// </summary>
        public TimeSpan RemainsTo(Horário horário)
        {
            Horário dif = new Horário(this.Horas, this.Minutos);

            //if (Horário.LessThan(this, horário))
            if (this < horário)
            {
                return TimeSpan.FromMinutes(horário.TotalMinutos - this.TotalMinutos);
            }
            //else if (Horário.GreaterThan(this, horário))
            else if (this > horário)
            {
                return TimeSpan.FromMinutes((new Horário(23, 59)).TotalMinutos + 1 - this.TotalMinutos + horário.TotalMinutos);
            }

            // se for igual
            return new TimeSpan();
        }



        public double TotalHoras
        {
            get { return ((double)this.Horas) + ((double)60 / this.Minutos); }
        }

        public double TotalMinutos
        {
            get { return ((double)this.Horas * 60) + ((double)this.Minutos); }
        }

        public override int GetHashCode()
        {
            return this.Horas.GetHashCode() * this.Minutos.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;

            Horário other = (Horário)obj;

            if (this.Horas == other.Horas && this.Minutos == other.Minutos)
                return true;

            return false;
        }

        public static Horário Parse(DateTime datetime)
        {
            return new Horário(datetime.Hour, datetime.Minute);
        }

        public static Horário Parse(TimeSpan timeSpan)
        {
            return new Horário(timeSpan.Hours, timeSpan.Minutes);
        }

        public int CompareTo(object obj)
        {
            if (this.GetType() != obj.GetType())
                throw new InvalidCastException();

            Horário other = (Horário)obj;

            return CompareTo(other);
        }

        public int CompareTo(Horário other)
        {
            return this.TimeSpan.CompareTo(other.TimeSpan);
        }



        public static bool operator ==(Horário h1, Horário h2)
        {
            if (object.ReferenceEquals(h1, h2))
                return true;

            if (object.ReferenceEquals(h1, null))
                return false;

            if (object.ReferenceEquals(h2, null))
                return false;

            return h1.CompareTo(h2) == 0;
        }

        public static bool operator !=(Horário h1, Horário h2)
        {
            return !(h1 == h2);
        }

        public static bool operator <(Horário h1, Horário h2)
        {
            return h1.CompareTo(h2) < 0;
        }

        public static bool operator <=(Horário h1, Horário h2)
        {
            return h1.CompareTo(h2) <= 0;
        }

        public static bool operator >(Horário h1, Horário h2)
        {
            return h1.CompareTo(h2) > 0;
        }

        public static bool operator >=(Horário h1, Horário h2)
        {
            return h1.CompareTo(h2) >= 0;
        }
    }
}