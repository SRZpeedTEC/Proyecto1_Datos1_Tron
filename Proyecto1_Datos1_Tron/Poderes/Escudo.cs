using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class Escudo : Poder
    {
        public Escudo(Brush colorItem) : base(colorItem)
        {
            this.SpritePoder = Image.FromFile(@"Resources\EscudoMoto.png");
            this.ColorItem = colorItem;           
        }

        public override Poder ClonarPoder()
        {
            return new Escudo(this.ColorItem);
        }

        public override void EfectoPoder(Jugador jugador)
        {
            Console.WriteLine("Efecto Escudo"); 
            jugador.ActivarEscudo();
        }
    }
}

