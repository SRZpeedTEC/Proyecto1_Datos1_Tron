using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class Poder
    {
        public Brush ColorItem { get; set; }

        public Image Sprite { get; set; }
        public Rectangle RectanguloPoder { get; set; }

        private const int TamanoPoder = 20;


        public Poder(Brush colorItem)
        {
            this.ColorItem = colorItem;
            this.RectanguloPoder = new Rectangle(0, 0, TamanoPoder, TamanoPoder);
        }

        public void ColocarAleatoriamente(NodoMapa NodoSeleccionado)
        {
            this.RectanguloPoder = new Rectangle(NodoSeleccionado.RectanguloMapa.X * TamanoPoder, NodoSeleccionado.RectanguloMapa.Y * TamanoPoder, TamanoPoder, TamanoPoder);
        }

        public void DibujarItem(Graphics g)
        {
            g.FillRectangle(ColorItem, RectanguloPoder);
        }
    }
}
