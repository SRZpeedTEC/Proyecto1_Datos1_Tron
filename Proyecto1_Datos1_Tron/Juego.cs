using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1_Datos1_Tron
{
    public partial class FormGame : Form
    {
        private Mapa mapa;
        private List<Jugador> jugadores;
        private Timer movimientoTimer;

        public FormGame()
        {
            InitializeComponent();
            mapa = new Mapa(50, 30, 20); // Tamaño del mapa con nodos de 20x20 píxeles
            jugadores = new List<Jugador>();

            // Crear jugadores en el mapa
            Jugador jugador1 = new Jugador(mapa, 10 * 20, 10 * 20, "Derecha", "Izquierda", Brushes.Red, Keys.W, Keys.S, Keys.D, Keys.A);
            Jugador jugador2 = new Jugador(mapa, 20 * 20, 20 * 20, "Izquierda", "Derecha", Brushes.Blue, Keys.Up, Keys.Down, Keys.Right, Keys.Left);

            jugadores.Add(jugador1);
            jugadores.Add(jugador2);

            this.KeyDown += new KeyEventHandler(OnKeyDown);
            pictureBox1.Paint += new PaintEventHandler(PictureBox1_Paint);

            movimientoTimer = new Timer();
            movimientoTimer.Interval = 100; // Ajusta este valor según la velocidad deseada
            movimientoTimer.Tick += new EventHandler(OnTimerTick);
            movimientoTimer.Start();
        }

        public void OnTimerTick(object sender, EventArgs e)
        {
            foreach (var jugador in jugadores)
            {
                jugador.Mover(); // El movimiento se maneja internamente
            }

            pictureBox1.Invalidate();
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            jugadores[0].CambiarDireccion(e.KeyCode);
            jugadores[1].CambiarDireccion(e.KeyCode);
    
        }


        
        private void FormGame_Load(object sender, EventArgs e)
        {

        }
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle cabezaRect = new Rectangle();
            // Dibujar la moto y la estela en el mapa
            foreach (var jugador in jugadores)
            {
                jugador.Dibujar(e.Graphics);

            }
           
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
