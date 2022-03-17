using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using MonitorDeCompilação.ModelsCore;


namespace MonitorDeCompilação.Models
{
    public class Sistema : AbstractBase
    {
        private string nome;
        private bool? liberado = null;

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

        public bool Liberado    // não é mapeado com o banco de dados;
        {
            get
            {
                bool status = Repository.GetBloqueios().Where(x => x.IdSistema == this.Id).Count() == 0;

                if (this.liberado != status)
                {
                    this.liberado = status;
                    OnPropertyChanged("Liberado");
                }

                return status;
            }
        }

        public BindingList<Bloqueio> Bloqueios
        {
            get
            {
                return Repository.GetBloqueios().Where(x => x.IdSistema == this.Id).ToBindingList<Bloqueio>();
            }

        }
    }
}
