using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SorsAdversa;
using DesdinovaModelPipeline;

namespace EditorEngine
{
    public partial class Form_PowerUp : Form
    {
        public Form_PowerUp()
        {
            InitializeComponent();

            comboType.Items.Add("Alpha Strike");
            comboType.Items.Add("Damage Booster");
            comboType.Items.Add("Regeneration Booster");
            comboType.Items.Add("Shield Booster");
            comboType.Items.Add("Speed Booster");
            comboType.SelectedIndex = 0;
        }

        private void Form_PowerUp_Load(object sender, EventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            PowerUp newPowerup = null;
            if (comboType.SelectedItem.ToString() == "Alpha Strike")
            {
                newPowerup = new PowerUp_AlphaStrike(Core.Content);
            }
            else if (comboType.SelectedItem.ToString() == "Damage Booster")
            {
                newPowerup = new PowerUp_DamageBooster(Core.Content);
            }
            else if (comboType.SelectedItem.ToString() == "Regeneration Booster")
            {
                newPowerup = new PowerUp_RegenerationBooster(Core.Content);
            }
            else if (comboType.SelectedItem.ToString() == "Shield Booster")
            {
                newPowerup = new PowerUp_ShieldBooster(Core.Content);
            }
            else if (comboType.SelectedItem.ToString() == "Speed Booster")
            {
                newPowerup = new PowerUp_SpeedBooster(Core.Content);
            }

            newPowerup.Tag = comboType.SelectedItem.ToString() + " - " + newName.Text.ToString();
            newPowerup.PositionX = Convert.ToSingle(newPosX.Value);
            newPowerup.PositionY = Convert.ToSingle(newPosY.Value);
            newPowerup.PositionZ = Convert.ToSingle(newPosZ.Value);
            Scene_Main.powerupList.Add(newPowerup);
            //listBox1.Items.Add(newPowerup.Tag);
        }
    }
}