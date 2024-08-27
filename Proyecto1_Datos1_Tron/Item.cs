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
        public Rectangle RectanguloItem = new Rectangle(0, 0, TamanoItem, TamanoItem);

        public const int TamanoItem = 20;


        public Item(Brush colorItem)
        {
            this.ColorItem = colorItem;                     
        }

        public void ColocarItemMapa(NodoMapa NodoSeleccionado)
        { 
            this.RectanguloItem = new Rectangle(NodoSeleccionado.RectanguloMapa.X, NodoSeleccionado.RectanguloMapa.Y, TamanoItem, TamanoItem);
        }

        public abstract Item Clonar();
       
        public void DibujarItem(Graphics g)
        {
            g.DrawImage(Sprite, RectanguloItem);
            
        }

        public virtual void EfectoItem(Jugador jugador)
        {
            Console.WriteLine("Efecto Item");
        }

    }
}
