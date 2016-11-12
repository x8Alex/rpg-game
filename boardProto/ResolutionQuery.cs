﻿using System;
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

        public ResolutionQuery()
        {
            InitializeComponent();
        }

        private void ResolutionQuery_Load(object sender, EventArgs e)
        {
            Dictionary<String, Vector2> dropBoxResolutionDictionary = new Dictionary<String, Vector2>();
            // Creates a dictionary of resolution values
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
            dropBoxResolution.SelectedIndex = 1;
        }

        private void dropBoxResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DROPLISTSELECTION = this.dropBoxResolution.GetItemText(this.dropBoxResolution.SelectedItem);
        }

        public Vector2 getResolution()
        {
            return ((KeyValuePair<String, Vector2>)dropBoxResolution.SelectedItem).Value;
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
    }
}
