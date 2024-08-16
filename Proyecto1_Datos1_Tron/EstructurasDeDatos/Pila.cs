using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron.EstructurasDeDatos
{
    public class Pila<T>
    {
        public ListaEnlazada<T> elementos = new ListaEnlazada<T>();

        public bool VacioPila()
        {
            return elementos.Contador == 0;
        }

        public void Meter(T elemento)
        {
            elementos.AgregarPrimero(elemento);
        }

        public T Eliminar()
        {
            if (VacioPila())
            {
                throw new InvalidOperationException("La pila está vacía");
            }
            T valor = elementos.ObtenerPrimero();
            elementos.RemoverPrimero();
            return valor;
        }

        public T CimaPila()
        {
            if (VacioPila())
            {
                throw new InvalidOperationException("La pila está vacía");
            }
            return elementos.ObtenerPrimero();
        }
    }
}
