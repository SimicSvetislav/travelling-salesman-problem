using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSP
{
    public partial class MyForm : Form
    {
        Pen red = new Pen(Color.Red);
        Pen black = new Pen(Color.Black);
        SolidBrush fillBlack = new SolidBrush(Color.Black);
        SolidBrush fillMagenta = new SolidBrush(Color.Magenta);
        private List<City> listOfCities;
        private string message;
        private double distanceT;
        private string timeSpan;

        public MyForm()
        {
            InitializeComponent();
        }

        public MyForm(List<City> l, string message, double distance, string time)
        {
            this.listOfCities = l;
            this.message = message;
            this.distanceT = distance;
            this.timeSpan = time;
            InitializeComponent();
        }

        private void MyForm_Paint(object sender, PaintEventArgs e)
        {
            algName.Text = message;
            distance.Text = distanceT.ToString("#.000");
            time.Text = timeSpan;
            Graphics g = e.Graphics;
            int scaleFactor = 4;
            int i;

            g.DrawEllipse(black, scaleFactor * listOfCities[0].CoorX - 5, scaleFactor * listOfCities[0].CoorY - 5, 10, 10);
            g.FillEllipse(fillMagenta, scaleFactor * listOfCities[0].CoorX - 5, scaleFactor * listOfCities[0].CoorY - 5, 10, 10);

            for (i = 0; i < listOfCities.Count - 1; ++i)
            {
                g.DrawLine(red, scaleFactor * listOfCities[i].CoorX, scaleFactor * listOfCities[i].CoorY, scaleFactor * listOfCities[i + 1].CoorX, scaleFactor * listOfCities[i + 1].CoorY);
                if (i!=0) {
                    g.DrawEllipse(black, scaleFactor * listOfCities[i].CoorX - 2, scaleFactor * listOfCities[i].CoorY - 2, 5, 5);
                    g.FillEllipse(fillBlack, scaleFactor * listOfCities[i].CoorX - 2, scaleFactor * listOfCities[i].CoorY - 2, 5, 5);
                }
            }

            g.DrawLine(red, scaleFactor * listOfCities[i].CoorX, scaleFactor * listOfCities[i].CoorY, scaleFactor * listOfCities[0].CoorX, scaleFactor * listOfCities[0].CoorY);
        }
    }
}
