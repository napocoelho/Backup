using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comum.Models
{
    [Serializable]
    public enum OrdemServiço
    {
        Nenhum = 0,
        Executar = 1,
        Pausar = 2,
        Parar = 3,
        Reiniciar = 4
    }
}
