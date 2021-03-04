using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightPuzzle.Modals
{
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
}
