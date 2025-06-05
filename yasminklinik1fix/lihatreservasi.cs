using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yasminklinik1fix
{
    public partial class lihatreservasi : Form
    {
        private string connectionString = "Data Source=LAPTOP-0G05EAT6\\LINDAPERMATA;" +
                                         "Initial Catalog=klinikyasmin1;Integrated Security=True";
        public lihatreservasi()
        {
            InitializeComponent();
            this.Load += new EventHandler(LihatReservasiForm_Load);

        }

        private void LihatReservasiForm_Load(object sender, EventArgs e)
        {
            LoadReservasiData();
        }




        private void LoadReservasiData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_LihatDataReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    try
                    {
                        conn.Open();
                        adapter.Fill(dt);

                        // Pisahkan data menjadi dua view
                        DataView viewMendatang = new DataView(dt);
                        viewMendatang.RowFilter = $"TanggalReservasi >= #{DateTime.Now.Date:MM/dd/yyyy}#";

                        DataView viewLalu = new DataView(dt);
                        viewLalu.RowFilter = $"TanggalReservasi < #{DateTime.Now.Date:MM/dd/yyyy}#";

                        // Set masing-masing DataGridView
                        dgvReservasiMendatang.AutoGenerateColumns = true;
                        dgvReservasiMendatang.DataSource = viewMendatang;

                        dgvReservasiLalu.AutoGenerateColumns = true;
                        dgvReservasiLalu.DataSource = viewLalu;

                        // Atur header kolom
                        AturHeaderGrid(dgvReservasiMendatang);
                        AturHeaderGrid(dgvReservasiLalu);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saat mengambil data:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void AturHeaderGrid(DataGridView dgv)
        {
            if (dgv.Columns.Contains("Id_Reservasi")) dgv.Columns["Id_Reservasi"].HeaderText = "ID Reservasi";
            if (dgv.Columns.Contains("Nama_Pasien")) dgv.Columns["Nama_Pasien"].HeaderText = "Nama Pasien";
            if (dgv.Columns.Contains("Nama_Dokter")) dgv.Columns["Nama_Dokter"].HeaderText = "Nama Dokter";
            if (dgv.Columns.Contains("Spesialisasi")) dgv.Columns["Spesialisasi"].HeaderText = "Spesialisasi";
            if (dgv.Columns.Contains("TanggalReservasi")) dgv.Columns["TanggalReservasi"].HeaderText = "Tanggal Reservasi";
            if (dgv.Columns.Contains("Jam_Konsultasi")) dgv.Columns["Jam_Konsultasi"].HeaderText = "Jam Konsultasi";
            if (dgv.Columns.Contains("Status_Reservasi")) dgv.Columns["Status_Reservasi"].HeaderText = "Status Reservasi";
        }


       
    }
}
