using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using NAudio.Wave;

namespace Proyecto1_Datos1_Tron
{
    public partial class FormGame : Form
    {
        //MAPA Y JUGADORES

        public Mapa mapa;
        public List<Jugador> jugadores;
        public List<Tuple<Jugador, Jugador>> colisionesPendientes = new List<Tuple<Jugador, Jugador>>();  
        public bool DosJugadores = false;


        // TIMERS

        private Timer actualizacionTimer;
        private Timer itemSpawnTimer;
        
        // SONIDOS

        public SoundPlayer sonidoCambioDireccion = new SoundPlayer(@"Resources\AudioMotos.wav");

        // ITEMS Y PODERES

        public List<Item> PosiblesItems = new List<Item>();
        public List<Item> itemsLista = new List<Item>();

        public List<Poder> PosiblesPoderes = new List<Poder>();
        public List<Poder> poderesLista = new List<Poder>();

        // BOMBAS

        public List<List<NodoMapa>> nodosExplosion = new List<List<NodoMapa>>(); // Nodos afectados por la explosión
        public List<Bomba> bombasActivadas = new List<Bomba>(); // Bombas activadas en el mapa
        private Brush colorExplosion;
        private Random randomGenerator;

        // AUDIO

        public AdministradorSonido Musica = new AdministradorSonido();


        public FormGame(bool modoVersus = false)
        {
            InitializeComponent();


            // MAPA Y JUGADOR

            
            mapa = new Mapa(58, 34, 20); // Tamaño del mapa con nodos de 20x20 píxeles
            jugadores = new List<Jugador>();
            randomGenerator = new Random();
            

            if (modoVersus)
            {
                // Configurar el modo versus: solo dos jugadores humanos
                DosJugadores = true;
                Jugador jugador1 = new Jugador(mapa, 10 * 20, 10 * 20, "Derecha", "Izquierda", Brushes.Blue, Keys.W, Keys.S, Keys.D, Keys.A, Keys.R, Keys.Q);
                Jugador jugador2 = new Jugador(mapa, 40 * 20, 10 * 20, "Izquierda", "Derecha", Brushes.Red, Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.P, Keys.O);

                jugadores.Add(jugador1);
                jugadores.Add(jugador2);
               
            }
            else
            {

                // Crear jugadores en el mapa
                Jugador jugador1 = new Jugador(mapa, 10 * 20, 10 * 20, "Derecha", "Izquierda", Brushes.Blue, Keys.W, Keys.S, Keys.D, Keys.A, Keys.R, Keys.Q);
                // Jugador jugador2 = new Jugador(mapa, 20 * 20, 20 * 20, "Izquierda", "Derecha", Brushes.Blue, Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.P, Keys.O);
                Bot bot1 = new Bot(mapa, 20 * 20, 20 * 20, "Izquierda", "Derecha", Brushes.Red);    // Posición personalizada
                Bot bot2 = new Bot(mapa, 57 * 20, 33 * 20, "Izquierda", "Derecha", Brushes.Orange); // Posición personalizada
                Bot bot3 = new Bot(mapa, 40 * 20, 20 * 20, "Izquierda", "Derecha", Brushes.Yellow); // Posición personalizada
                Bot bot4 = new Bot(mapa, 0 * 20, 0 * 20, "Derecha", "Izquierda", Brushes.Green);    // Esquina superior izquierda         
               


                jugadores.Add(jugador1);
                // jugadores.Add(jugador2);
                jugadores.Add(bot1);
                jugadores.Add(bot2);
                jugadores.Add(bot3);
                jugadores.Add(bot4);
            }

            this.KeyDown += new KeyEventHandler(OnKeyDown);
            pictureBox1.Paint += new PaintEventHandler(PictureBox1_Paint);


            // ITEMS
            RecargaCombustible recargaCombustible = new RecargaCombustible(Brushes.Green);
            RecargaCombustible RecargaCombustible2 = new RecargaCombustible(Brushes.Green);
            RecargaCombustible RecargaCombustible3 = new RecargaCombustible(Brushes.Green);
            CrecimientoEstela  crecimientoEstela = new CrecimientoEstela(Brushes.Blue);
            Bomba bomba = new Bomba(Brushes.Red);
            PosiblesItems.Add(recargaCombustible);
            PosiblesItems.Add(crecimientoEstela);
            PosiblesItems.Add(bomba);
            PosiblesItems.Add(RecargaCombustible2);
            PosiblesItems.Add(RecargaCombustible3);

            // PODERES
            Escudo escudo = new Escudo(Brushes.Yellow);
            HiperVelocidad hiperVelocidad = new HiperVelocidad(Brushes.Purple);
            PosiblesPoderes.Add(escudo);
            PosiblesPoderes.Add(hiperVelocidad);


            // TIMER ITEMS Y PODERES

            itemSpawnTimer = new Timer();
            itemSpawnTimer.Interval = 2000; // 5000 milisegundos = 5 segundos
            itemSpawnTimer.Tick += new EventHandler(OnItemSpawnTick);
            itemSpawnTimer.Start();


            // TIMER JUEGO

            actualizacionTimer = new Timer();
            actualizacionTimer.Interval = 50; // Ajusta este valor según la velocidad deseada
            actualizacionTimer.Tick += new EventHandler(OnTimerTick);
            sonidoCambioDireccion.Play();
            actualizacionTimer.Start();
            MandarIndicadores();

        }

