using LightPuzzle.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightPuzzle.AppCode
{
    public class LightBoxHelper
    {
        public static bool CheckWin(List<LightBox> lightBoxes, int gridSize)
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
        public static LightBox GetLightBoxByCoordinates(List<LightBox> lightBoxes, LightBox.Coordinates coordinates)
        {
            LightBox lightBoxMatch = null;
            foreach (LightBox lightBox in lightBoxes)
            {
                if (lightBox.Location == coordinates)
                    lightBoxMatch = lightBox;
            }
            return lightBoxMatch;
        }

        //Options dialog for setting grid size
        public static int ShowOptions(int gridSize)
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
            NumericUpDown txtGridSize = new NumericUpDown() { Left = 50, Top = 50, Width = 400, Minimum = 5, Maximum = 10, Value = 5 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(txtGridSize);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? Convert.ToInt32(txtGridSize.Text) : gridSize;
        }

        public static bool IsNeighbour(LightBox lightBox, LightBox compareLightBox)
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

        public static void SetLightBoxNeighbours(List<LightBox> lightBoxes)
        {
            foreach (LightBox lightBox in lightBoxes)
                foreach (LightBox compareLightBox in lightBoxes)
                    if (LightBoxHelper.IsNeighbour(lightBox, compareLightBox))
                        lightBox.Neighbours.Add(compareLightBox);
        }
    }
}
