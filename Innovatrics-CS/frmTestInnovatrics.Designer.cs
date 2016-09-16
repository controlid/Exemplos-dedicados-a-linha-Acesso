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
            ((System.ComponentModel.ISupportInitialize)(this.picDedo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(198, 88);
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
            this.txtOut.Location = new System.Drawing.Point(12, 117);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ReadOnly = true;
            this.txtOut.Size = new System.Drawing.Size(342, 232);
            this.txtOut.TabIndex = 1;
            // 
            // picDedo
            // 
            this.picDedo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picDedo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picDedo.Location = new System.Drawing.Point(12, 12);
            this.picDedo.Name = "picDedo";
            this.picDedo.Size = new System.Drawing.Size(68, 99);
            this.picDedo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDedo.TabIndex = 2;
            this.picDedo.TabStop = false;
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(89, 88);
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
            this.lnkLic.Location = new System.Drawing.Point(86, 12);
            this.lnkLic.Name = "lnkLic";
            this.lnkLic.Size = new System.Drawing.Size(268, 73);
            this.lnkLic.TabIndex = 5;
            this.lnkLic.TabStop = true;
            this.lnkLic.Text = "Para que esse exemplo funcione será necessário ter uma licença da innovatrics";
            this.lnkLic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkLic.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLic_LinkClicked);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(279, 88);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Buscar";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // frmTestInnovatrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 361);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lnkLic);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.picDedo);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.btnAdd);
            this.MinimumSize = new System.Drawing.Size(380, 400);
            this.Name = "frmTestInnovatrics";
            this.Text = "Exemplo Teste Innovatrics";
            this.Load += new System.EventHandler(this.frmTestInnovatrics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picDedo)).EndInit();
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
    }
}