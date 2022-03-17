using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDeCompilação.ModelsCore
{
    public interface ITypedCache
    {
        Type Type { get; }
    }
}
