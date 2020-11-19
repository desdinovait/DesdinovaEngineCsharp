using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace EditorEngine
{
    public partial class PositionBar : UserControl
    {
        public PositionBar()
        {
            InitializeComponent();
        }

        private void trackBarPosition_Scroll(object sender, EventArgs e)
        {
            labelCurrentPosition.Text = trackBarPosition.Value.ToString();
            Scene_Main.currentBarPosition = Convert.ToSingle(trackBarPosition.Value);
        }
    }
}
