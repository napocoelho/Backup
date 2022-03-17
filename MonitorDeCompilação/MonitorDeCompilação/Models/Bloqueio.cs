using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using MonitorDeCompilação.ModelsCore;

namespace MonitorDeCompilação.Models
{
    public class Bloqueio : AbstractBase
    {
        private int idSistema, idDesenvolvedor;
        
        public int IdSistema
        {
            get { return idSistema; }
            set
            {
                if (idSistema != value)
                {
                    idSistema = value;
                    OnPropertyChanged("IdSistema");
                }
            }
        }

        public int IdDesenvolvedor
        {
            get { return idDesenvolvedor; }
            set
            {
                if (idDesenvolvedor != value)
                {
                    idDesenvolvedor = value;
                    OnPropertyChanged("IdDesenvolvedor");
                }
            }
        }
        
        public Sistema Sistema
        {
            get { return Repository.GetSistemas().Where(x => x.Id == this.IdSistema).FirstOrDefault(); }
        }

        public Desenvolvedor Desenvolvedor
        {
            get { return Repository.GetDesenvolvedores().Where(x => x.Id == this.IdDesenvolvedor).FirstOrDefault(); }
        }


        public Bloqueio()
        {  }

        public Bloqueio(Sistema sistema, Desenvolvedor desenvolvedor)
        {
            this.IdSistema = sistema.Id;
            this.IdDesenvolvedor = desenvolvedor.Id;
        }
    }
}
