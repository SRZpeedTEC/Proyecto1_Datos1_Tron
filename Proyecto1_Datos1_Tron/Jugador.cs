using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Proyecto1_Datos1_Tron
{
    public class Jugador
    {
        public int Velocidad { get; set; }
        public int Combustible { get; set; }
        public LinkedList<Rectangle> Estela { get; set; }
        public Queue<Item> Items { get; set; }
        public Stack<Poder> Poderes { get; set; }
        public string DireccionActual { get; set; }
        public string DireccionProhibida { get; set; }
        public Image MotoSpriteUP { get; set; }
        public Image MotoSpriteDOWN { get; set; }
        public Image MotoSpriteLEFT { get; set; }
        public Image MotoSpriteRIGHT { get; set; }
        public Brush colorEstela { get; set; }

        public Keys UpKey { get; set; }
        public Keys DownKey { get; set; }
        public Keys LeftKey { get; set; }
        public Keys RightKey { get; set; }
        private Mapa mapa { get; set; }




        private const int TamañoCuadrado = 20; // Tamaño del cuadrado de colisión
        private const int TamañoEstela = 3; // Tamaño de la estela

        public Jugador(Mapa mapa, int posicionInicialX, int posicionInicialY, string DireccionActual, string DireccionProhibida, Brush colorEstela, Keys UpKey, Keys DownKey, Keys RightKey, Keys LeftKey)
        {
            Random rnd = new Random();
            Velocidad = rnd.Next(1, 11);
            Combustible = 100;

            Estela = new LinkedList<Rectangle>();
            Estela.AddFirst(new Rectangle(posicionInicialX, posicionInicialY, TamañoCuadrado, TamañoCuadrado));
            for(int x = 0; x < 3; x++)
            {
                Estela.AddLast(new Rectangle(posicionInicialX, posicionInicialY, TamañoEstela, TamañoEstela));    
            }


            Items = new Queue<Item>();
            Poderes = new Stack<Poder>();

            this.DireccionActual = DireccionActual;
            this.DireccionProhibida = DireccionProhibida;
            this.colorEstela = colorEstela;
            this.mapa = mapa;

            MotoSpriteUP = Image.FromFile(@"Resources\motoSprite.png");
            MotoSpriteDOWN = Image.FromFile(@"Resources\motoSpriteDown.png");
            MotoSpriteRIGHT = Image.FromFile(@"Resources\motoSpriteR.png");
            MotoSpriteLEFT = Image.FromFile(@"Resources\motoSpriteL.png");
            this.UpKey = UpKey;
            this.DownKey = DownKey;
            this.RightKey = RightKey;
            this.LeftKey = LeftKey;
            
        }

        public void Mover()
        {
            Rectangle cabezaActual = Estela.First.Value;
            Rectangle nuevaPosicion = cabezaActual;

            switch (DireccionActual)
            {
                case "Arriba":
                    nuevaPosicion = new Rectangle(cabezaActual.X, cabezaActual.Y - TamañoCuadrado, TamañoCuadrado, TamañoCuadrado);
                    break;
                case "Abajo":
                    nuevaPosicion = new Rectangle(cabezaActual.X, cabezaActual.Y + TamañoCuadrado, TamañoCuadrado, TamañoCuadrado);
                    break;
                case "Izquierda":
                    nuevaPosicion = new Rectangle(cabezaActual.X - TamañoCuadrado, cabezaActual.Y, TamañoCuadrado, TamañoCuadrado);
                    break;
                case "Derecha":
                    nuevaPosicion = new Rectangle(cabezaActual.X + TamañoCuadrado, cabezaActual.Y, TamañoCuadrado, TamañoCuadrado);
                    break;
            }

            NodoMapa nodoDestino = mapa.ObtenerNodo(nuevaPosicion);

            if (nodoDestino != null && nodoDestino.ocupado != true)
            {
                // Dejar la estela en la posición actual
                nodoDestino.ocupado = true;

                // Agregar nueva posición a la estela
                Estela.AddFirst(nuevaPosicion);

                Rectangle ultimoSegmento = Estela.Last.Value;
                NodoMapa nodoUltimoSegmento = mapa.ObtenerNodo(ultimoSegmento);
                if (nodoUltimoSegmento != null)
                {
                    nodoUltimoSegmento.ocupado = false; // Liberar el nodo
                }

                // Remover el último segmento de la estela
                Estela.RemoveLast();
            }
        }

        public bool Colision()
        {
            // Comprobar si la cabeza de la estela colisiona con alguna otra posición de la estela
            Rectangle cabeza = Estela.First.Value;
            foreach (var segment in Estela)
            {
                if (cabeza.X == segment.X && cabeza.Y == segment.Y)
                { Console.WriteLine("Colision"); return true;}    
            }

            return false;
        }

        public void Dibujar(Graphics g)
        {
            foreach (var segmento in Estela.Skip(1))
            {
                g.FillRectangle(colorEstela, segmento);
            }

                  

            // Dibujar la cabeza de la moto
            Rectangle cabeza = Estela.First.Value;
            // Escalar la cabeza de la moto para que sea más grande visualmente
            int cabezaWidth = (int)(cabeza.Width * 1.75);
            int cabezaHeight = (int)(cabeza.Height * 1.75);
            cabeza = new Rectangle(
                cabeza.X + (cabeza.Width - cabezaWidth) / 2,
                cabeza.Y + (cabeza.Height - cabezaHeight) / 2,
                cabezaWidth,
                cabezaHeight

            );

            // Dibujar la cabeza de la moto
            switch (DireccionActual)
            {
                case "Derecha":
                    g.DrawImage(MotoSpriteRIGHT, cabeza);
                    break;
                case "Izquierda":
                    g.DrawImage(MotoSpriteLEFT, cabeza);
                    break;
                case "Abajo":
                    g.DrawImage(MotoSpriteDOWN, cabeza);
                    break;
                case "Arriba":
                    g.DrawImage(MotoSpriteUP, cabeza);
                    break;
            }
        }
        public virtual void CambiarDireccion(Keys key)
        {
            if (key == UpKey && DireccionProhibida != "Arriba")
            {
                DireccionActual = "Arriba";
                DireccionProhibida = "Abajo";
            }
            else if (key == DownKey && DireccionProhibida != "Abajo")
            {
                DireccionActual = "Abajo";
                DireccionProhibida = "Arriba";
            }
            else if (key == RightKey && DireccionProhibida != "Derecha")
            {
                DireccionActual = "Derecha";
                DireccionProhibida = "Izquierda";
            }
            else if (key == LeftKey && DireccionProhibida != "Izquierda")
            {
                DireccionActual = "Izquierda";
                DireccionProhibida = "Derecha";
            }

        }

    }
}



