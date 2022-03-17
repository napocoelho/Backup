using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comum.Utils;

namespace Comum.Models
{
    //[Serializable]
    public static class Semana
    {

        /*
        private DiaDaSemana dia;
        private Horário horário;

        public  Semana(DiaDaSemana dia )
        {
            this.dia = dia;
            this.horário = new Horário();
        }

        public Semana(DiaDaSemana dia, Horário horário)
        {
            this.dia = dia;
            this.horário = horário;
        }
        */


        public static DiaDaSemana Hoje { get { return (DiaDaSemana)DateTime.Today.DayOfWeek.ToDiaDaSemana(); } }
    }
}