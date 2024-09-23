using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1_Datos1_Tron
{
    public class Bomba : Item
    {        
        public Bomba(Brush colorItem) : base(colorItem)

        {
            this.Sprite = Image.FromFile(@"Resources\bomba.png");         
            this.ColorItem = colorItem;
          
        }

        public override void EfectoItem(Jugador jugador)
        {

            NodoMapa CentroExplosion = jugador.mapaJuego.ObtenerNodo(jugador.Estela.ObtenerPrimero());
            Task.Run(async () =>
            {
                await Task.Delay(3000);
                Explosion(jugador, CentroExplosion);
            });
            
        }

        private void Explosion(Jugador jugador, NodoMapa CentroExplosion)
        {
            int rangoExplosion = 2;
            List<NodoMapa> NodosExplosion = new List<NodoMapa>();
            for (int x = -rangoExplosion; x <= rangoExplosion; x++)
            {
                for (int y = -rangoExplosion; y <= rangoExplosion; y++)
                {
                    NodoMapa areaExplosion = jugador.mapaJuego.ObtenerNodo(new Rectangle(
                        CentroExplosion.RectanguloMapa.X + x * jugador.mapaJuego.tamanoNodo,
                        CentroExplosion.RectanguloMapa.Y + y * jugador.mapaJuego.tamanoNodo,
                        jugador.mapaJuego.tamanoNodo, jugador.mapaJuego.tamanoNodo));

                    if (areaExplosion != null)
                    {
                        areaExplosion.ocupado = true;
                        NodosExplosion.Add(areaExplosion);
                    }
                }
            }

            FormGame form = (FormGame)Application.OpenForms["FormGame"];
            form.ManejarExplosion(NodosExplosion, Brushes.Gray);
        }

        public void DibujarBombaExlotando(Graphics g)
        {
            Console.WriteLine("PUMMM");
            g.DrawImage(Sprite, RectanguloItem);
            Task.Run(async () =>
            {
                await Task.Delay(3000);
                return;
            });          
        }

        public override Item Clonar()
        {
            return new Bomba(this.ColorItem);
        }
    }
}