        public void OnTimerTick(object sender, EventArgs e) // Método que se ejecuta cada vez que el timer llega a su intervalo
        {
            
            VerificarColisionesPendientes();
            MostrarPoderes();
            BotarObjetos();
            ActualizarListaJugadores();
            confirmarVictoria();



            pictureBox1.Invalidate();
        }

        public void OnKeyDown(object sender, KeyEventArgs e) // Método que se ejecuta cada vez que se presiona una tecla
        {
            foreach (var jugador in jugadores)
            {
                jugador.CambiarDireccion(e.KeyCode);
                jugador.TeclasPoderes(e.KeyCode);
            }
        }
      

        private void FormGame_Load(object sender, EventArgs e)
        {

        }
        public void PictureBox1_Paint(object sender, PaintEventArgs e) // Método que se ejecuta cada vez que se redibuja el PictureBox
        {
            foreach (var nodo in mapa.nodosMapa)
            {
                e.Graphics.DrawRectangle(Pens.DarkSlateBlue, nodo.RectanguloMapa);

            }
            
            foreach (var jugador in jugadores)
            {
                jugador.Dibujar(e.Graphics);

            }

            var itemsListaCopy = new List<Item>(itemsLista);
            foreach (var item in itemsListaCopy)
            {
                if (item != null)
                {
                    item.DibujarItem(e.Graphics);
                }               
            }

            var poderesListaCopy = new List<Poder>(poderesLista);
            foreach (var poder in poderesListaCopy)
            {
                if (poder != null)
                {
                    poder.DibujarPoder(e.Graphics);
                }
            }

            foreach (var bomba in bombasActivadas)
            {
                bomba.Sprite = Image.FromFile(@"Resources\bombaExplotando.png");
                bomba.DibujarBombaExlotando(e.Graphics);
                Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    bombasActivadas.Remove(bomba);
                });
            }

