using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public abstract class Item
    {
        public Brush ColorItem { get; set; }
        
        public Image Sprite { get; set; }
        public Rectangle RectanguloItem { get; set; }

        public const int TamanoItem = 20;


        public Item(Brush colorItem)
        {
            this.ColorItem = colorItem;           
            this.RectanguloItem = new Rectangle(0, 0, TamanoItem, TamanoItem);
        }

        public void ColocarItemMapa(NodoMapa NodoSeleccionado)
        { 
            this.RectanguloItem = new Rectangle(NodoSeleccionado.RectanguloMapa.X, NodoSeleccionado.RectanguloMapa.Y, TamanoItem, TamanoItem);
        }
        public abstract Item Clonar();
       

        public void DibujarItem(Graphics g)
        {
            g.FillRectangle(ColorItem, RectanguloItem);
            
        }

        public virtual void EfectoItem(Jugador jugador)
        {
            Console.WriteLine("Efecto Item");
        }

    }
}
