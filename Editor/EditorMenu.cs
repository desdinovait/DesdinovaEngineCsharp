using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DesdinovaModelPipeline;

namespace EditorEngine
{
    public partial class EditorMenu : UserControl
    {
        public EditorMenu()
        {
            InitializeComponent();
        }

        private void informationPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            informationPanelToolStripMenuItem.Checked = !informationPanelToolStripMenuItem.Checked;
            Scene_Main.infoPanel.ToDraw = informationPanelToolStripMenuItem.Checked;    
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridToolStripMenuItem.Checked = !gridToolStripMenuItem.Checked;
            Scene_Main.grid.ToDraw = gridToolStripMenuItem.Checked;    
        }

        private void boundingSpheresAndCollisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boundingSpheresAndCollisionToolStripMenuItem.Checked = !boundingSpheresAndCollisionToolStripMenuItem.Checked;
            Scene_Main.debugShowBoundingSpheres = boundingSpheresAndCollisionToolStripMenuItem.Checked;
        }

        private void collisionSpheresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            collisionSpheresToolStripMenuItem.Checked = !collisionSpheresToolStripMenuItem.Checked;
            Scene_Main.debugShowCollisionSpheres = collisionSpheresToolStripMenuItem.Checked;
        }

        private void namesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            namesToolStripMenuItem.Checked = !namesToolStripMenuItem.Checked;
            Scene_Main.debugShowNames = namesToolStripMenuItem.Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_About aboutForm = new Form_About();
            aboutForm.ShowDialog();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Core.Exit();
            //Application.Exit();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form_Player formPlayers = new Form_Player();
            formPlayers.Show();
        }

        private void toolStripEnemy_Click(object sender, EventArgs e)
        {
            Form_Enemy formEnemy = new Form_Enemy();
            formEnemy.Show();
        }

        private void toolStripPowerup_Click(object sender, EventArgs e)
        {
            Form_PowerUp formPower = new Form_PowerUp();
            formPower.Show();
        }

        private void toolStripCamera_Click(object sender, EventArgs e)
        {
            Form_Camera formCamera = new Form_Camera();
            formCamera.Show();
        }

        private void toolStripElement_Click(object sender, EventArgs e)
        {
            Form_Element formElement = new Form_Element();
            formElement.Show();
        }

        private void toolStripPath_Click(object sender, EventArgs e)
        {
            Form_Path formPath = new Form_Path();
            formPath.Show();
        }
    }
}
