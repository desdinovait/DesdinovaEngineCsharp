namespace EditorEngine
{
    partial class Form_Player
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
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonColor = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.numericUpDownP1X = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownP1Y = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownP1Z = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownP2Y = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownP2Z = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownP2X = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP1X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP1Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP1Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP2Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP2Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP2X)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownP2Y);
            this.groupBox1.Controls.Add(this.numericUpDownP2Z);
            this.groupBox1.Controls.Add(this.numericUpDownP2X);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numericUpDownP1Y);
            this.groupBox1.Controls.Add(this.numericUpDownP1Z);
            this.groupBox1.Controls.Add(this.numericUpDownP1X);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 91);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Players";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Player1 Start Position :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Velocity :";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(108, 21);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 20);
            this.textBox7.TabIndex = 3;
            this.textBox7.Text = "0.025";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox1);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.buttonColor);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.textBox7);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(12, 109);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(310, 175);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Level";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(108, 47);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(190, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "\\\\";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Background music :";
            // 
            // buttonColor
            // 
            this.buttonColor.Location = new System.Drawing.Point(108, 99);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(34, 24);
            this.buttonColor.TabIndex = 5;
            this.buttonColor.Text = "...";
            this.buttonColor.UseVisualStyleBackColor = true;
            this.buttonColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 105);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Background color :";
            // 
            // numericUpDownP1X
            // 
            this.numericUpDownP1X.Location = new System.Drawing.Point(118, 23);
            this.numericUpDownP1X.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownP1X.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownP1X.Name = "numericUpDownP1X";
            this.numericUpDownP1X.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownP1X.TabIndex = 5;
            this.numericUpDownP1X.ValueChanged += new System.EventHandler(this.numericUpDownP1X_ValueChanged);
            // 
            // numericUpDownP1Y
            // 
            this.numericUpDownP1Y.Location = new System.Drawing.Point(180, 23);
            this.numericUpDownP1Y.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownP1Y.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownP1Y.Name = "numericUpDownP1Y";
            this.numericUpDownP1Y.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownP1Y.TabIndex = 6;
            this.numericUpDownP1Y.ValueChanged += new System.EventHandler(this.numericUpDownP1Y_ValueChanged);
            // 
            // numericUpDownP1Z
            // 
            this.numericUpDownP1Z.Location = new System.Drawing.Point(242, 23);
            this.numericUpDownP1Z.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownP1Z.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownP1Z.Name = "numericUpDownP1Z";
            this.numericUpDownP1Z.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownP1Z.TabIndex = 7;
            this.numericUpDownP1Z.ValueChanged += new System.EventHandler(this.numericUpDownP1Z_ValueChanged);
            // 
            // numericUpDownP2Y
            // 
            this.numericUpDownP2Y.Location = new System.Drawing.Point(180, 53);
            this.numericUpDownP2Y.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownP2Y.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownP2Y.Name = "numericUpDownP2Y";
            this.numericUpDownP2Y.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownP2Y.TabIndex = 10;
            this.numericUpDownP2Y.ValueChanged += new System.EventHandler(this.numericUpDownP2Y_ValueChanged);
            // 
            // numericUpDownP2Z
            // 
            this.numericUpDownP2Z.Location = new System.Drawing.Point(242, 53);
            this.numericUpDownP2Z.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownP2Z.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownP2Z.Name = "numericUpDownP2Z";
            this.numericUpDownP2Z.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownP2Z.TabIndex = 11;
            this.numericUpDownP2Z.ValueChanged += new System.EventHandler(this.numericUpDownP2Z_ValueChanged);
            // 
            // numericUpDownP2X
            // 
            this.numericUpDownP2X.Location = new System.Drawing.Point(118, 53);
            this.numericUpDownP2X.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownP2X.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownP2X.Name = "numericUpDownP2X";
            this.numericUpDownP2X.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownP2X.TabIndex = 9;
            this.numericUpDownP2X.ValueChanged += new System.EventHandler(this.numericUpDownP2X_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Player2 Start Position :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "SkyBox :";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(108, 73);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(190, 21);
            this.comboBox1.TabIndex = 9;
            // 
            // Form_Player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 294);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_Player";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Players Options";
            this.Load += new System.EventHandler(this.Form_Players_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP1X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP1Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP1Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP2Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP2Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP2X)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonColor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDownP1X;
        private System.Windows.Forms.NumericUpDown numericUpDownP1Y;
        private System.Windows.Forms.NumericUpDown numericUpDownP1Z;
        private System.Windows.Forms.NumericUpDown numericUpDownP2Y;
        private System.Windows.Forms.NumericUpDown numericUpDownP2Z;
        private System.Windows.Forms.NumericUpDown numericUpDownP2X;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
    }
}