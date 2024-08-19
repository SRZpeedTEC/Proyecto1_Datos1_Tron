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
namespace Proyecto1_Datos1_Tron
{
    public partial class FormGame : Form
    {
        
        private Mapa mapa;
        private List<Jugador> jugadores;
        private Timer movimientoTimer;
        private Timer itemSpawnTimer;
        
        public SoundPlayer sonidoCambioDireccion = new SoundPlayer(@"Resources\AudioMotos.wav");
        public List<Item> PosiblesItems = new List<Item>();
        public List<Item> itemsLista = new List<Item>();
        public List<List<NodoMapa>> nodosExplosion = new List<List<NodoMapa>>(); // Nodos afectados por la explosión
        private Brush colorExplosion;


        public FormGame()
        {
            InitializeComponent();


            // MAPA Y JUGADOR


            mapa = new Mapa(58, 34, 20); // Tamaño del mapa con nodos de 20x20 píxeles
            jugadores = new List<Jugador>();

            // Crear jugadores en el mapa
            Jugador jugador1 = new Jugador(mapa, 10 * 20, 10 * 20, "Derecha", "Izquierda", Brushes.Red, Keys.W, Keys.S, Keys.D, Keys.A);
            // Jugador jugador2 = new Jugador(mapa, 20 * 20, 20 * 20, "Izquierda", "Derecha", Brushes.Blue, Keys.Up, Keys.Down, Keys.Right, Keys.Left);

            jugadores.Add(jugador1);
            // jugadores.Add(jugador2);

            this.KeyDown += new KeyEventHandler(OnKeyDown);
            pictureBox1.Paint += new PaintEventHandler(PictureBox1_Paint);


            // ITEMS
            RecargaCombustible recargaCombustible = new RecargaCombustible(Brushes.Green);
            CrecimientoEstela  crecimientoEstela = new CrecimientoEstela(Brushes.Blue);
            Bomba bomba = new Bomba(Brushes.Red);
            // PosiblesItems.Add(recargaCombustible);
            // PosiblesItems.Add(crecimientoEstela);   
            PosiblesItems.Add(bomba);
            

            // TIMER ITEMS Y PODERES

            itemSpawnTimer = new Timer();
            itemSpawnTimer.Interval = 5000; // 5000 milisegundos = 5 segundos
            itemSpawnTimer.Tick += new EventHandler(OnItemSpawnTick);
            itemSpawnTimer.Start();


            // TIMER JUEGO

            movimientoTimer = new Timer();
            movimientoTimer.Interval = 50; // Ajusta este valor según la velocidad deseada
            movimientoTimer.Tick += new EventHandler(OnTimerTick);
            sonidoCambioDireccion.Play();
            movimientoTimer.Start();

       
            


        }

        public void OnTimerTick(object sender, EventArgs e)
        {
            foreach (var jugador in jugadores)
            {
                jugador.Mover(); // El movimiento se maneja internamente
                jugador.GastoCombustible(); // El gasto de combustible se maneja internamente
                jugador.ActualizarCombustible(lblCombustible, progressBarCombustible); // Actualiza el combustible del jugador
            }

            pictureBox1.Invalidate();
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            jugadores[0].CambiarDireccion(e.KeyCode);
            // jugadores[1].CambiarDireccion(e.KeyCode);
    
        }
 
        private void FormGame_Load(object sender, EventArgs e)
        {

        }
        public void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var nodo in mapa.nodosMapa)
            {
                e.Graphics.DrawRectangle(Pens.DarkSlateBlue, nodo.RectanguloMapa);

            }
            
            foreach (var jugador in jugadores)
            {
                jugador.Dibujar(e.Graphics);

            }
            foreach (var item in itemsLista)
            {
                item.DibujarItem(e.Graphics);
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

        public void ManejarExplosion(List<NodoMapa> nodosAfectados, Brush colorExplosion)
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





        private void OnItemSpawnTick(object sender, EventArgs e)
        {

            Random randomItem = new Random();
            Item MoldeItem = PosiblesItems[randomItem.Next(PosiblesItems.Count)];
            Item nuevoItem = MoldeItem.Clonar();

            // Obtener un nodo disponible aleatorio
            NodoMapa nodoDisponible = mapa.ObtenerNodoDisponible();
            if (nodoDisponible != null)
            {
                // Colocar el item en el nodo seleccionado
                nuevoItem.ColocarItemMapa(nodoDisponible);

                // Marcar el nodo como ocupado
                nodoDisponible.ocupadoItem = true;
                nodoDisponible.item = nuevoItem;

                // Dibujar el item en el mapa
                itemsLista.Add(nuevoItem);

                pictureBox1.Invalidate();

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
    }
}
