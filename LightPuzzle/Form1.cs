using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightPuzzle
{
    public partial class Form1 : Form
    {
        List<LightBox> lightBoxes = new List<LightBox>();
        static int gridSize = 5;
        public Form1()
        {
            InitializeComponent();
            NewGame();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void NewGame()
        {
            panelLayout.RowCount = gridSize;
            panelLayout.ColumnCount = gridSize;
            foreach (RowStyle style in panelLayout.RowStyles)
            {
                style.SizeType = SizeType.Percent;
                style.Height = 100 / gridSize;
            }
            foreach (ColumnStyle style in panelLayout.ColumnStyles)
            {
                style.SizeType = SizeType.Percent;
                style.Width = 100 / gridSize;
            }
            panelLayout.Controls.Clear();
            lightBoxes.Clear();
            Random rd = new Random();
            int xCoord = 0;
            int yCoord = 0;
            int activeCount = 0;
            for (int i = 0; i < (gridSize * gridSize); i++)
            {
                int num = rd.Next(0, 10);
                bool lightActive = rd.Next(0, 10) > 8;
                if (lightActive)
                    activeCount++;

                if (activeCount == 0 && xCoord == (gridSize - 1) && yCoord == (gridSize - 1))
                    lightActive = true;
                LightBox lightBox = new LightBox(lightActive, new LightBox.Coordinates(xCoord, yCoord));
                lightBox.Light.Click += delegate (object sender, EventArgs e) { lightBoxClick(sender, e, lightBox.Location); };
                lightBoxes.Add(lightBox);

                panelLayout.Controls.Add(lightBox.Light);
                xCoord++;
                if (xCoord > (gridSize - 1))
                {
                    xCoord = 0;
                    yCoord++;
                }
            }

            foreach (LightBox lightBox in lightBoxes)
            {

                foreach (LightBox compareLightBox in lightBoxes)
                {
                    if (IsNeighbour(lightBox, compareLightBox))
                        lightBox.Neighbours.Add(compareLightBox);

                }
            }
        }

        public bool IsNeighbour(LightBox lightBox, LightBox compareLightBox)
        {
            if (lightBox.Location.Column == compareLightBox.Location.Column &&
                (lightBox.Location.Row == compareLightBox.Location.Row + 1 || lightBox.Location.Row == compareLightBox.Location.Row - 1))
                return true;
            else if (lightBox.Location.Row == compareLightBox.Location.Row &&
                (lightBox.Location.Column == compareLightBox.Location.Column + 1 || lightBox.Location.Column == compareLightBox.Location.Column - 1))
                return true;
            else
                return false;
        }
        protected void lightBoxClick(object sender, EventArgs e, LightBox.Coordinates coordinates)
        {
            LightBox clickedLightBox = GetLightBoxByCoordinates(coordinates);
            clickedLightBox.Active = !clickedLightBox.Active;
            foreach (LightBox neighbours in clickedLightBox.Neighbours)
            {
                neighbours.Active = !neighbours.Active;
            }
            bool win = CheckWin();
            if (win)
            {
                DialogResult dialogResult = MessageBox.Show("Would you like to try again?", "Well Done", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    NewGame();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            

        }

        private bool CheckWin()
        {
            int activeLightBoxes = 0;
            foreach (LightBox lightbox in lightBoxes)
            {
                if (lightbox.Active)
                    activeLightBoxes++;
            }
            if (activeLightBoxes == (gridSize * gridSize))
                return true;
            return false;
        }

        public LightBox GetLightBoxByCoordinates(LightBox.Coordinates coordinates)
        {
            LightBox lightBoxMatch = null;
            foreach (LightBox lightBox in lightBoxes)
            {
                if (lightBox.Location == coordinates)
                    lightBoxMatch = lightBox;
            }
            return lightBoxMatch;
        }

        public class LightBox
        {
            private bool _active;
            public LightBox(bool active, Coordinates location)
            {
                Light = new Panel();
                _active = active;
                Location = location;
                Light.BackColor = active ? Color.LimeGreen : Color.DarkGreen;
                Neighbours = new List<LightBox>();
            }

            public Panel Light { get; set; }
            public bool Active
            {
                get { return _active; }
                set
                {
                    _active = value;
                    Light.BackColor = _active ? Color.LimeGreen : Color.DarkGreen;
                }
            }
            public Coordinates Location { get; set; }
            public List<LightBox> Neighbours { get; set; }

            public class Coordinates
            {
                public Coordinates(int row, int column)
                {
                    Row = row;
                    Column = column;
                }

                public int Row { get; set; }
                public int Column { get; set; }
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridSize = ShowOptions();
            NewGame();
        }

        public static int ShowOptions()
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Options",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Choose your grid size" };
            NumericUpDown txtGridSize = new NumericUpDown() { Left = 50, Top = 50, Width = 400, Minimum = 3, Maximum = 9, Value = 5 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(txtGridSize);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? Convert.ToInt32(txtGridSize.Text) : gridSize;
        }
    }
}
