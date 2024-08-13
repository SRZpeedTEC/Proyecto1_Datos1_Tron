using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class Mapa
    {
        public LinkedList<NodoMapa> nodosMapa { get; set; } // Constructor del mapa, indica el ancho, el alto y el tamaño de los nodos 
        public int ancho { get; set; }
        public int alto { get; set; }
        public int tamanoNodo { get; set; }

        public Mapa(int ancho, int alto, int tamanoNodo)
        {
            this.ancho = ancho;
            this.alto = alto;
            this.tamanoNodo = tamanoNodo;

            nodosMapa = new LinkedList<NodoMapa>();     // Crea una lista de nodos que representaran el mapa 
            NodoMapa[,] nodosArreglo = new NodoMapa[ancho, alto];   // Crea una matriz de nodos que representaran el mapa 

            for (int x = 0; x < ancho; x++)     // crear los nodos del mapa con la Clase nodoMapa, les asigna una posicion en la matriz, en la lista enlazadda y una representacion grafica de un rectangulo
            {
                for (int y = 0; y < alto; y++)
                {
                    Rectangle rect = new Rectangle(x * tamanoNodo, y * tamanoNodo, tamanoNodo, tamanoNodo);
                    NodoMapa nuevoNodo = new NodoMapa(rect);
                    nodosArreglo[x, y] = nuevoNodo;
                    nodosMapa.AddLast(nuevoNodo);
                }
            }

            for (int x = 0; x < ancho; x++) // Le asigna a cada nodo sus nodos adyacentes recorriendo la matriz 
            {
                for (int y = 0; y < alto; y++)
                {
                    NodoMapa nodoRevisado = nodosArreglo[x, y];
                    if (y > 0)
                    {
                        nodoRevisado.arriba = nodosArreglo[x, y - 1];
                    }
                    if (y < alto - 1)
                    {
                        nodoRevisado.abajo = nodosArreglo[x, y + 1];
                    }
                    if (x > 0)
                    {
                        nodoRevisado.izquierda = nodosArreglo[x - 1, y];
                    }
                    if (x < ancho - 1)
                    {
                        nodoRevisado.derecha = nodosArreglo[x + 1, y];
                    }
                }
            }
        }

        public NodoMapa ObtenerNodo(Rectangle nodoJugador)  // Metodo que recorre la lista de nodos y verifica si el rectangulo del jugador y estela esta en contacto con algun nodo 
        {
            foreach (var nodo in nodosMapa)
            {
                if (nodo.RectanguloMapa.IntersectsWith(nodoJugador))
                {
                    return nodo;
                }
            }
            return null;
        }
    }
}
