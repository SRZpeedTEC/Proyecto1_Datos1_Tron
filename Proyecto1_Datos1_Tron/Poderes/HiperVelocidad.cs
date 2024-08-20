using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class HiperVelocidad : Poder
    {
        public Brush ColorItem { get; set; }

        public Image Sprite { get; set; }
        public Rectangle RectanguloPoder { get; set; }

        private const int TamanoPoder = 20;


        public HiperVelocidad(Brush colorItem) : base(colorItem)
        {
            this.ColorItem = colorItem;
            this.RectanguloPoder = new Rectangle(0, 0, TamanoPoder, TamanoPoder);
        }

        public override Poder ClonarPoder()
        {
            return new HiperVelocidad(this.ColorItem);
        }

        public override void EfectoPoder(Jugador jugador)
        {
            Console.WriteLine("Efecto Velocidad"); // jugador.ActivarEscudo();
        }
    }
}
