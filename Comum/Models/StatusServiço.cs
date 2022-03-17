using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comum.Models
{
    [Serializable]
    public enum StatusServiço
    {
        Nenhum = 0,
        Iniciando = 1,
        Rodando = 2,
        Parando = 3,
        Parado = 4,
        Pausando = 5,
        Pausado = 6,
        Reiniciando = 7
    }
}
