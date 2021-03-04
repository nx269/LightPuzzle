using LightPuzzle.AppCode;
using LightPuzzle.Modals;
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
        List<LightBox> lightBoxes = new List<LightBox>();//list of created lightbox objects
        static int gridSize = 5;//grid size as set by options
        public Form1()
        {
            InitializeComponent();
            NewGame();
        }



        public void NewGame()
        {
            SetLayout();
            Random rd = new Random();
            int xCoord = 0;
            int yCoord = 0;
            int activeCount = 0;
            for (int i = 0; i < (gridSize * gridSize); i++)
            {
                bool lightActive = rd.Next(0, 10) > 8;//checking greater than 8 to lower chances of initially starting with too many lightboxes
                if (lightActive)
                    activeCount++;

                if (activeCount == 0 && xCoord == (gridSize - 1) && yCoord == (gridSize - 1))//if no lights have been turned on, turn last lightbox on
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
            LightBoxHelper.SetLightBoxNeighbours(lightBoxes);
        }


        //Initial setup for grid
        public void SetLayout()
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
        }


        protected void lightBoxClick(object sender, EventArgs e, LightBox.Coordinates coordinates)
        {
            LightBox clickedLightBox = LightBoxHelper.GetLightBoxByCoordinates(lightBoxes, coordinates);
            clickedLightBox.Active = !clickedLightBox.Active;
            foreach (LightBox neighbours in clickedLightBox.Neighbours)
                neighbours.Active = !neighbours.Active;

            bool win = LightBoxHelper.CheckWin(lightBoxes, gridSize);
            if (win)
            {
                DialogResult dialogResult = MessageBox.Show("Would you like to try again?", "Well Done", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                    NewGame();
                else if (dialogResult == DialogResult.No)
                    Application.Exit();
            }
        }

        #region Menu Options

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridSize = LightBoxHelper.ShowOptions(gridSize);
            NewGame();
        }

        #endregion Menu Options
    }
}
