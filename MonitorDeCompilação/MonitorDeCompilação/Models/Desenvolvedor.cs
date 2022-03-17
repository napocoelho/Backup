using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConnectionManagerDll;
using System.Data;

using System.ComponentModel;
using MonitorDeCompilação.ModelsCore;

namespace MonitorDeCompilação.Models
{
    public class Desenvolvedor : AbstractBase
    {
        private string nome, email;

        public string Nome
        {
            get { return nome; }
            set
            {
                if (nome != value)
                {
                    nome = value;
                    OnPropertyChanged("Nome");
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        /*
        public BindingList<StatusDeCompilação> Status
        {
            get 
            {
                return Repository.GetStatus().Where(x => x.IdDesenvolvedor == this.Id).ToBindingList();
            }
        }
         */

        public BindingList<Bloqueio> Bloqueios
        {
            get
            {
                return Repository.GetBloqueios().Where(x => x.IdDesenvolvedor == this.Id).ToBindingList();
            }
        }
    }
}
