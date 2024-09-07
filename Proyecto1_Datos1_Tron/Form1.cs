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
    public partial class Form1 : Form
    {
        public AdministradorSonido MusicaGeneral = new AdministradorSonido();
        public Form1()
        {
            InitializeComponent();

            this.BackgroundImage = Image.FromFile(@"Resources\backGroundTRON.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            MusicaGeneral.ReproducirSonido(@"Resources\CancionPrincipal.wav");


        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormGame formGame = new FormGame();
            Console.WriteLine("Button Clicked");
            formGame.Show();
            this.Hide();

            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormGame formGame = new FormGame(true);
            Console.WriteLine("Modo Versus Activado");
            formGame.Show();
            this.Hide();
        }
    }
}
