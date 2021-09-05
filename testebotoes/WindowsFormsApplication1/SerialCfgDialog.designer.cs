namespace WindowsFormsApplication1
{
    partial class SerialCfgDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialCfgDialog));
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbbaudrate = new System.Windows.Forms.ComboBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.ComboBox();
            this.remote_control = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Port:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Baud Rate:";
            // 
            // cmbbaudrate
            // 
            this.cmbbaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbbaudrate.FormattingEnabled = true;
            this.cmbbaudrate.Items.AddRange(new object[] {
            "9600",
            "15000"});
            this.cmbbaudrate.Location = new System.Drawing.Point(8, 93);
            this.cmbbaudrate.Name = "cmbbaudrate";
            this.cmbbaudrate.Size = new System.Drawing.Size(108, 21);
            this.cmbbaudrate.TabIndex = 15;
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(252, 131);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(61, 23);
            this.btn_ok.TabIndex = 16;
            this.btn_ok.Text = "Ok";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtPort
            // 
            this.txtPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtPort.FormattingEnabled = true;
            this.txtPort.Location = new System.Drawing.Point(8, 37);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(97, 21);
            this.txtPort.TabIndex = 19;
            // 
            // remote_control
            // 
            this.remote_control.AutoSize = true;
            this.remote_control.Location = new System.Drawing.Point(8, 131);
            this.remote_control.Margin = new System.Windows.Forms.Padding(2);
            this.remote_control.Name = "remote_control";
            this.remote_control.Size = new System.Drawing.Size(99, 17);
            this.remote_control.TabIndex = 20;
            this.remote_control.Text = "Remote Control";
            this.remote_control.UseVisualStyleBackColor = true;
            this.remote_control.CheckedChanged += new System.EventHandler(this.remote_control_CheckedChanged);
            // 
            // SerialCfgDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 168);
            this.Controls.Add(this.remote_control);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.cmbbaudrate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SerialCfgDialog";
            this.Text = "Serial Communication";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbbaudrate;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.ComboBox txtPort;
        private System.Windows.Forms.CheckBox remote_control;
    }
}

