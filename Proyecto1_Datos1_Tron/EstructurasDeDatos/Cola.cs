using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class Cola<T>
    {
        public ListaEnlazada<T> elementos = new ListaEnlazada<T>();

        public bool VacioCola()
        {
            return elementos.Contador == 0;
        }

        public void AgregarCola(T elemento)
        {
            elementos.AgregarUltimo(elemento);
        }
        public T QuitarCola()
        {
            if (VacioCola())
            {
                throw new InvalidOperationException("La cola esta vacia");
            }
            T valor = elementos.ObtenerPrimero();
            elementos.RemoverPrimero();
            return valor;
        }

        public T CimaCola()
        {
            if (VacioCola())
            {
                throw new InvalidOperationException("La cola esta vacia");
            }
            return elementos.ObtenerPrimero();
        }
        public T FondoCola()
        {
            if (VacioCola())
            {
                throw new InvalidOperationException("La cola esta vacia");
            }
            return elementos.ObtenerUltimo();
        }
    }
}
