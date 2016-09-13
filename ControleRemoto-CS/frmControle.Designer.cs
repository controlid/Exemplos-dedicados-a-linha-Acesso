namespace ControleRemoto
{
    partial class frmControle
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
            this.btnAcionar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkRele2 = new System.Windows.Forms.CheckBox();
            this.chkRele1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gBox = new System.Windows.Forms.GroupBox();
            this.gBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAcionar
            // 
            this.btnAcionar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAcionar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAcionar.Location = new System.Drawing.Point(12, 12);
            this.btnAcionar.Name = "btnAcionar";
            this.btnAcionar.Size = new System.Drawing.Size(260, 73);
            this.btnAcionar.TabIndex = 0;
            this.btnAcionar.Text = "Acionar";
            this.btnAcionar.UseVisualStyleBackColor = true;
            this.btnAcionar.Click += new System.EventHandler(this.btnAcionar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP do Equipamento:";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(9, 44);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(110, 20);
            this.txtIP.TabIndex = 2;
            this.txtIP.Text = "192.168.0.129";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(125, 44);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(62, 20);
            this.txtUser.TabIndex = 3;
            this.txtUser.Text = "admin";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(193, 44);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(62, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.Text = "admin";
            // 
            // chkRele2
            // 
            this.chkRele2.AutoSize = true;
            this.chkRele2.Location = new System.Drawing.Point(72, 83);
            this.chkRele2.Name = "chkRele2";
            this.chkRele2.Size = new System.Drawing.Size(57, 17);
            this.chkRele2.TabIndex = 5;
            this.chkRele2.Text = "Rele 2";
            this.chkRele2.UseVisualStyleBackColor = true;
            // 
            // chkRele1
            // 
            this.chkRele1.AutoSize = true;
            this.chkRele1.Checked = true;
            this.chkRele1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRele1.Location = new System.Drawing.Point(9, 83);
            this.chkRele1.Name = "chkRele1";
            this.chkRele1.Size = new System.Drawing.Size(57, 17);
            this.chkRele1.TabIndex = 6;
            this.chkRele1.Text = "Rele 1";
            this.chkRele1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(193, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Senha:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(122, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Usuário:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Itens a serem acionados:";
            // 
            // gBox
            // 
            this.gBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gBox.Controls.Add(this.label1);
            this.gBox.Controls.Add(this.label4);
            this.gBox.Controls.Add(this.txtIP);
            this.gBox.Controls.Add(this.label3);
            this.gBox.Controls.Add(this.txtUser);
            this.gBox.Controls.Add(this.label2);
            this.gBox.Controls.Add(this.txtPassword);
            this.gBox.Controls.Add(this.chkRele1);
            this.gBox.Controls.Add(this.chkRele2);
            this.gBox.Location = new System.Drawing.Point(12, 91);
            this.gBox.Name = "gBox";
            this.gBox.Size = new System.Drawing.Size(260, 111);
            this.gBox.TabIndex = 10;
            this.gBox.TabStop = false;
            this.gBox.Text = "Configuração";
            // 
            // frmControle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 214);
            this.Controls.Add(this.gBox);
            this.Controls.Add(this.btnAcionar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmControle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Acionamento Remoto";
            this.Load += new System.EventHandler(this.frmControle_Load);
            this.gBox.ResumeLayout(false);
            this.gBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAcionar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkRele2;
        private System.Windows.Forms.CheckBox chkRele1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gBox;
    }
}

