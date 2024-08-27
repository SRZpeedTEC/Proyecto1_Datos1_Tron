using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public abstract class Poder
    {
        public Brush ColorItem { get; set; }

        public Image SpritePoder { get; set; }
        public Rectangle RectanguloPoder { get; set; }

        private const int TamanoPoder = 20;


        public Poder(Brush colorItem)
        {
            this.ColorItem = colorItem;
            this.RectanguloPoder = new Rectangle(0, 0, TamanoPoder, TamanoPoder);
        }

        public void ColocarPoderMapa(NodoMapa NodoSeleccionado)
        {
            this.RectanguloPoder = new Rectangle(NodoSeleccionado.RectanguloMapa.X, NodoSeleccionado.RectanguloMapa.Y, TamanoPoder, TamanoPoder);
        }
        public abstract Poder ClonarPoder();


        public void DibujarPoder(Graphics g)
        {
            g.DrawImage(SpritePoder, RectanguloPoder);
        }

        public virtual void EfectoPoder(Jugador jugador)
        {
            Console.WriteLine("Efecto Poder");
        }

    }
}
