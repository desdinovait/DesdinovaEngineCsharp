using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
//Using XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
//Desdinova Engine
using DesdinovaEngineX;
using DesdinovaEngineX.Helpers;
using SorsAdversa;

namespace EditorEngine
{
    public partial class UIControl : UserControl
    {
        public UIControl()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Enemy newEnemy = null;
            if (comboType.SelectedItem.ToString() == "Enemy1")
            {
                newEnemy = new Enemy_Enemy1(Core.Content);
            }
            else if (comboType.SelectedItem.ToString() == "Enemy2")
            {
                newEnemy = new Enemy_Enemy2(Core.Content);
            }
            else if (comboType.SelectedItem.ToString() == "Enemy3")
            {
                 newEnemy = new Enemy_Enemy3(Core.Content);
            }

            newEnemy.Tag = comboType.SelectedItem.ToString() + " - " + newName.Text.ToString();
            newEnemy.PositionX = Convert.ToSingle(newPosX.Value);
            newEnemy.PositionY = Convert.ToSingle(newPosY.Text);
            newEnemy.PositionZ = Convert.ToSingle(10);
            Scene_Main.enemyList.Add(newEnemy);
            listBox1.Items.Add(newEnemy.Tag);
        }

        private void UIControl_Load(object sender, EventArgs e)
        {
            comboType.Items.Add("Enemy1");
            comboType.Items.Add("Enemy2");
            comboType.Items.Add("Enemy3");
            comboType.SelectedIndex = 0;
        }
    }
}
