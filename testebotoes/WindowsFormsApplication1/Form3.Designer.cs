namespace WindowsFormsApplication1
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.cmb_stype = new System.Windows.Forms.ComboBox();
            this.lbl_offset = new System.Windows.Forms.Label();
            this.lbl_ampl = new System.Windows.Forms.Label();
            this.lbl_frq = new System.Windows.Forms.Label();
            this.knob_offset = new Knobs.Knob();
            this.knob_ampl = new Knobs.Knob();
            this.knob_frq = new Knobs.Knob();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radio_x1000 = new System.Windows.Forms.RadioButton();
            this.radio_x100 = new System.Windows.Forms.RadioButton();
            this.radio_x10 = new System.Windows.Forms.RadioButton();
            this.radio_x1 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmb_stype
            // 
            this.cmb_stype.FormattingEnabled = true;
            this.cmb_stype.Location = new System.Drawing.Point(60, 142);
            this.cmb_stype.Name = "cmb_stype";
            this.cmb_stype.Size = new System.Drawing.Size(147, 21);
            this.cmb_stype.TabIndex = 31;
            this.cmb_stype.SelectedIndexChanged += new System.EventHandler(this.cmb_stype_SelectedIndexChanged);
            // 
            // lbl_offset
            // 
            this.lbl_offset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_offset.Location = new System.Drawing.Point(554, 108);
            this.lbl_offset.Name = "lbl_offset";
            this.lbl_offset.Size = new System.Drawing.Size(96, 28);
            this.lbl_offset.TabIndex = 30;
            this.lbl_offset.Text = "label3";
            this.lbl_offset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ampl
            // 
            this.lbl_ampl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_ampl.Location = new System.Drawing.Point(452, 108);
            this.lbl_ampl.Name = "lbl_ampl";
            this.lbl_ampl.Size = new System.Drawing.Size(96, 28);
            this.lbl_ampl.TabIndex = 29;
            this.lbl_ampl.Text = "label2";
            this.lbl_ampl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_frq
            // 
            this.lbl_frq.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_frq.Location = new System.Drawing.Point(350, 108);
            this.lbl_frq.Name = "lbl_frq";
            this.lbl_frq.Size = new System.Drawing.Size(96, 28);
            this.lbl_frq.TabIndex = 28;
            this.lbl_frq.Text = "label1";
            this.lbl_frq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // knob_offset
            // 
            this.knob_offset.KnobBorderStyle = Knobs.KnobStyle.FlatBorder;
            this.knob_offset.KnobColor = System.Drawing.Color.ForestGreen;
            this.knob_offset.KnobRadius = 20;
            this.knob_offset.Location = new System.Drawing.Point(572, 32);
            this.knob_offset.MarkerColor = System.Drawing.Color.Black;
            this.knob_offset.Maximum = 10;
            this.knob_offset.Name = "knob_offset";
            this.knob_offset.Size = new System.Drawing.Size(65, 99);
            this.knob_offset.TabIndex = 27;
            this.knob_offset.Text = "Offset";
            this.knob_offset.TickColor = System.Drawing.Color.Black;
            this.knob_offset.ValueChanged += new System.EventHandler(this.knob_offset_ValueChanged);
            // 
            // knob_ampl
            // 
            this.knob_ampl.KnobBorderStyle = Knobs.KnobStyle.FlatBorder;
            this.knob_ampl.KnobColor = System.Drawing.Color.Navy;
            this.knob_ampl.KnobRadius = 20;
            this.knob_ampl.Location = new System.Drawing.Point(467, 32);
            this.knob_ampl.MarkerColor = System.Drawing.Color.Black;
            this.knob_ampl.Maximum = 10;
            this.knob_ampl.Name = "knob_ampl";
            this.knob_ampl.Size = new System.Drawing.Size(65, 99);
            this.knob_ampl.TabIndex = 26;
            this.knob_ampl.Text = "Amplitude";
            this.knob_ampl.TickColor = System.Drawing.Color.Black;
            this.knob_ampl.ValueChanged += new System.EventHandler(this.knob_ampl_ValueChanged);
            // 
            // knob_frq
            // 
            this.knob_frq.KnobBorderStyle = Knobs.KnobStyle.FlatBorder;
            this.knob_frq.KnobColor = System.Drawing.Color.Maroon;
            this.knob_frq.KnobRadius = 20;
            this.knob_frq.Location = new System.Drawing.Point(363, 32);
            this.knob_frq.MarkerColor = System.Drawing.Color.Black;
            this.knob_frq.Maximum = 10;
            this.knob_frq.Minimum = 1;
            this.knob_frq.Name = "knob_frq";
            this.knob_frq.Size = new System.Drawing.Size(65, 99);
            this.knob_frq.TabIndex = 25;
            this.knob_frq.Text = "Frequencia";
            this.knob_frq.TickColor = System.Drawing.Color.Black;
            this.knob_frq.Value = 1;
            this.knob_frq.ValueChanged += new System.EventHandler(this.knob_frq_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radio_x1000);
            this.groupBox1.Controls.Add(this.radio_x100);
            this.groupBox1.Controls.Add(this.radio_x10);
            this.groupBox1.Controls.Add(this.radio_x1);
            this.groupBox1.Location = new System.Drawing.Point(60, 45);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(246, 75);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            // 
            // radio_x1000
            // 
            this.radio_x1000.AutoSize = true;
            this.radio_x1000.Location = new System.Drawing.Point(23, 27);
            this.radio_x1000.Margin = new System.Windows.Forms.Padding(2);
            this.radio_x1000.Name = "radio_x1000";
            this.radio_x1000.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radio_x1000.Size = new System.Drawing.Size(49, 17);
            this.radio_x1000.TabIndex = 39;
            this.radio_x1000.Text = "1000";
            this.radio_x1000.UseVisualStyleBackColor = true;
            this.radio_x1000.CheckedChanged += new System.EventHandler(this.radiobutns_CheckedChanged);
            // 
            // radio_x100
            // 
            this.radio_x100.AutoSize = true;
            this.radio_x100.Location = new System.Drawing.Point(84, 27);
            this.radio_x100.Margin = new System.Windows.Forms.Padding(2);
            this.radio_x100.Name = "radio_x100";
            this.radio_x100.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radio_x100.Size = new System.Drawing.Size(43, 17);
            this.radio_x100.TabIndex = 38;
            this.radio_x100.Text = "100";
            this.radio_x100.UseVisualStyleBackColor = true;
            this.radio_x100.CheckedChanged += new System.EventHandler(this.radiobutns_CheckedChanged);
            // 
            // radio_x10
            // 
            this.radio_x10.AutoSize = true;
            this.radio_x10.Checked = true;
            this.radio_x10.Location = new System.Drawing.Point(140, 27);
            this.radio_x10.Margin = new System.Windows.Forms.Padding(2);
            this.radio_x10.Name = "radio_x10";
            this.radio_x10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radio_x10.Size = new System.Drawing.Size(37, 17);
            this.radio_x10.TabIndex = 37;
            this.radio_x10.TabStop = true;
            this.radio_x10.Text = "10";
            this.radio_x10.UseVisualStyleBackColor = true;
            this.radio_x10.CheckedChanged += new System.EventHandler(this.radiobutns_CheckedChanged);
            // 
            // radio_x1
            // 
            this.radio_x1.AutoSize = true;
            this.radio_x1.Location = new System.Drawing.Point(188, 27);
            this.radio_x1.Margin = new System.Windows.Forms.Padding(2);
            this.radio_x1.Name = "radio_x1";
            this.radio_x1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radio_x1.Size = new System.Drawing.Size(31, 17);
            this.radio_x1.TabIndex = 36;
            this.radio_x1.Text = "1";
            this.radio_x1.UseVisualStyleBackColor = true;
            this.radio_x1.CheckedChanged += new System.EventHandler(this.radiobutns_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(523, 170);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 35);
            this.button1.TabIndex = 37;
            this.button1.Text = "Confirmar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 231);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmb_stype);
            this.Controls.Add(this.lbl_offset);
            this.Controls.Add(this.lbl_ampl);
            this.Controls.Add(this.lbl_frq);
            this.Controls.Add(this.knob_offset);
            this.Controls.Add(this.knob_ampl);
            this.Controls.Add(this.knob_frq);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form3";
            this.Text = "Signal Generator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_stype;
        private System.Windows.Forms.Label lbl_offset;
        private System.Windows.Forms.Label lbl_ampl;
        private System.Windows.Forms.Label lbl_frq;
        private Knobs.Knob knob_offset;
        private Knobs.Knob knob_ampl;
        private Knobs.Knob knob_frq;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radio_x1000;
        private System.Windows.Forms.RadioButton radio_x100;
        private System.Windows.Forms.RadioButton radio_x10;
        private System.Windows.Forms.RadioButton radio_x1;
        private System.Windows.Forms.Button button1;

    }
}