using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace Proyecto1_Datos1_Tron
{
    public class Jugador
    {
        public int Velocidad { get; set; }
        public int Combustible { get; set; }
        public ListaEnlazada<Rectangle> Estela { get; set; }
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

        public bool vivo { get; set; }
        private Mapa mapa { get; set; }

        public SoundPlayer sonidoCambioDireccion = new SoundPlayer(@"Resources\AudioMotos.wav");
        public SoundPlayer sonidoColision = new SoundPlayer(@"Resources\SonidoColision.wav");





        private const int TamañoCuadrado = 20; // Tamaño del cuadrado de colisión
        private const int TamañoEstela = 3; // Tamaño de la estela

        public Jugador(Mapa mapa, int posicionInicialX, int posicionInicialY, string DireccionActual, string DireccionProhibida, Brush colorEstela, Keys UpKey, Keys DownKey, Keys RightKey, Keys LeftKey)
        {
            Random rnd = new Random();
            Velocidad = rnd.Next(1, 11);
            Combustible = 100;
            vivo = true;

            Estela = new ListaEnlazada<Rectangle>();
            Estela.AgregarPrimero(new Rectangle(posicionInicialX, posicionInicialY, TamañoCuadrado, TamañoCuadrado));
            for(int x = 0; x < 3; x++)
            {
                Estela.AgregarUltimo(new Rectangle(posicionInicialX, posicionInicialY, TamañoEstela, TamañoEstela));    
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
            Rectangle cabezaActual = Estela.ObtenerPrimero();
            Rectangle nuevaPosicion = cabezaActual;
            if(vivo != false)
            {
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
                    Estela.AgregarPrimero(nuevaPosicion);

                    Rectangle ultimoSegmento = Estela.ObtenerUltimo();
                    NodoMapa nodoUltimoSegmento = mapa.ObtenerNodo(ultimoSegmento);
                    if (nodoUltimoSegmento != null)
                    {
                        nodoUltimoSegmento.ocupado = false; // Liberar el nodo
                    }

                    // Remover el último segmento de la estela
                    Estela.RemoverUltimo();
                }
                else if (nodoDestino == null || nodoDestino.ocupado == true)
                {
                    Console.WriteLine("Colision");
                    vivo = false;
                    DestruccionMoto();
                }
            }
        }

        public void Dibujar(Graphics g)
        {
            if (vivo != false)
            {
                foreach (var segmento in Estela.Salto(0))
                {
                    g.FillRectangle(colorEstela, segmento);
                }



                // Dibujar la cabeza de la moto
                Rectangle cabeza = Estela.ObtenerPrimero();
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
        }
            
        public virtual void CambiarDireccion(Keys key)
        {
            if (vivo != false) {
                if (key == UpKey && DireccionProhibida != "Arriba")
                {
                    sonidoCambioDireccion.Play();
                    DireccionActual = "Arriba";
                    DireccionProhibida = "Abajo";
                }
                else if (key == DownKey && DireccionProhibida != "Abajo")
                {
                    sonidoCambioDireccion.Play();
                    DireccionActual = "Abajo";
                    DireccionProhibida = "Arriba";
                }
                else if (key == RightKey && DireccionProhibida != "Derecha")
                {
                    sonidoCambioDireccion.Play();
                    DireccionActual = "Derecha";
                    DireccionProhibida = "Izquierda";
                }
                else if (key == LeftKey && DireccionProhibida != "Izquierda")
                {
                    sonidoCambioDireccion.Play();
                    DireccionActual = "Izquierda";
                    DireccionProhibida = "Derecha";
                }
            }
            

        }

        public void DestruccionMoto()
        {
            if (vivo == false)
            {
                while (Estela.Contador > 0)
                {
                    Rectangle segmento = Estela.ObtenerPrimero();
                    NodoMapa nodoSegmento = mapa.ObtenerNodo(segmento);
                    nodoSegmento.ocupado = false;
                    Estela.RemoverPrimero();
                    sonidoCambioDireccion.Stop();
                    sonidoColision.Play();
                    
                }
            }
        }
    }
}



