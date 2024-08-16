using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class Nodos<T>
    {
        public T Valor { get; set; }
        public Nodos<T> Siguiente { get; set; }

        public Nodos(T valor)
        {
            this.Valor = valor;
            this.Siguiente = null;
        }
    }
}
