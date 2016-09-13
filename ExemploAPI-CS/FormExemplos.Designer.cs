namespace ExemploAPI
{
    partial class frmExemplos
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
            this.tbc = new System.Windows.Forms.TabControl();
            this.tbDevice = new System.Windows.Forms.TabPage();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSSL = new System.Windows.Forms.CheckBox();
            this.nmPort = new System.Windows.Forms.NumericUpDown();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbAcoes = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnReboot = new System.Windows.Forms.Button();
            this.cmbGiro = new System.Windows.Forms.ComboBox();
            this.btnInfo = new System.Windows.Forms.Button();
            this.btnGiro = new System.Windows.Forms.Button();
            this.btnRele2 = new System.Windows.Forms.Button();
            this.btnRele3 = new System.Windows.Forms.Button();
            this.btnRele1 = new System.Windows.Forms.Button();
            this.btnRele4 = new System.Windows.Forms.Button();
            this.tbConfig = new System.Windows.Forms.TabPage();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnDataHora = new System.Windows.Forms.Button();
            this.tbUsers = new System.Windows.Forms.TabPage();
            this.button12 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button13 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.button11 = new System.Windows.Forms.Button();
            this.tbGroups = new System.Windows.Forms.TabPage();
            this.txtOut = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAgora = new System.Windows.Forms.Button();
            this.tbc.SuspendLayout();
            this.tbDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmPort)).BeginInit();
            this.tbAcoes.SuspendLayout();
            this.tbConfig.SuspendLayout();
            this.tbUsers.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbc
            // 
            this.tbc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbc.Controls.Add(this.tbDevice);
            this.tbc.Controls.Add(this.tbAcoes);
            this.tbc.Controls.Add(this.tbConfig);
            this.tbc.Controls.Add(this.tbUsers);
            this.tbc.Controls.Add(this.tbGroups);
            this.tbc.Location = new System.Drawing.Point(0, 0);
            this.tbc.Name = "tbc";
            this.tbc.Padding = new System.Drawing.Point(10, 8);
            this.tbc.SelectedIndex = 0;
            this.tbc.Size = new System.Drawing.Size(385, 236);
            this.tbc.TabIndex = 0;
            this.tbc.SelectedIndexChanged += new System.EventHandler(this.tbc_SelectedIndexChanged);
            // 
            // tbDevice
            // 
            this.tbDevice.Controls.Add(this.txtPassword);
            this.tbDevice.Controls.Add(this.txtUser);
            this.tbDevice.Controls.Add(this.label4);
            this.tbDevice.Controls.Add(this.label3);
            this.tbDevice.Controls.Add(this.label2);
            this.tbDevice.Controls.Add(this.label1);
            this.tbDevice.Controls.Add(this.chkSSL);
            this.tbDevice.Controls.Add(this.nmPort);
            this.tbDevice.Controls.Add(this.txtIP);
            this.tbDevice.Controls.Add(this.btnLogin);
            this.tbDevice.Location = new System.Drawing.Point(4, 32);
            this.tbDevice.Name = "tbDevice";
            this.tbDevice.Padding = new System.Windows.Forms.Padding(3);
            this.tbDevice.Size = new System.Drawing.Size(377, 200);
            this.tbDevice.TabIndex = 0;
            this.tbDevice.Text = "Equipamento";
            this.tbDevice.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(119, 63);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.Text = "admin";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(11, 63);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(100, 20);
            this.txtUser.TabIndex = 8;
            this.txtUser.Text = "admin";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(116, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Senha";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Usuário";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Porta";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP / DNS";
            // 
            // chkSSL
            // 
            this.chkSSL.AutoSize = true;
            this.chkSSL.Location = new System.Drawing.Point(180, 26);
            this.chkSSL.Name = "chkSSL";
            this.chkSSL.Size = new System.Drawing.Size(46, 17);
            this.chkSSL.TabIndex = 3;
            this.chkSSL.Text = "SSL";
            this.chkSSL.UseVisualStyleBackColor = true;
            // 
            // nmPort
            // 
            this.nmPort.Location = new System.Drawing.Point(119, 25);
            this.nmPort.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nmPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmPort.Name = "nmPort";
            this.nmPort.Size = new System.Drawing.Size(55, 20);
            this.nmPort.TabIndex = 2;
            this.nmPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(11, 24);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 20);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "192.168.0.155";
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Location = new System.Drawing.Point(232, 24);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(137, 59);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Conectar";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbAcoes
            // 
            this.tbAcoes.Controls.Add(this.label6);
            this.tbAcoes.Controls.Add(this.label7);
            this.tbAcoes.Controls.Add(this.btnReboot);
            this.tbAcoes.Controls.Add(this.cmbGiro);
            this.tbAcoes.Controls.Add(this.btnInfo);
            this.tbAcoes.Controls.Add(this.btnGiro);
            this.tbAcoes.Controls.Add(this.btnRele2);
            this.tbAcoes.Controls.Add(this.btnRele3);
            this.tbAcoes.Controls.Add(this.btnRele1);
            this.tbAcoes.Controls.Add(this.btnRele4);
            this.tbAcoes.Location = new System.Drawing.Point(4, 32);
            this.tbAcoes.Name = "tbAcoes";
            this.tbAcoes.Size = new System.Drawing.Size(377, 200);
            this.tbAcoes.TabIndex = 4;
            this.tbAcoes.Text = "Ações";
            this.tbAcoes.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Ações Específicas";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Ações Comum";
            // 
            // btnReboot
            // 
            this.btnReboot.Location = new System.Drawing.Point(215, 24);
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.Size = new System.Drawing.Size(82, 24);
            this.btnReboot.TabIndex = 7;
            this.btnReboot.Text = "Reboot";
            this.btnReboot.UseVisualStyleBackColor = true;
            this.btnReboot.Click += new System.EventHandler(this.btnReboot_Click);
            // 
            // cmbGiro
            // 
            this.cmbGiro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGiro.FormattingEnabled = true;
            this.cmbGiro.Items.AddRange(new object[] {
            "Ambos",
            "Horário",
            "Anti-Horário"});
            this.cmbGiro.Location = new System.Drawing.Point(173, 97);
            this.cmbGiro.Name = "cmbGiro";
            this.cmbGiro.Size = new System.Drawing.Size(121, 21);
            this.cmbGiro.TabIndex = 6;
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(11, 24);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(201, 23);
            this.btnInfo.TabIndex = 0;
            this.btnInfo.Text = "Ler Informações do equipamento";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnGiro
            // 
            this.btnGiro.Location = new System.Drawing.Point(11, 96);
            this.btnGiro.Name = "btnGiro";
            this.btnGiro.Size = new System.Drawing.Size(156, 24);
            this.btnGiro.TabIndex = 5;
            this.btnGiro.Text = "Catraca Liberar Sentido";
            this.btnGiro.UseVisualStyleBackColor = true;
            this.btnGiro.Click += new System.EventHandler(this.btnGiro_Click);
            // 
            // btnRele2
            // 
            this.btnRele2.Location = new System.Drawing.Point(99, 66);
            this.btnRele2.Name = "btnRele2";
            this.btnRele2.Size = new System.Drawing.Size(82, 24);
            this.btnRele2.TabIndex = 1;
            this.btnRele2.Text = "Ligar Relê 2";
            this.btnRele2.UseVisualStyleBackColor = true;
            this.btnRele2.Click += new System.EventHandler(this.btnRele_Click);
            // 
            // btnRele3
            // 
            this.btnRele3.Location = new System.Drawing.Point(187, 66);
            this.btnRele3.Name = "btnRele3";
            this.btnRele3.Size = new System.Drawing.Size(82, 24);
            this.btnRele3.TabIndex = 4;
            this.btnRele3.Text = "Ligar Relê 3";
            this.btnRele3.UseVisualStyleBackColor = true;
            this.btnRele3.Click += new System.EventHandler(this.btnRele_Click);
            // 
            // btnRele1
            // 
            this.btnRele1.Location = new System.Drawing.Point(11, 66);
            this.btnRele1.Name = "btnRele1";
            this.btnRele1.Size = new System.Drawing.Size(82, 24);
            this.btnRele1.TabIndex = 2;
            this.btnRele1.Text = "Ligar Relê 1";
            this.btnRele1.UseVisualStyleBackColor = true;
            this.btnRele1.Click += new System.EventHandler(this.btnRele_Click);
            // 
            // btnRele4
            // 
            this.btnRele4.Location = new System.Drawing.Point(273, 66);
            this.btnRele4.Name = "btnRele4";
            this.btnRele4.Size = new System.Drawing.Size(82, 24);
            this.btnRele4.TabIndex = 3;
            this.btnRele4.Text = "Ligar Relê 4";
            this.btnRele4.UseVisualStyleBackColor = true;
            this.btnRele4.Click += new System.EventHandler(this.btnRele_Click);
            // 
            // tbConfig
            // 
            this.tbConfig.Controls.Add(this.btnAgora);
            this.tbConfig.Controls.Add(this.dateTimePicker1);
            this.tbConfig.Controls.Add(this.btnDataHora);
            this.tbConfig.Location = new System.Drawing.Point(4, 32);
            this.tbConfig.Name = "tbConfig";
            this.tbConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tbConfig.Size = new System.Drawing.Size(377, 200);
            this.tbConfig.TabIndex = 1;
            this.tbConfig.Text = "Configurações";
            this.tbConfig.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "HH:mm dd/MM/yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(168, 10);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(124, 20);
            this.dateTimePicker1.TabIndex = 7;
            // 
            // btnDataHora
            // 
            this.btnDataHora.Location = new System.Drawing.Point(8, 8);
            this.btnDataHora.Name = "btnDataHora";
            this.btnDataHora.Size = new System.Drawing.Size(156, 24);
            this.btnDataHora.TabIndex = 6;
            this.btnDataHora.Text = "Ajustar Data / Hora";
            this.btnDataHora.UseVisualStyleBackColor = true;
            this.btnDataHora.Click += new System.EventHandler(this.btnDataHora_Click);
            // 
            // tbUsers
            // 
            this.tbUsers.Controls.Add(this.button12);
            this.tbUsers.Controls.Add(this.button7);
            this.tbUsers.Controls.Add(this.groupBox2);
            this.tbUsers.Controls.Add(this.button11);
            this.tbUsers.Location = new System.Drawing.Point(4, 32);
            this.tbUsers.Name = "tbUsers";
            this.tbUsers.Size = new System.Drawing.Size(377, 200);
            this.tbUsers.TabIndex = 2;
            this.tbUsers.Text = "Usuários";
            this.tbUsers.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button12.Location = new System.Drawing.Point(261, 120);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(107, 23);
            this.button12.TabIndex = 10;
            this.button12.Text = "Cartão Remoto";
            this.button12.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button7.Location = new System.Drawing.Point(148, 120);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(107, 23);
            this.button7.TabIndex = 9;
            this.button7.Text = "Biomeria Remota";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button13);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.button10);
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Location = new System.Drawing.Point(8, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 106);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Incluir / Ler / Alterar / Excluir";
            // 
            // button13
            // 
            this.button13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button13.Location = new System.Drawing.Point(276, 71);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(75, 23);
            this.button13.TabIndex = 7;
            this.button13.Text = "Alterar";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(18, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "ID";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(169, 55);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Matricula";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(78, 30);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(66, 23);
            this.button10.TabIndex = 0;
            this.button10.Text = "Ler";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button9.Location = new System.Drawing.Point(276, 13);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 0;
            this.button9.Text = "Excluir";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Location = new System.Drawing.Point(276, 42);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 0;
            this.button8.Text = "Incluir";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.Location = new System.Drawing.Point(172, 71);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(101, 20);
            this.textBox4.TabIndex = 5;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(6, 32);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(66, 20);
            this.textBox5.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 55);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Nome";
            // 
            // textBox6
            // 
            this.textBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox6.Location = new System.Drawing.Point(6, 71);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(160, 20);
            this.textBox6.TabIndex = 3;
            // 
            // button11
            // 
            this.button11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button11.Location = new System.Drawing.Point(8, 120);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 0;
            this.button11.Text = "Listar";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // tbGroups
            // 
            this.tbGroups.Location = new System.Drawing.Point(4, 32);
            this.tbGroups.Name = "tbGroups";
            this.tbGroups.Size = new System.Drawing.Size(377, 200);
            this.tbGroups.TabIndex = 3;
            this.tbGroups.Text = "Grupos";
            this.tbGroups.UseVisualStyleBackColor = true;
            // 
            // txtOut
            // 
            this.txtOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOut.Location = new System.Drawing.Point(12, 258);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ReadOnly = true;
            this.txtOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOut.Size = new System.Drawing.Size(360, 117);
            this.txtOut.TabIndex = 1;
            this.txtOut.WordWrap = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 239);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Informações (log)";
            // 
            // btnAgora
            // 
            this.btnAgora.Location = new System.Drawing.Point(298, 8);
            this.btnAgora.Name = "btnAgora";
            this.btnAgora.Size = new System.Drawing.Size(54, 24);
            this.btnAgora.TabIndex = 8;
            this.btnAgora.Text = "Agora";
            this.btnAgora.UseVisualStyleBackColor = true;
            this.btnAgora.Click += new System.EventHandler(this.btnAgora_Click);
            // 
            // frmExemplos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 387);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.tbc);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "frmExemplos";
            this.Text = "Exemplos de uso API";
            this.Load += new System.EventHandler(this.frmExemplos_Load);
            this.tbc.ResumeLayout(false);
            this.tbDevice.ResumeLayout(false);
            this.tbDevice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmPort)).EndInit();
            this.tbAcoes.ResumeLayout(false);
            this.tbAcoes.PerformLayout();
            this.tbConfig.ResumeLayout(false);
            this.tbUsers.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tbc;
        private System.Windows.Forms.TabPage tbDevice;
        private System.Windows.Forms.TabPage tbConfig;
        private System.Windows.Forms.TabPage tbUsers;
        private System.Windows.Forms.TabPage tbGroups;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSSL;
        private System.Windows.Forms.NumericUpDown nmPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtOut;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tbAcoes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnReboot;
        private System.Windows.Forms.ComboBox cmbGiro;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Button btnGiro;
        private System.Windows.Forms.Button btnRele2;
        private System.Windows.Forms.Button btnRele3;
        private System.Windows.Forms.Button btnRele1;
        private System.Windows.Forms.Button btnRele4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnDataHora;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button btnAgora;
    }
}

