using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace MonitorDeCompilação.ModelsCore
{
    public class TypedCache<T> : ITypedCache where T : AbstractBase, new()
    {
        public BindingList<T> Values { get; private set; }
        public bool IsCached { get; set; }

        public TypedCache()
        {
            Values = new BindingList<T>();
            IsCached = false;
        }

        public Type Type { get { return typeof(T); } }
    }
}
