using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class Pila<T> : IEnumerable<T>
    {
        public ListaEnlazada<T> elementos = new ListaEnlazada<T>();

        public bool VacioPila()
        {
            return elementos.Contador == 0;
        }

        public void MeterPila(T elemento)
        {
            elementos.AgregarPrimero(elemento);
        }
        
        public void VaciarPila()
        {
            elementos = new ListaEnlazada<T>();
        }

        public void MeterPilaFinal(T elemento)
        {
            elementos.AgregarUltimo(elemento);
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
        public int ContadorPila()
        {
            return elementos.Contador;
        }
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= ContadorPila())
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

