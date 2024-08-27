using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class Cola<T> : IEnumerable<T>
    {
        public ListaEnlazada<T> elementos = new ListaEnlazada<T>();

      

        public void Enqueue(T elemento)
        {
            elementos.AgregarUltimo(elemento);
        }

        public void Dequeue()
        {
            if (VacioCola())
            {
                throw new InvalidOperationException("La cola esta vacia");
            }
            elementos.RemoverPrimero();
        }

        public T Peek()
        {
            if (VacioCola())
            {
                throw new InvalidOperationException("La cola esta vacia");
            }
            return elementos.ObtenerPrimero();
        }

        public T PeekDequeue()
        {
            if (VacioCola())
            {
                throw new InvalidOperationException("La cola esta vacia");
            }
            T valor = elementos.ObtenerPrimero();
            elementos.RemoverPrimero();
            return valor;
        }

        public bool VacioCola()
        {
            return elementos.Contador == 0;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= elementos.Contador)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return elementos[index]; // Asumiendo que ListaEnlazada<T> tiene un indexador
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return elementos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

