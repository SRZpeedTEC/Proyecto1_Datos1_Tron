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

        public HiperVelocidad(Brush colorItem) : base(colorItem)
        {
            this.SpritePoder = Image.FromFile(@"Resources\VelocidadPoder.png");
            this.ColorItem = colorItem;
                   
        }

        public override Poder ClonarPoder()
        {
            return new HiperVelocidad(this.ColorItem);
        }

        public override void EfectoPoder(Jugador jugador)
        {
            Console.WriteLine("Efecto HiperVelocidad");
            jugador.ActivarHiperVelocidad();
        }
    }
}
