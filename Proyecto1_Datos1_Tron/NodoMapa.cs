using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class NodoMapa
    {
        public NodoMapa arriba { get; set; }
        public NodoMapa abajo { get; set; }
        public NodoMapa izquierda { get; set; }
        public NodoMapa derecha { get; set; }
        public Rectangle RectanguloMapa { get; set; }
        public bool ocupado { get; set; }
        public bool ocupadoItem { get; set; }
        public bool ocupadoPoder { get; set; }
        public Item item { get; set; }
        public Poder poder { get; set; }

        public NodoMapa(Rectangle rectanguloMapa)
        {
            this.RectanguloMapa = rectanguloMapa;
            this.ocupado = false;
            this.ocupadoItem = false;
            this.ocupadoPoder = false;
            this.item = null;
            this.poder = null;

        }
    }
}
