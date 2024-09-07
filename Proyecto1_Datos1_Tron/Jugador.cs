using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Media;
using System.Collections.Generic;

using FormTimer = System.Windows.Forms.Timer;
using ThreadTimer = System.Threading.Timer;

namespace Proyecto1_Datos1_Tron
{
    public class Jugador
    {
        // ATRIBUTOS JUGADOR
        public int Velocidad { get; set; }
        public int CombustibleTanque { get; set; }
        public int Combustible { get; set; }
        public int Kilometraje = 0;
        public ListaEnlazada<Rectangle> Estela { get; set; }
        public int TamanoEstela = 3; // Tamaño de la estela
        public ThreadTimer movimientoTimer;

        // AUXILIARES
        public Cola<Item> Items { get; set; }
        public Pila<Poder> Poderes { get; set; }
        public string DireccionActual { get; set; }
        public string DireccionProhibida { get; set; }
        public Rectangle nuevaPosicion { get; set; }
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

        public Label lblinfoCombustible { get; set; }
        public ProgressBar progressBarinfoCombustible { get; set; }


        public bool vivo { get; set; }
        public Mapa mapaJuego { get; set; }

        public SoundPlayer sonidoCambioDireccion = new SoundPlayer(@"Resources\AudioMotos.wav");       
        public AdministradorSonido SonidosJuego = new AdministradorSonido();

        public const int TamañoCuadrado = 20; // Tamaño del cuadrado de colisión

        // PODERES

        public bool escudoActivo = false;
        public bool hiperVelocidadActiva = false;   
        public bool poderAplicado = false;

        public Jugador(Mapa mapaJuego, int posicionInicialX, int posicionInicialY, string DireccionActual, string DireccionProhibida, Brush colorEstela, Keys UpKey, Keys DownKey, Keys RightKey, Keys LeftKey, Keys PowerKey, Keys ChangeKey)
        {
            Random rnd = new Random();
            Velocidad = 55;
            CombustibleTanque = 100;
            Combustible = CombustibleTanque;
            vivo = true;
            movimientoTimer = new ThreadTimer(TickHandler, null, 100, Velocidad);

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
       
        public void TickHandler(object sender)      // Funcion que se ejecuta cada vez que el timer del jugador hace tick, maneja logica primaria
        {
            FuncionesPorTick();
        }
        public virtual void  Mover()
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
                FormGame form = (FormGame)Application.OpenForms["FormGame"];
                List<Jugador> otrosJugadores = form.jugadores;

                foreach (var otroJugador in otrosJugadores)
                {
                    if (otroJugador != this && otroJugador.Estela.ObtenerPrimero().IntersectsWith(nuevaPosicion) && this.vivo && otroJugador.vivo)
                    {
                        // Añadir a la lista de colisiones pendientes                      
                        form.colisionesPendientes.Add(new Tuple<Jugador, Jugador>(this, otroJugador));
                        return; // Salir sin hacer más movimientos
                    }
                }


                if (nodoDestino != null && nodoDestino.ocupadoItem == true)
                {
                    Console.WriteLine("Item Recogido");
                    SonidosJuego.ReproducirSonido(@"Resources\RecoleccionObjeto.wav");
                    Item itemRecogido = nodoDestino.item;
                    itemRecogido.EfectoItem(this);
                    nodoDestino.ocupadoItem = false;
                    nodoDestino.item = null;
                    Items.Enqueue(itemRecogido);
                    if (itemRecogido is Bomba)
                    {
                        form.bombasActivadas.Add((Bomba)itemRecogido);
                        Console.WriteLine("Bomba Recogida");
                    }
                                      
                    form.itemsLista.Remove(itemRecogido);
                }
                else if (nodoDestino != null && nodoDestino.ocupadoPoder == true)
                {
                    Console.WriteLine("Poder Recogido");
                    SonidosJuego.ReproducirSonido(@"Resources\RecoleccionObjeto.wav");
                    Poder poderRecogido = nodoDestino.poder;                   
                    nodoDestino.ocupadoPoder = false;
                    nodoDestino.poder = null;
                    Poderes.Push(poderRecogido);
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
                else if ((nodoDestino == null || nodoDestino.ocupado == true) && !escudoActivo)
                {
                    Console.WriteLine("Colision");
                    vivo = false;
                    DestruccionMoto();
                }
              
            }
        }


