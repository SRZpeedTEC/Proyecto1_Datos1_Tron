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
    public partial class Victoria : Form
    {
        public Victoria(List<int> Ganador)
        {
            InitializeComponent();

            this.BackgroundImage = Image.FromFile(@"Resources\ScreenVictory.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.Ganadorlbl.Text = $"Ganador: Jugador {string.Join(", ", Ganador)}";

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Victoria_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            FormGame form = (FormGame)Application.OpenForms["FormGame"];

            if (form1 != null)
            {
                form1.Show();
                this.Close(); // Opcional: cerrar la ventana actual si lo deseas
                form.Close();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
