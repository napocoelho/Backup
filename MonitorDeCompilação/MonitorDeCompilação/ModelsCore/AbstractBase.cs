using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace MonitorDeCompilação.ModelsCore
{
    public abstract class AbstractBase : INotifyPropertyChanged
    {
        #region Propriedades

        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        #endregion Propriedades

        #region Eventos

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Eventos

        #region Sobrecargas

        public override int GetHashCode()
        {
            if (Id != 0)
                return Id;

            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            AbstractBase other = obj as AbstractBase;

            Type type = obj.GetType();

            if (other == null)
                return false;

            if (object.ReferenceEquals(this, other))
                return true;

            if (this.Id == 0 || other.Id == 0)
                return false;

            if (this.Id == other.Id && this.GetType() == other.GetType())
                return true;

            return false;
        }

        #endregion Sobrecargas
    }
}