        public void AlternarPoder()
        {
            if (!Poderes.VacioPila() && Poderes.elementos.Contador > 1)
            {
                // Sacar el poder de la cima de la pila
                Poder poderActual = Poderes.PopPeek();

                // Crear una pila temporal para almacenar los otros poderes
                Stack<Poder> pilaTemporal = new Stack<Poder>();

                // Mover los otros poderes a la pila temporal
                while (!Poderes.VacioPila())
                {
                    pilaTemporal.Push(Poderes.PopPeek());
                }

                // Poner el poder que estaba en la cima al fondo de la pila
                Poderes.Push(poderActual);

                // Regresar los otros poderes a la pila original
                while (pilaTemporal.Count > 0)
                {
                    Poderes.Push(pilaTemporal.Pop());
                }
            }
        }

            public void ActivarEscudo()
        {   
            
            escudoActivo = true;
            Task.Delay(5000).ContinueWith(t =>
            {
                escudoActivo = false;
                poderAplicado = false;
            });
            
        }

        public void ActivarHiperVelocidad()
        {
            if (!hiperVelocidadActiva)
            {
                hiperVelocidadActiva = true;
                Velocidad = 25;
                ReiniciarTimer();

                Task.Delay(5000).ContinueWith(t =>
                {
                    hiperVelocidadActiva = false;
                    poderAplicado = false;
                    Velocidad = 55;
                    ReiniciarTimer();

                });
            }
        }

        private void ReiniciarTimer()
        {
            movimientoTimer.Change(0, Velocidad);
        }

        public virtual void AplicarPoder()
        {
            if (!Poderes.VacioPila())
            {
                poderAplicado = true;
                Poder poderActual = Poderes.PopPeek();
                poderActual.EfectoPoder(this);
            }
        }

        public virtual void TeclasPoderes(Keys key)
        {
            if (key == PowerKey)
            {
                if (!poderAplicado)
                {
                    AplicarPoder();
                }
                else if (poderAplicado)
                {

                    if (escudoActivo && Poderes.Peek() is Escudo)
                    {
                        return;
                    }

                    else if (hiperVelocidadActiva && Poderes.Peek() is HiperVelocidad)
                    {
                        return;
                    }
                    else if (hiperVelocidadActiva && Poderes.Peek() is Escudo)
                    {
                        AplicarPoder();
                    }
                    else if (escudoActivo && Poderes.Peek() is HiperVelocidad)
                    {
                        AplicarPoder();
                    }
                }
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
            if (vivo != false)
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
        }
        
        public void ActualizarCombustible(Label lblCombustible, ProgressBar progressBarCombustible)
        {
            if (lblCombustible.InvokeRequired)
            {
                lblCombustible.Invoke(new Action(() =>
                {
                    lblCombustible.Text = $"Combustible: {Combustible} / {CombustibleTanque}";
                    progressBarCombustible.Value = Combustible;
                }));
            }
            else
            {
                lblCombustible.Text = $"Combustible: {Combustible} / {CombustibleTanque}";
                progressBarCombustible.Value = Combustible;
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

                if (escudoActivo)
                {
                    g.FillRectangle(Brushes.Gray, cabeza);
                }


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

        public void RecibirInformacionForm(Label lblCombustible, ProgressBar progressBarCombustible)
        {
            this.lblinfoCombustible = lblCombustible;
            this.progressBarinfoCombustible = progressBarCombustible;
        }

        public virtual void FuncionesPorTick()
        {

           
            Mover();
            GastoCombustible();
            ActualizarCombustible(lblinfoCombustible, progressBarinfoCombustible);
            
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
                    movimientoTimer.Change(Timeout.Infinite, Timeout.Infinite);                  
                }

                SonidosJuego.ReproducirSonido(@"Resources\SonidoColision.wav");
            }
        }
      
    }
}