            if (nodosExplosion != null && nodosExplosion.Count > 0)
            {
                for (int i = 0; i < nodosExplosion.Count; i++)
                {
                    for (int j = 0; j < nodosExplosion[i].Count; j++)
                    {
                        e.Graphics.FillRectangle(colorExplosion, nodosExplosion[i][j].RectanguloMapa);
                    }
                }
            }
            
        }

        public void MandarIndicadores()
        {
            
            jugadores[0].RecibirInformacionForm((Label)Controls["lblCombustible"], (ProgressBar)Controls["progressBarCombustible"]);

            // Actualizar indicadores para el jugador 2 si existe
            if (DosJugadores)
            {
                jugadores[1].RecibirInformacionForm((Label)Controls["lblCombustible2"], (ProgressBar)Controls["progressBarCombustible2"]);
            }
        }
     

        public void VerificarColisionesPendientes()
        {

            lock (colisionesPendientes)
            { 

                var colisionesPendientesCopia = new List<Tuple<Jugador, Jugador>>(colisionesPendientes);

                foreach (var colision in colisionesPendientesCopia)
                {
                    var jugador1 = colision.Item1;
                    var jugador2 = colision.Item2;

                    if (jugador1.escudoActivo)
                    {
                        jugador2.vivo = false;
                        jugador2.DestruccionMoto();
                        Console.WriteLine("ColisionDada");
                    }
                    else if (jugador2.escudoActivo)
                    {
                        jugador1.vivo = false;
                        jugador1.DestruccionMoto();
                        Console.WriteLine("ColisionDada");
                    }
                    else
                    {
                        jugador1.vivo = false;
                        jugador2.vivo = false;
                        jugador1.DestruccionMoto();
                        jugador2.DestruccionMoto();
                        Console.WriteLine("ColisionDada");
                    }

                    colisionesPendientes.Remove(colision);

                }               
            }
        }

        public void ActualizarListaJugadores()
        {
            jugadores = jugadores.Where(j => j.vivo).ToList();
        }



        public void ManejarExplosion(List<NodoMapa> nodosAfectados, Brush colorExplosion) // Método que se encarga de manejar la explosión de la bomba
        {
            Task.Run(async () =>
            {
                nodosExplosion.Add(nodosAfectados);
                this.colorExplosion = colorExplosion;

                Invoke(new Action(() =>
                {
                    foreach (var nodo in nodosAfectados)
                    {
                        nodo.ocupado = true;
                        
                    }
                }));

                await Task.Delay(3000);

                Invoke(new Action(() =>
                {
                    foreach (var nodo in nodosAfectados)
                    {
                        nodo.ocupado = false;
                        
                    }
                    nodosExplosion.Remove(nodosAfectados);
                    pictureBox1.Invalidate();
                }));
            });
        }

        private void OnItemSpawnTick(object sender, EventArgs e) // Método que se ejecuta cada vez que el timer de spawn de items llega a su intervalo
        {

            

            Item MoldeItem = PosiblesItems[randomGenerator.Next(PosiblesItems.Count)];
            Item nuevoItem = MoldeItem.Clonar();

            Poder MoldePoder = PosiblesPoderes[randomGenerator.Next(PosiblesPoderes.Count)];
            Poder nuevoPoder = MoldePoder.ClonarPoder();

            // Obtener un nodo disponible aleatorio
            NodoMapa nodoDisponibleItem = mapa.ObtenerNodoDisponible();
            if (nodoDisponibleItem != null)
            {
                // Colocar el item en el nodo seleccionado
                nuevoItem.ColocarItemMapa(nodoDisponibleItem);

                // Marcar el nodo como ocupado
                nodoDisponibleItem.ocupadoItem = true;
                nodoDisponibleItem.item = nuevoItem;

                // Dibujar el item en el mapa
                itemsLista.Add(nuevoItem);

            }
            NodoMapa nodoDisponiblePoder = mapa.ObtenerNodoDisponible();
            if (nodoDisponiblePoder != null)
            {
                // Colocar el item en el nodo seleccionado
                nuevoPoder.ColocarPoderMapa(nodoDisponiblePoder);

                // Marcar el nodo como ocupado
                nodoDisponiblePoder.ocupadoPoder = true;
                nodoDisponiblePoder.poder = nuevoPoder;

                // Dibujar el item en el mapa
                poderesLista.Add(nuevoPoder);

            }                         
        }
        public void MostrarPoderes()
        {
            PictureBox[] SpritePoderes = { PoderPrimero, PoderSegundo, PoderTercero };

            foreach (var pictureBox in SpritePoderes)
            {
                pictureBox.Image = null;
            }

            int i = 0;
            foreach (var poder in jugadores[0].Poderes)
            {
                if (poder != null)
                {
                    if (i >= SpritePoderes.Length) break;
                    SpritePoderes[i].Image = poder.SpritePoder;
                    i++;
                }              

            }
            
            if(DosJugadores && jugadores.Count > 1)
            {
                PictureBox[] SpritePoderes2 = { PoderPrimero2, PoderSegundo2, PoderTercero2 };

                foreach (var pictureBox in SpritePoderes2)
                {
                    pictureBox.Image = null;
                }

                int j = 0;
                foreach (var poder in jugadores[1].Poderes)
                {
                    if (poder != null)
                    {
                        if (j >= SpritePoderes2.Length) break;
                        SpritePoderes2[j].Image = poder.SpritePoder;
                        j++;
                    }

                }              
            }
                
        }

        public void confirmarVictoria()
        {
            if (jugadores.Count <= 1)
            {
                foreach (var jugadores in jugadores)
                {
                    jugadores.DestruccionFinal();
                }

                actualizacionTimer.Stop();
                Victoria pantallaVictoria = new Victoria();
                Console.WriteLine("Modo Versus Activado");
                pantallaVictoria.Show();
                this.Hide();
            }
        }

        public void BotarObjetos()
        {
            foreach (var jugador in jugadores)
            {
                if (!jugador.vivo)
                {
                    Pila<Poder> PoderesPostMorten = jugador.Poderes;
                    foreach (var Poder in PoderesPostMorten)
                    {
                        NodoMapa nodoDisponiblePoderPostMorten = mapa.ObtenerNodoDisponible();
                        Poder.ColocarPoderMapa(nodoDisponiblePoderPostMorten);

                        nodoDisponiblePoderPostMorten.ocupadoPoder = true;
                        nodoDisponiblePoderPostMorten.poder = Poder;

                        poderesLista.Add(Poder);
                    }
                    PoderesPostMorten.elementos.EliminarElementos();
                    jugador.Poderes.elementos.EliminarElementos();
                }
            }

        }

            private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Items_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
