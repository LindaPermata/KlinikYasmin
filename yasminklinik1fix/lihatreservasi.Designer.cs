namespace yasminklinik1fix
{
    partial class lihatreservasi
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
            this.dgvReservasiMendatang = new System.Windows.Forms.DataGridView();
            this.dgvReservasiLalu = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservasiMendatang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservasiLalu)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvReservasiMendatang
            // 
            this.dgvReservasiMendatang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReservasiMendatang.Location = new System.Drawing.Point(41, 59);
            this.dgvReservasiMendatang.Name = "dgvReservasiMendatang";
            this.dgvReservasiMendatang.RowHeadersWidth = 51;
            this.dgvReservasiMendatang.RowTemplate.Height = 24;
            this.dgvReservasiMendatang.Size = new System.Drawing.Size(873, 184);
            this.dgvReservasiMendatang.TabIndex = 0;
            // 
            // dgvReservasiLalu
            // 
            this.dgvReservasiLalu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReservasiLalu.Location = new System.Drawing.Point(41, 354);
            this.dgvReservasiLalu.Name = "dgvReservasiLalu";
            this.dgvReservasiLalu.RowHeadersWidth = 51;
            this.dgvReservasiLalu.RowTemplate.Height = 24;
            this.dgvReservasiLalu.Size = new System.Drawing.Size(873, 180);
            this.dgvReservasiLalu.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(405, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "RESERVASI MENDATANG";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(420, 316);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "HISTORY RESERVASI";
            // 
            // lihatreservasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 555);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvReservasiLalu);
            this.Controls.Add(this.dgvReservasiMendatang);
            this.Name = "lihatreservasi";
            this.Text = "lihatreservasi";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservasiMendatang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservasiLalu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReservasiMendatang;
        private System.Windows.Forms.DataGridView dgvReservasiLalu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}