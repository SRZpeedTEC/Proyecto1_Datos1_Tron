using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave; 

namespace Proyecto1_Datos1_Tron
{
    public class Bot : Jugador
    {
        private Random random; // Para generar decisiones aleatorias        
        public bool aplicandoItem = false; // Para evitar que se aplique un objeto más de una vez
        public bool aplicandoPoder = false; // Para evitar que se aplique un poder más de una vez
        public Timer DireccionTimer;

        public Bot(Mapa mapaJuego, int posicionInicialX, int posicionInicialY, string DireccionActual, string DireccionProhibida, Brush colorEstela)
            : base(mapaJuego, posicionInicialX, posicionInicialY, DireccionActual, DireccionProhibida, colorEstela, Keys.None, Keys.None, Keys.None, Keys.None, Keys.None, Keys.None)
        {
            random = new Random();
            DireccionTimer = new Timer();
            DireccionTimer.Interval = 1000;
            DireccionTimer.Tick += new EventHandler(DireccionTimer_Tick);
            DireccionTimer.Start();

        }


        public override void FuncionesPorTick()
        {
            Mover();
            GastoCombustible();
        }

        public void DireccionTimer_Tick(object sender, EventArgs e)
        {
            moverAleatoriamente();
        }

        public override void Mover()
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
                        if (!EvaluarRutasEscape())
                        {
                            form.colisionesPendientes.Add(new Tuple<Jugador, Jugador>(this, otroJugador));
                            return; // Salir sin hacer más movimientos
                        }

                    }
                }

                if (nodoDestino != null && nodoDestino.ocupadoItem == true)
                {
                    // Recoger item y aplicarlo
                    Item itemRecogido = nodoDestino.item;
                    itemRecogido.EfectoItem(this);
                    nodoDestino.ocupadoItem = false;
                    nodoDestino.item = null;
                    form.itemsLista.Remove(itemRecogido);

                    if (!aplicandoItem)
                    {
                        aplicandoItem = true;
                        Task.Delay(1000).ContinueWith(t =>
                        {                           
                            aplicandoItem = false;
                        });
                    }
                }
                else if (nodoDestino != null && nodoDestino.ocupadoPoder == true)
                {
                    // Recoger poder y aplicarlo
                    Poder poderRecogido = nodoDestino.poder;
                    poderRecogido.EfectoPoder(this);
                    nodoDestino.ocupadoPoder = false;
                    nodoDestino.poder = null;
                    form.poderesLista.Remove(poderRecogido);

                    if (!aplicandoPoder)
                    {
                        aplicandoPoder = true;
                        Task.Delay(1000).ContinueWith(t =>
                        {
                            aplicandoPoder = false;
                        });
                    }
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
                    if (!EvaluarRutasEscape())
                    {
                        Console.WriteLine("Colision");
                        vivo = false;
                        DestruccionMoto();
                    }

                }

            }
        }
        public bool EvaluarRutasEscape()
        {
            // Array de posibles direcciones
            string[] direcciones = { "Arriba", "Abajo", "Izquierda", "Derecha" };
            // Lista para almacenar direcciones válidas
            List<string> direccionesValidas = new List<string>();

            foreach (var direccion in direcciones)
            {
                if (direccion != DireccionProhibida || direccion != DireccionActual)
                {
                    Rectangle nuevaPosicion = Estela.ObtenerPrimero();

                    // Ajustar la nueva posición según la dirección
                    switch (direccion)
                    {
                        case "Arriba":
                            nuevaPosicion = new Rectangle(nuevaPosicion.X, nuevaPosicion.Y - TamañoCuadrado, TamañoCuadrado, TamañoCuadrado);
                            break;
                        case "Abajo":
                            nuevaPosicion = new Rectangle(nuevaPosicion.X, nuevaPosicion.Y + TamañoCuadrado, TamañoCuadrado, TamañoCuadrado);
                            break;
                        case "Izquierda":
                            nuevaPosicion = new Rectangle(nuevaPosicion.X - TamañoCuadrado, nuevaPosicion.Y, TamañoCuadrado, TamañoCuadrado);
                            break;
                        case "Derecha":
                            nuevaPosicion = new Rectangle(nuevaPosicion.X + TamañoCuadrado, nuevaPosicion.Y, TamañoCuadrado, TamañoCuadrado);
                            break;
                    }

                    NodoMapa nodo = mapaJuego.ObtenerNodo(nuevaPosicion);
                    if (nodo != null && !nodo.ocupado)
                    {
                        direccionesValidas.Add(direccion);
                    }
                }
            }

            if (direccionesValidas.Count > 0)
            {
                // Cambiar a una de las direcciones válidas aleatoriamente
                DireccionActual = direccionesValidas[random.Next(direccionesValidas.Count)];
                DireccionProhibida = DireccionActual == "Arriba" ? "Abajo" :
                                     DireccionActual == "Abajo" ? "Arriba" :
                                     DireccionActual == "Izquierda" ? "Derecha" : "Izquierda";
                return true; // Cambio de dirección exitoso
            }

            return false; // No hay direcciones disponibles
        }

        public void moverAleatoriamente()
        {
            // Array de posibles direcciones
            string[] Direcciones = { "Arriba", "Abajo", "Izquierda", "Derecha" };
            List<string> DireccionesLista = new List<string>();
            foreach (var Direccion in Direcciones)

                if (Direccion != DireccionProhibida || Direccion != DireccionActual)
                {
                    DireccionesLista.Add(Direccion);
                }
            // Lista para almacenar direcciones válidas          

            DireccionActual = Direcciones[random.Next(DireccionesLista.Count)];
            DireccionProhibida = DireccionActual == "Arriba" ? "Abajo" :
                                 DireccionActual == "Abajo" ? "Arriba" :
                                 DireccionActual == "Izquierda" ? "Derecha" : "Izquierda";

        }

    }
}
