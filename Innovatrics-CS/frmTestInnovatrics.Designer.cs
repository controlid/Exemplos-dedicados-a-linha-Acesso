namespace TestInnovatrics
{
    partial class frmTestInnovatrics
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtOut = new System.Windows.Forms.TextBox();
            this.picDedo = new System.Windows.Forms.PictureBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tr1 = new System.Windows.Forms.TrackBar();
            this.tr2 = new System.Windows.Forms.TrackBar();
            this.picHist = new System.Windows.Forms.PictureBox();
            this.lst = new System.Windows.Forms.ListBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnDirectory = new System.Windows.Forms.Button();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkBitmap = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picDedo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tr1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tr2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHist)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(406, 47);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(125, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Adicionar";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtOut
            // 
            this.txtOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOut.Location = new System.Drawing.Point(12, 338);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ReadOnly = true;
            this.txtOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOut.Size = new System.Drawing.Size(600, 167);
            this.txtOut.TabIndex = 1;
            // 
            // picDedo
            // 
            this.picDedo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picDedo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picDedo.Location = new System.Drawing.Point(12, 12);
            this.picDedo.Name = "picDedo";
            this.picDedo.Size = new System.Drawing.Size(260, 320);
            this.picDedo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDedo.TabIndex = 2;
            this.picDedo.TabStop = false;
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(278, 21);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(86, 49);
            this.btnCapture.TabIndex = 3;
            this.btnCapture.Text = "Capturar Futronic";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(537, 21);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 49);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Buscar";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tr1
            // 
            this.tr1.LargeChange = 25;
            this.tr1.Location = new System.Drawing.Point(278, 160);
            this.tr1.Maximum = 255;
            this.tr1.Name = "tr1";
            this.tr1.Size = new System.Drawing.Size(150, 45);
            this.tr1.TabIndex = 9;
            this.tr1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tr1.Value = 50;
            this.tr1.Scroll += new System.EventHandler(this.tr_Scroll);
            // 
            // tr2
            // 
            this.tr2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tr2.LargeChange = 25;
            this.tr2.Location = new System.Drawing.Point(462, 160);
            this.tr2.Maximum = 255;
            this.tr2.Name = "tr2";
            this.tr2.Size = new System.Drawing.Size(150, 45);
            this.tr2.TabIndex = 10;
            this.tr2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tr2.Value = 200;
            this.tr2.Scroll += new System.EventHandler(this.tr_Scroll);
            // 
            // picHist
            // 
            this.picHist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picHist.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picHist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picHist.Location = new System.Drawing.Point(278, 76);
            this.picHist.Name = "picHist";
            this.picHist.Size = new System.Drawing.Size(334, 78);
            this.picHist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picHist.TabIndex = 11;
            this.picHist.TabStop = false;
            // 
            // lst
            // 
            this.lst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lst.FormattingEnabled = true;
            this.lst.Location = new System.Drawing.Point(278, 237);
            this.lst.Name = "lst";
            this.lst.Size = new System.Drawing.Size(334, 95);
            this.lst.TabIndex = 12;
            this.lst.SelectedIndexChanged += new System.EventHandler(this.lst_SelectedIndexChanged);
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(278, 211);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(303, 20);
            this.txtPath.TabIndex = 13;
            this.txtPath.Text = "c:\\ProgramData\\Control iD\\iDSecure\\bio";
            // 
            // btnDirectory
            // 
            this.btnDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDirectory.Location = new System.Drawing.Point(587, 211);
            this.btnDirectory.Name = "btnDirectory";
            this.btnDirectory.Size = new System.Drawing.Size(25, 20);
            this.btnDirectory.TabIndex = 14;
            this.btnDirectory.Text = "...";
            this.btnDirectory.UseVisualStyleBackColor = true;
            this.btnDirectory.Click += new System.EventHandler(this.btnDirectory_Click);
            // 
            // txtNome
            // 
            this.txtNome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNome.Location = new System.Drawing.Point(406, 21);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(125, 20);
            this.txtNome.TabIndex = 15;
            this.txtNome.TextChanged += new System.EventHandler(this.txtNome_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(403, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Nome a adicionar";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(275, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Local de busca das imagens";
            // 
            // chkBitmap
            // 
            this.chkBitmap.AutoSize = true;
            this.chkBitmap.Checked = true;
            this.chkBitmap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBitmap.Location = new System.Drawing.Point(532, 1);
            this.chkBitmap.Name = "chkBitmap";
            this.chkBitmap.Size = new System.Drawing.Size(80, 17);
            this.chkBitmap.TabIndex = 18;
            this.chkBitmap.Text = "Use Bitmap";
            this.chkBitmap.UseVisualStyleBackColor = true;
            this.chkBitmap.CheckedChanged += new System.EventHandler(this.chkBitmap_CheckedChanged);
            // 
            // frmTestInnovatrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 517);
            this.Controls.Add(this.chkBitmap);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.btnDirectory);
            this.Controls.Add(this.lst);
            this.Controls.Add(this.picHist);
            this.Controls.Add(this.tr2);
            this.Controls.Add(this.tr1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.picDedo);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.btnAdd);
            this.MinimumSize = new System.Drawing.Size(580, 380);
            this.Name = "frmTestInnovatrics";
            this.Text = "Exemplo Teste Innovatrics";
            this.Load += new System.EventHandler(this.frmTestInnovatrics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picDedo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tr1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tr2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHist)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtOut;
        private System.Windows.Forms.PictureBox picDedo;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TrackBar tr1;
        private System.Windows.Forms.TrackBar tr2;
        private System.Windows.Forms.PictureBox picHist;
        private System.Windows.Forms.ListBox lst;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnDirectory;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkBitmap;
    }
}