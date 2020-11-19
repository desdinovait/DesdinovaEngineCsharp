using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SorsAdversa;
using EditorEngine;
using DesdinovaModelPipeline;

namespace EditorEngine
{
    public partial class Form_Enemy : Form
    {
        public Form_Enemy()
        {
            InitializeComponent();

            comboType.Items.Add("Enemy1");
            comboType.Items.Add("Enemy2");
            comboType.Items.Add("Enemy3");
            comboType.SelectedIndex = 0;

        }

        private void buttonAdd_Click(object sender, EventArgs e)
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
            newEnemy.PositionY = Convert.ToSingle(newPosY.Value);
            newEnemy.PositionZ = Convert.ToSingle(newPosZ.Value);
            Scene_Main.enemyList.Add(newEnemy);
            listBox1.Items.Add(newEnemy.Tag);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            Scene_Main.enemyList.RemoveAt(listBox1.SelectedIndex);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private void Form_Enemy_Load(object sender, EventArgs e)
        {
            for (int i=0; i< Scene_Main.enemyList.Count; i++)
            {
                listBox1.Items.Add(Scene_Main.enemyList[i].Tag);
            }
        }

        private void buttonCurrentZ_Click(object sender, EventArgs e)
        {
            newPosX.Value = (decimal)Scene_Main.currentBarPosition;
        }
    }
}