namespace yasminklinik1fix
{
    partial class pilihdokter
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSpesialisasi = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDokter = new System.Windows.Forms.ComboBox();
            this.dgvJadwalDokter = new System.Windows.Forms.DataGridView();
            this.btnSimpan = new System.Windows.Forms.Button();
            this.btnLihatReservasi = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpTanggalReservasi = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJadwalDokter)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Spesialisasi";
            // 
            // cmbSpesialisasi
            // 
            this.cmbSpesialisasi.FormattingEnabled = true;
            this.cmbSpesialisasi.Location = new System.Drawing.Point(223, 50);
            this.cmbSpesialisasi.Name = "cmbSpesialisasi";
            this.cmbSpesialisasi.Size = new System.Drawing.Size(400, 24);
            this.cmbSpesialisasi.TabIndex = 1;
            this.cmbSpesialisasi.SelectedIndexChanged += new System.EventHandler(this.cmbSpesialisasi_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(130, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Dokter";
            // 
            // cmbDokter
            // 
            this.cmbDokter.FormattingEnabled = true;
            this.cmbDokter.Location = new System.Drawing.Point(223, 100);
            this.cmbDokter.Name = "cmbDokter";
            this.cmbDokter.Size = new System.Drawing.Size(400, 24);
            this.cmbDokter.TabIndex = 3;
            this.cmbDokter.SelectedIndexChanged += new System.EventHandler(this.cmbDokter_SelectedIndexChanged);
            // 
            // dgvJadwalDokter
            // 
            this.dgvJadwalDokter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJadwalDokter.Location = new System.Drawing.Point(109, 184);
            this.dgvJadwalDokter.Name = "dgvJadwalDokter";
            this.dgvJadwalDokter.RowHeadersWidth = 51;
            this.dgvJadwalDokter.RowTemplate.Height = 24;
            this.dgvJadwalDokter.Size = new System.Drawing.Size(541, 205);
            this.dgvJadwalDokter.TabIndex = 4;
            this.dgvJadwalDokter.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJadwalDokter_CellClick);
            // 
            // btnSimpan
            // 
            this.btnSimpan.Location = new System.Drawing.Point(426, 404);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 34);
            this.btnSimpan.TabIndex = 5;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.UseVisualStyleBackColor = true;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // btnLihatReservasi
            // 
            this.btnLihatReservasi.Location = new System.Drawing.Point(524, 404);
            this.btnLihatReservasi.Name = "btnLihatReservasi";
            this.btnLihatReservasi.Size = new System.Drawing.Size(126, 32);
            this.btnLihatReservasi.TabIndex = 7;
            this.btnLihatReservasi.Text = "Lihat Reservasi";
            this.btnLihatReservasi.UseVisualStyleBackColor = true;
            this.btnLihatReservasi.Click += new System.EventHandler(this.btnLihatReservasi_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Tanggal Reservasi";
            // 
            // dtpTanggalReservasi
            // 
            this.dtpTanggalReservasi.Location = new System.Drawing.Point(223, 150);
            this.dtpTanggalReservasi.Name = "dtpTanggalReservasi";
            this.dtpTanggalReservasi.Size = new System.Drawing.Size(400, 22);
            this.dtpTanggalReservasi.TabIndex = 9;
            // 
            // pilihdokter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dtpTanggalReservasi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnLihatReservasi);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.dgvJadwalDokter);
            this.Controls.Add(this.cmbDokter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbSpesialisasi);
            this.Controls.Add(this.label1);
            this.Name = "pilihdokter";
            this.Text = "PilihDokter";
            ((System.ComponentModel.ISupportInitialize)(this.dgvJadwalDokter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSpesialisasi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbDokter;
        private System.Windows.Forms.DataGridView dgvJadwalDokter;
        private System.Windows.Forms.Button btnSimpan;
        private System.Windows.Forms.Button btnLihatReservasi;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpTanggalReservasi;
    }
}