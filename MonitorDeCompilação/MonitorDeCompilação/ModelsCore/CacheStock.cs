using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace MonitorDeCompilação.ModelsCore
{
    public class CacheStock
    {
        private Dictionary<Type, ITypedCache> Collection { get; set; }

        public CacheStock()
        {
            this.Collection = new Dictionary<Type, ITypedCache>();
        }

        public BindingList<T> Values<T>() where T : AbstractBase, new()
        {
            Type type = typeof(T);

            if (!this.Collection.ContainsKey(type))
            {
                this.Collection[type] = new TypedCache<T>();
            }

            return (this.Collection[type] as TypedCache<T>).Values;
        }

        public TypedCache<T> Items<T>() where T : AbstractBase, new()
        {
            Type type = typeof(T);

            if (!this.Collection.ContainsKey(type))
            {
                this.Collection[type] = new TypedCache<T>();
            }

            return this.Collection[type] as TypedCache<T>;
        }
    }
}
