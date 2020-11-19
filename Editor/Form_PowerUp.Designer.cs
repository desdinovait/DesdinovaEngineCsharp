namespace EditorEngine
{
    partial class Form_PowerUp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.newPosZ = new System.Windows.Forms.NumericUpDown();
            this.newPosY = new System.Windows.Forms.NumericUpDown();
            this.newPosX = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.newName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newPosZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newPosX)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.newPosZ);
            this.groupBox1.Controls.Add(this.newPosY);
            this.groupBox1.Controls.Add(this.newPosX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.newName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(179, 190);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New PowerUp :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(62, 129);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Z :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Y :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "X :";
            // 
            // newPosZ
            // 
            this.newPosZ.Location = new System.Drawing.Point(85, 126);
            this.newPosZ.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.newPosZ.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            -2147483648});
            this.newPosZ.Name = "newPosZ";
            this.newPosZ.Size = new System.Drawing.Size(88, 20);
            this.newPosZ.TabIndex = 10;
            this.newPosZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // newPosY
            // 
            this.newPosY.Location = new System.Drawing.Point(85, 100);
            this.newPosY.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.newPosY.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            -2147483648});
            this.newPosY.Name = "newPosY";
            this.newPosY.Size = new System.Drawing.Size(88, 20);
            this.newPosY.TabIndex = 9;
            this.newPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // newPosX
            // 
            this.newPosX.Location = new System.Drawing.Point(85, 74);
            this.newPosX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.newPosX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.newPosX.Name = "newPosX";
            this.newPosX.Size = new System.Drawing.Size(88, 20);
            this.newPosX.TabIndex = 3;
            this.newPosX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Tag :";
            // 
            // newName
            // 
            this.newName.Location = new System.Drawing.Point(62, 48);
            this.newName.Name = "newName";
            this.newName.Size = new System.Drawing.Size(111, 20);
            this.newName.TabIndex = 7;
            this.newName.Text = "noname";
            this.newName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Position :";
            // 
            // comboType
            // 
            this.comboType.FormattingEnabled = true;
            this.comboType.Location = new System.Drawing.Point(62, 19);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(111, 21);
            this.comboType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Type :";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(62, 152);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(111, 28);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // Form_PowerUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 273);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_PowerUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_PowerUp";
            this.Load += new System.EventHandler(this.Form_PowerUp_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newPosZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newPosX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown newPosZ;
        private System.Windows.Forms.NumericUpDown newPosY;
        private System.Windows.Forms.NumericUpDown newPosX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox newName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAdd;
    }
}