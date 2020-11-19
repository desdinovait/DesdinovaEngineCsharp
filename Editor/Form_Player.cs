using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EditorEngine
{
    public partial class Form_Player : Form
    {
        public Form_Player()
        {
            InitializeComponent();
        }

        private void Form_Players_Load(object sender, EventArgs e)
        {
            numericUpDownP1X.Value = (decimal)Scene_Main.player1.PositionX;
            numericUpDownP1Y.Value = (decimal)Scene_Main.player1.PositionY;
            numericUpDownP1Z.Value = (decimal)Scene_Main.player1.PositionZ;

            numericUpDownP2X.Value = (decimal)Scene_Main.player2.PositionX;
            numericUpDownP2Y.Value = (decimal)Scene_Main.player2.PositionY;
            numericUpDownP2Z.Value = (decimal)Scene_Main.player2.PositionZ;

        }


        private void buttonColor_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            colorDialog1.FullOpen = true;
            colorDialog1.AnyColor = true;
            colorDialog1.Color = Color.FromArgb(Scene_Main.backgroundColor.A, Scene_Main.backgroundColor.R, Scene_Main.backgroundColor.G, Scene_Main.backgroundColor.B);
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Scene_Main.backgroundColor = new Microsoft.Xna.Framework.Graphics.Color(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B, colorDialog1.Color.A);
            }

        }

        private void numericUpDownP1X_ValueChanged(object sender, EventArgs e)
        {
            Scene_Main.player1.PositionX = (float)numericUpDownP1X.Value;
        }

        private void numericUpDownP1Y_ValueChanged(object sender, EventArgs e)
        {
            Scene_Main.player1.PositionY = (float)numericUpDownP1Y.Value;
        }

        private void numericUpDownP1Z_ValueChanged(object sender, EventArgs e)
        {
            Scene_Main.player1.PositionZ = (float)numericUpDownP1Z.Value;
        }

        private void numericUpDownP2X_ValueChanged(object sender, EventArgs e)
        {
            Scene_Main.player2.PositionX = (float)numericUpDownP2X.Value;
        }

        private void numericUpDownP2Y_ValueChanged(object sender, EventArgs e)
        {
            Scene_Main.player2.PositionY = (float)numericUpDownP2Y.Value;
        }

        private void numericUpDownP2Z_ValueChanged(object sender, EventArgs e)
        {
            Scene_Main.player2.PositionZ = (float)numericUpDownP2Z.Value;
        }

    }
}