using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace boardProto
{
    public partial class ResolutionQuery : Form
    {
        bool LAUNCHGAME = false;
        bool WINDOWED = true;
        int borderWidth;
        Vector2 borderSize;
        System.Drawing.Rectangle screenRectangle;

        public ResolutionQuery()
        {
            InitializeComponent();
            screenRectangle = RectangleToScreen(this.ClientRectangle);
            borderWidth = (this.Width - this.ClientSize.Width) / 2;
            borderSize = new Vector2((float)(borderWidth), (float)(this.Height - this.ClientSize.Height - 2 * borderWidth));
            Console.WriteLine(borderSize.ToString());
        }

        private void ResolutionQuery_Load(object sender, EventArgs e)
        {
            Dictionary<String, Vector2> dropBoxResolutionDictionary = new Dictionary<String, Vector2>();
            // Creates a dictionary of resolution values
            dropBoxResolutionDictionary.Add("1920x1200", new Vector2(1920, 1200));
            dropBoxResolutionDictionary.Add("1680x1050", new Vector2(1680, 1050));
            dropBoxResolutionDictionary.Add("1366x768", new Vector2(1366,768));
            dropBoxResolutionDictionary.Add("1280x800", new Vector2(1280, 800));
            dropBoxResolutionDictionary.Add("1280x720", new Vector2(1280, 720));
            dropBoxResolutionDictionary.Add("1024x768", new Vector2(1024, 768));
            dropBoxResolutionDictionary.Add("800x600", new Vector2(800,600));
            dropBoxResolution.DataSource = new BindingSource(dropBoxResolutionDictionary, null);
            dropBoxResolution.DisplayMember = "Key";
            dropBoxResolution.ValueMember = "Value";

            // Set the default choice to the second member of the dictionary
            dropBoxResolution.SelectedIndex = 2;
        }

        private void dropBoxResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DROPLISTSELECTION = this.dropBoxResolution.GetItemText(this.dropBoxResolution.SelectedItem);
        }

        public Vector2 getResolution()
        {
            return ((KeyValuePair<String, Vector2>)dropBoxResolution.SelectedItem).Value;
        }

        public bool getScreenMode()
        {
            return WINDOWED;
        }

        public bool getLaunchGame()
        {
            return LAUNCHGAME;
        }

        private void buttonLaunch_Click(object sender, EventArgs e)
        {
            LAUNCHGAME = true;
            this.Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBoxFullscreen_CheckedChanged(object sender, EventArgs e)
        {
            WINDOWED = !WINDOWED;
        }

        public Vector2 BorderSize
        {
            get { return borderSize; }
        }

        public int BorderWidth
        {
            get { return borderWidth; }
        }
    }
}
