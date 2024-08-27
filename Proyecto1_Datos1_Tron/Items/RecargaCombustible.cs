using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class RecargaCombustible : Item
    {
        public int CantidadCombustible { get; set; }
        public RecargaCombustible(Brush colorItem) : base(colorItem)
       
        {
            Random rnd = new Random();
            this.ColorItem = colorItem;
            this.Sprite = Image.FromFile(@"Resources\CombustibleJuego.png");
            this.CantidadCombustible = rnd.Next(1, 100);
        }

        public override void EfectoItem(Jugador jugador)
        {
            if (jugador.Combustible + CantidadCombustible > 100)
            {
                jugador.Combustible = 100;
            }
            else
            {
                jugador.Combustible += CantidadCombustible;
            }
        }
        public override Item Clonar()
        {
            return new RecargaCombustible(this.ColorItem);
        }
    }
}
