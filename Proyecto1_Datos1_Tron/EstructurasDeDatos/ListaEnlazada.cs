using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class ListaEnlazada<T> : IEnumerable<T>
    {
        public Nodos<T> Cabeza { get; set; }
        public Nodos<T> Cola { get; set; }
        public int Contador { get; set; }

        public ListaEnlazada()
        {
            this.Cabeza = null;
            this.Cola = null;
            this.Contador = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Nodos<T> actual = Cabeza;
            while (actual != null)
            {
                yield return actual.Valor;
                actual = actual.Siguiente;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void AgregarPrimero(T valor)
        {

        // Agrega un nodo al principio de la lista
            Nodos<T> nuevoNodo = new Nodos<T>(valor);
            if (Cabeza == null)
            {
                Cabeza = nuevoNodo;
                Cola = nuevoNodo;
            }
            else
            {
                nuevoNodo.Siguiente = Cabeza;
                Cabeza = nuevoNodo;
            }
            Contador++;
        }

        public void AgregarUltimo(T valor) {
            // Agrega un nodo al final de la lista
            Nodos<T> nuevoNodo = new Nodos<T>(valor);
            if (Cabeza == null)
            {
                Cabeza = nuevoNodo;
                Cola = nuevoNodo;
            }
            else
            {
                Cola.Siguiente = nuevoNodo;
                Cola = nuevoNodo;
            }
            Contador++;
        }
        public void RemoverPrimero()
        {
            if (Cabeza == null) return;

            if (Cabeza == Cola)
            {
                Cabeza = null;
                Cola = null;
            }
            else
            {
                Cabeza = Cabeza.Siguiente;
            }
            Contador--;
        }
        public void RemoverUltimo()
        {
            if (Cabeza == null) return;

            if (Cabeza == Cola)
            {
                Cabeza = null;
                Cola = null;
            }
            else
            {
                Nodos<T> actual = Cabeza;
                while (actual.Siguiente != Cola)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = null;
                Cola = actual;
            }
            Contador--;
        }

        public T ObtenerPrimero()
        {
            return Cabeza != null ? Cabeza.Valor : default;
        }
        public T ObtenerUltimo()
        {
            return Cola != null ? Cola.Valor : default;
        }

        public Nodos<T> ObtenerPrimerNodo()
        {
            return Cabeza;
        }
        public Nodos<T> ObtenerUltimoNodo()
        {
            return Cola;
        }

        public Nodos<T> ObtenerSiguienteNodo(Nodos<T> nodo)
        {
            return nodo?.Siguiente;
        }
        public ListaEnlazada<T> Salto(int posicion)
        {
            ListaEnlazada<T> ListaSaltada = new ListaEnlazada<T>();
            Nodos<T> actual = Cabeza;
            
            for (int i = 0; i <= posicion; i++)
            {
                if (actual == null) break;
                actual = actual.Siguiente;
                
            }
            while(actual != null)
            {
                ListaSaltada.AgregarUltimo(actual.Valor);
                actual = actual.Siguiente;
            }
            return ListaSaltada;
           
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Contador)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                Nodos<T> actual = Cabeza;
                for (int i = 0; i < index; i++)
                {
                    actual = actual.Siguiente;
                }

                return actual.Valor;
            }
        }


    }
}


