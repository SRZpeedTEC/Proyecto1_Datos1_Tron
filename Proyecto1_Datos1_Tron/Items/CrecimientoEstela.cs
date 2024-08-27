using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class CrecimientoEstela : Item
    {
        public int CantidadEstela { get; set; }
        
        public CrecimientoEstela(Brush colorItem) : base(colorItem)

        {
            Random rnd = new Random();
            this.ColorItem = colorItem;
            this.Sprite = Image.FromFile(@"Resources\EstelaCrecimiento.png");
            this.CantidadEstela = rnd.Next(1, 2);
        }

        public override void EfectoItem(Jugador jugador)
        {
            jugador.AumentarEstela(CantidadEstela);
        }
        public override Item Clonar()
        {
            return new CrecimientoEstela(this.ColorItem);
        }
    }
}
