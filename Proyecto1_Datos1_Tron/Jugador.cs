using System;
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
        public int CombustibleTanque { get; set; }
        public int Combustible { get; set; }
        public int Kilometraje = 0;
        public ListaEnlazada<Rectangle> Estela { get; set; }
        public int TamanoEstela = 3; // Tamaño de la estela
        public Cola<Item> Items { get; set; }
        public Pila<Poder> Poderes { get; set; }
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
        public Keys PowerKey { get; set; }
        public Keys ChangeKey { get; set; }

        public bool vivo { get; set; }
        public Mapa mapaJuego { get; set; }

        public SoundPlayer sonidoCambioDireccion = new SoundPlayer(@"Resources\AudioMotos.wav");
        public SoundPlayer sonidoColision = new SoundPlayer(@"Resources\SonidoColision.wav");





        private const int TamañoCuadrado = 20; // Tamaño del cuadrado de colisión


        public Jugador(Mapa mapaJuego, int posicionInicialX, int posicionInicialY, string DireccionActual, string DireccionProhibida, Brush colorEstela, Keys UpKey, Keys DownKey, Keys RightKey, Keys LeftKey, Keys PowerKey, Keys ChangeKey)
        {
            Random rnd = new Random();
            Velocidad = rnd.Next(1, 11);
            CombustibleTanque = 100;
            Combustible = CombustibleTanque;
            vivo = true;

            Estela = new ListaEnlazada<Rectangle>();
            Estela.AgregarPrimero(new Rectangle(posicionInicialX, posicionInicialY, TamañoCuadrado, TamañoCuadrado));
            for (int x = 0; x < TamanoEstela; x++)
            {
                Estela.AgregarUltimo(new Rectangle(posicionInicialX, posicionInicialY, TamañoCuadrado, TamañoCuadrado));
            }


            Items = new Cola<Item>();
            Poderes = new Pila<Poder>();

            this.DireccionActual = DireccionActual;
            this.DireccionProhibida = DireccionProhibida;
            this.colorEstela = colorEstela;
            this.mapaJuego = mapaJuego;

            MotoSpriteUP = Image.FromFile(@"Resources\motoSprite.png");
            MotoSpriteDOWN = Image.FromFile(@"Resources\motoSpriteDown.png");
            MotoSpriteRIGHT = Image.FromFile(@"Resources\motoSpriteR.png");
            MotoSpriteLEFT = Image.FromFile(@"Resources\motoSpriteL.png");
            this.UpKey = UpKey;
            this.DownKey = DownKey;
            this.RightKey = RightKey;
            this.LeftKey = LeftKey;
            this.PowerKey = PowerKey;
            this.ChangeKey = ChangeKey;

        }

        public void Mover()
        {
            Rectangle cabezaActual = Estela.ObtenerPrimero();
            Rectangle nuevaPosicion = cabezaActual;
            if (vivo != false)
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

                NodoMapa nodoDestino = mapaJuego.ObtenerNodo(nuevaPosicion);

                if (nodoDestino != null && nodoDestino.ocupadoItem == true)
                {
                    Console.WriteLine("Item Recogido");
                    Item itemRecogido = nodoDestino.item;
                    itemRecogido.EfectoItem(this);
                    nodoDestino.ocupadoItem = false;
                    nodoDestino.item = null;
                    Items.AgregarCola(itemRecogido);
                    FormGame form = (FormGame)Application.OpenForms["FormGame"];
                    form.itemsLista.Remove(itemRecogido);
                }
                else if (nodoDestino != null && nodoDestino.ocupadoPoder == true)
                {
                    Console.WriteLine("Poder Recogido");
                    Poder poderRecogido = nodoDestino.poder;
                    // poderRecogido.EfectoPoder(this);
                    nodoDestino.ocupadoPoder = false;
                    nodoDestino.poder = null;
                    Poderes.MeterPila(poderRecogido);
                    FormGame form = (FormGame)Application.OpenForms["FormGame"];
                    form.poderesLista.Remove(poderRecogido);
                }

                else if (nodoDestino != null && nodoDestino.ocupado != true)
                {
                    // Dejar la estela en la posición actual
                    nodoDestino.ocupado = true;

                    // Agregar nueva posición a la estela
                    Estela.AgregarPrimero(nuevaPosicion);

                    Rectangle ultimoSegmento = Estela.ObtenerUltimo();
                    NodoMapa nodoUltimoSegmento = mapaJuego.ObtenerNodo(ultimoSegmento);
                    if (nodoUltimoSegmento != null)
                    {
                        nodoUltimoSegmento.ocupado = false; // Liberar el nodo
                    }

                    // Remover el último segmento de la estela
                    Estela.RemoverUltimo();
                    Kilometraje++;
                }
                else if (nodoDestino == null || nodoDestino.ocupado == true)
                {
                    Console.WriteLine("Colision");
                    vivo = false;
                    DestruccionMoto();
                }
              
            }
        }

        public void AlternarPoder()
        {
            if (!Poderes.VacioPila() && Poderes.ContadorPila() > 1)
            {
                Poder poderActual = Poderes.Eliminar();
                Poderes.MeterPilaFinal(poderActual);
            }
        }
        public void AplicarPoder()
        {
            if (!Poderes.VacioPila())
            {
                Poder poderActual = Poderes.Eliminar();
                poderActual.EfectoPoder(this);
            }
        } 

        public virtual void TeclasPoderes(Keys key)
        {
            if (key == PowerKey)
            {
                AplicarPoder();
            }
            else if (key == ChangeKey)
            {
                AlternarPoder();
            }
        }

        public void AumentarEstela(int Agregado)
        {
            int TamanoEstelaOriginal = TamanoEstela;
            TamanoEstela+= Agregado;
            
            for (int x = TamanoEstelaOriginal; x < TamanoEstela; x++)
            {
                Estela.AgregarUltimo(new Rectangle(Estela.ObtenerUltimo().X, Estela.ObtenerUltimo().Y, TamañoCuadrado, TamañoCuadrado));
            }
        }
        

        public void GastoCombustible()
        {
            if (Kilometraje == 5)
            {
                Combustible--;
                Kilometraje = 0;
            }
            else if (Combustible == 0)
            {
                vivo = false;
                DestruccionMoto();
            }
        }
        
        public void ActualizarCombustible(Label lblCombustible, ProgressBar progressBarCombustible)
        {
            lblCombustible.Text = $"Combustible: {Combustible} / {CombustibleTanque}";
            progressBarCombustible.Value = Combustible;
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
                    NodoMapa nodoSegmento = mapaJuego.ObtenerNodo(segmento);
                    nodoSegmento.ocupado = false;
                    Estela.RemoverPrimero();
                    sonidoCambioDireccion.Stop();
                    sonidoColision.Play();
                    
                }
            }
        }
    }
}



