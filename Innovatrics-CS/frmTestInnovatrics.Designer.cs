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
            this.lnkLic = new System.Windows.Forms.LinkLabel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btRaw = new System.Windows.Forms.Button();
            this.tr1 = new System.Windows.Forms.TrackBar();
            this.tr2 = new System.Windows.Forms.TrackBar();
            this.picHist = new System.Windows.Forms.PictureBox();
            this.btINC = new System.Windows.Forms.Button();
            this.lst = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.picDedo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tr1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tr2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHist)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(396, 168);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
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
            this.txtOut.Location = new System.Drawing.Point(278, 197);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ReadOnly = true;
            this.txtOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOut.Size = new System.Drawing.Size(274, 135);
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
            this.btnCapture.Location = new System.Drawing.Point(278, 168);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(103, 23);
            this.btnCapture.TabIndex = 3;
            this.btnCapture.Text = "Capturar Futronic";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // lnkLic
            // 
            this.lnkLic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkLic.Location = new System.Drawing.Point(275, 12);
            this.lnkLic.Name = "lnkLic";
            this.lnkLic.Size = new System.Drawing.Size(277, 83);
            this.lnkLic.TabIndex = 5;
            this.lnkLic.TabStop = true;
            this.lnkLic.Text = "Para que esse exemplo funcione será necessário ter uma licença da innovatrics";
            this.lnkLic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkLic.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLic_LinkClicked);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(477, 168);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Buscar";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btRaw
            // 
            this.btRaw.Location = new System.Drawing.Point(278, 139);
            this.btRaw.Name = "btRaw";
            this.btRaw.Size = new System.Drawing.Size(51, 23);
            this.btRaw.TabIndex = 7;
            this.btRaw.Text = "Raw";
            this.btRaw.UseVisualStyleBackColor = true;
            this.btRaw.Click += new System.EventHandler(this.btRaw_Click);
            // 
            // tr1
            // 
            this.tr1.LargeChange = 25;
            this.tr1.Location = new System.Drawing.Point(396, 76);
            this.tr1.Maximum = 255;
            this.tr1.Name = "tr1";
            this.tr1.Size = new System.Drawing.Size(156, 45);
            this.tr1.TabIndex = 9;
            this.tr1.Value = 50;
            this.tr1.Scroll += new System.EventHandler(this.tr_Scroll);
            // 
            // tr2
            // 
            this.tr2.LargeChange = 25;
            this.tr2.Location = new System.Drawing.Point(396, 117);
            this.tr2.Maximum = 255;
            this.tr2.Name = "tr2";
            this.tr2.Size = new System.Drawing.Size(156, 45);
            this.tr2.TabIndex = 10;
            this.tr2.Value = 200;
            this.tr2.Scroll += new System.EventHandler(this.tr_Scroll);
            // 
            // picHist
            // 
            this.picHist.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picHist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picHist.Location = new System.Drawing.Point(12, 338);
            this.picHist.Name = "picHist";
            this.picHist.Size = new System.Drawing.Size(260, 78);
            this.picHist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picHist.TabIndex = 11;
            this.picHist.TabStop = false;
            // 
            // btINC
            // 
            this.btINC.Location = new System.Drawing.Point(335, 139);
            this.btINC.Name = "btINC";
            this.btINC.Size = new System.Drawing.Size(29, 23);
            this.btINC.TabIndex = 8;
            this.btINC.Text = "+";
            this.btINC.UseVisualStyleBackColor = true;
            this.btINC.Click += new System.EventHandler(this.btINC_Click);
            // 
            // lst
            // 
            this.lst.FormattingEnabled = true;
            this.lst.Location = new System.Drawing.Point(278, 347);
            this.lst.Name = "lst";
            this.lst.Size = new System.Drawing.Size(274, 69);
            this.lst.TabIndex = 12;
            this.lst.SelectedIndexChanged += new System.EventHandler(this.lst_SelectedIndexChanged);
            // 
            // frmTestInnovatrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 428);
            this.Controls.Add(this.lst);
            this.Controls.Add(this.picHist);
            this.Controls.Add(this.tr2);
            this.Controls.Add(this.tr1);
            this.Controls.Add(this.btINC);
            this.Controls.Add(this.btRaw);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lnkLic);
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
        private System.Windows.Forms.LinkLabel lnkLic;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btRaw;
        private System.Windows.Forms.TrackBar tr1;
        private System.Windows.Forms.TrackBar tr2;
        private System.Windows.Forms.PictureBox picHist;
        private System.Windows.Forms.Button btINC;
        private System.Windows.Forms.ListBox lst;
    }
}