using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yasminklinik1fix
{
    public partial class pilihdokter : Form
    {

        private readonly string connectionString = "Data Source=LAPTOP-0G05EAT6\\LINDAPERMATA;" +
             "Initial Catalog=klinikyasmin1;Integrated Security=True";

        private SqlConnection conn;
        private string idPasien; //  Menyimpan Id_Pasien dari login/registrasi
        private string selectedJadwalId = "";
        private DataGridViewRow selectedRow = null;

        public pilihdokter()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
            LoadSpesialisasi();
            SetupDataGridView();

            dtpTanggalReservasi.MinDate = DateTime.Today;
            dtpTanggalReservasi.MaxDate = DateTime.Today.AddDays(7);  // maksimal 7 hari ke depan
            dtpTanggalReservasi.Value = DateTime.Today; // default ke hari ini

        }

        public pilihdokter(string idPasien) : this()
        {
            this.idPasien = idPasien;
        }

        private void SetupDataGridView()
        {
            // Setup DataGridView properties
            dgvJadwalDokter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJadwalDokter.MultiSelect = false;
            dgvJadwalDokter.ReadOnly = true;
            dgvJadwalDokter.AllowUserToAddRows = false;
        }
        private void LoadSpesialisasi()
        {
            try
            {
                cmbSpesialisasi.Items.Clear();
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT Spesialisasi FROM Dokter ORDER BY Spesialisasi", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmbSpesialisasi.Items.Add(reader["Spesialisasi"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                MessageBox.Show("Error loading spesialisasi: " + ex.Message);
            }
        }
        private void cmbSpesialisasi_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset semua yang terkait
            cmbDokter.Items.Clear();
            cmbDokter.SelectedIndex = -1;
            dgvJadwalDokter.DataSource = null;
            ResetSelection();

            if (cmbSpesialisasi.SelectedIndex == -1)
                return;

            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Nama FROM Dokter WHERE Spesialisasi = @spesialisasi ORDER BY Nama", conn);
                cmd.Parameters.AddWithValue("@spesialisasi", cmbSpesialisasi.SelectedItem.ToString());
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmbDokter.Items.Add(reader["Nama"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                MessageBox.Show("Error loading dokter: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbDokter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset selection
            dgvJadwalDokter.DataSource = null;
            ResetSelection();

            if (cmbDokter.SelectedIndex == -1)
                return;

            try
            {
                using (SqlConnection connTemp = new SqlConnection(connectionString))
                {
                    connTemp.Open();
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT JD.Id_Jadwal, JD.Hari, JD.JamMulai, JD.JamSelesai
                        FROM JadwalDokter JD
                        JOIN Dokter D ON JD.Id_Dokter = D.Id_Dokter
                        WHERE D.Nama = @nama
                        ORDER BY 
                            CASE JD.Hari 
                                WHEN 'Senin' THEN 1
                                WHEN 'Selasa' THEN 2
                                WHEN 'Rabu' THEN 3
                                WHEN 'Kamis' THEN 4
                                WHEN 'Jumat' THEN 5
                                WHEN 'Sabtu' THEN 6
                                WHEN 'Minggu' THEN 7
                                ELSE 8
                            END,
                            JD.JamMulai", connTemp);
                    cmd.Parameters.AddWithValue("@nama", cmbDokter.SelectedItem.ToString());

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvJadwalDokter.DataSource = dt;

                    // Customize column headers and visibility
                    if (dgvJadwalDokter.Columns.Count > 0)
                    {
                        if (dgvJadwalDokter.Columns["Id_Jadwal"] != null)
                            dgvJadwalDokter.Columns["Id_Jadwal"].Visible = false;
                        if (dgvJadwalDokter.Columns["Hari"] != null)
                            dgvJadwalDokter.Columns["Hari"].HeaderText = "Hari";
                        if (dgvJadwalDokter.Columns["JamMulai"] != null)
                            dgvJadwalDokter.Columns["JamMulai"].HeaderText = "Jam Mulai";
                        if (dgvJadwalDokter.Columns["JamSelesai"] != null)
                            dgvJadwalDokter.Columns["JamSelesai"].HeaderText = "Jam Selesai";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading jadwal dokter: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateReservasiId()
        {
            string newId = "RSV001"; // Default ID format

            try
            {
                using (SqlConnection connTemp = new SqlConnection(connectionString))
                {
                    connTemp.Open();
                    string query = "SELECT TOP 1 Id_Reservasi FROM Reservasi ORDER BY Id_Reservasi DESC";
                    SqlCommand cmd = new SqlCommand(query, connTemp);
                    var result = cmd.ExecuteScalar();

                    if (result != null && !string.IsNullOrEmpty(result.ToString()))
                    {
                        string lastId = result.ToString(); // Format RSV001, RSV002
                        if (lastId.Length >= 6 && lastId.StartsWith("RSV"))
                        {
                            int number = int.Parse(lastId.Substring(3)) + 1;
                            newId = "RSV" + number.ToString("D3"); // Format to 3 digits
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Jika ada error (misalnya tabel kosong), gunakan default
                MessageBox.Show("Warning: Menggunakan ID default. " + ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                newId = "RSV001";
            }

            return newId;
        }
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // Validasi input lengkap
            if (cmbSpesialisasi.SelectedIndex == -1)
            {
                MessageBox.Show("Silakan pilih spesialisasi terlebih dahulu.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbDokter.SelectedIndex == -1)
            {
                MessageBox.Show("Silakan pilih dokter terlebih dahulu.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(selectedJadwalId))
            {
                MessageBox.Show("Silakan pilih jadwal dari tabel dengan mengklik salah satu baris.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(idPasien))
            {
                MessageBox.Show("ID Pasien tidak valid. Silakan login ulang.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validasi tanggal reservasi (hanya boleh dari hari ini sampai 7 hari ke depan)
            DateTime tanggalReservasiDipilih = dtpTanggalReservasi.Value.Date;

            if (tanggalReservasiDipilih < DateTime.Today || tanggalReservasiDipilih > DateTime.Today.AddDays(7))
            {
                MessageBox.Show("Tanggal reservasi hanya bisa dipilih dari hari ini sampai 7 hari ke depan.", "Validasi Tanggal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tampilkan konfirmasi
            string dokterNama = cmbDokter.SelectedItem.ToString();
            string spesialisasi = cmbSpesialisasi.SelectedItem.ToString();
            string hari = selectedRow?.Cells["Hari"].Value?.ToString() ?? "";
            string jamMulai = selectedRow?.Cells["JamMulai"].Value?.ToString() ?? "";
            string jamSelesai = selectedRow?.Cells["JamSelesai"].Value?.ToString() ?? "";

            DialogResult konfirmasi = MessageBox.Show(
                $"Konfirmasi Reservasi:\n\n" +
                $"Spesialisasi: {spesialisasi}\n" +
                $"Dokter: {dokterNama}\n" +
                $"Jadwal: {hari}, {jamMulai} - {jamSelesai}\n" +
                $"Tanggal Reservasi: {tanggalReservasiDipilih.ToShortDateString()}\n\n" +
                $"Apakah Anda yakin ingin membuat reservasi ini?",
                "Konfirmasi Reservasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (konfirmasi != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection connNew = new SqlConnection(connectionString))
                {
                    connNew.Open();

                    // Get dokter ID dari jadwal
                    string getDokterQuery = "SELECT Id_Dokter FROM JadwalDokter WHERE Id_Jadwal = @Id_Jadwal";
                    string idDokter = "";

                    using (SqlCommand getDokterCmd = new SqlCommand(getDokterQuery, connNew))
                    {
                        getDokterCmd.Parameters.AddWithValue("@Id_Jadwal", selectedJadwalId);
                        var dokterResult = getDokterCmd.ExecuteScalar();
                        if (dokterResult != null)
                        {
                            idDokter = dokterResult.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Jadwal tidak ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Cek apakah pasien sudah punya reservasi aktif untuk dokter dan tanggal yang sama
                    string cekQuery = @"SELECT COUNT(*) FROM Reservasi R 
                                WHERE R.Id_Dokter = @Id_Dokter 
                                AND R.Id_Pasien = @Id_Pasien 
                                AND R.Status_Reservasi IN ('Terkonfirmasi')
                                AND R.TanggalReservasi = @TanggalReservasi";

                    using (SqlCommand cekCmd = new SqlCommand(cekQuery, connNew))
                    {
                        cekCmd.Parameters.AddWithValue("@Id_Dokter", idDokter);
                        cekCmd.Parameters.AddWithValue("@Id_Pasien", idPasien);
                        cekCmd.Parameters.AddWithValue("@TanggalReservasi", tanggalReservasiDipilih);

                        int exists = (int)cekCmd.ExecuteScalar();
                        if (exists > 0)
                        {
                            MessageBox.Show("Anda sudah memiliki reservasi aktif untuk dokter ini pada tanggal yang sama.",
                                          "Reservasi Sudah Ada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Ambil jam konsultasi dari jadwal
                    string getJamQuery = "SELECT JamMulai FROM JadwalDokter WHERE Id_Jadwal = @Id_Jadwal";
                    TimeSpan jamKonsultasi;

                    using (SqlCommand getJamCmd = new SqlCommand(getJamQuery, connNew))
                    {
                        getJamCmd.Parameters.AddWithValue("@Id_Jadwal", selectedJadwalId);
                        var jamResult = getJamCmd.ExecuteScalar();
                        if (jamResult != null)
                        {
                            jamKonsultasi = (TimeSpan)jamResult;
                        }
                        else
                        {
                            MessageBox.Show("Jam konsultasi tidak ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Generate ID reservasi baru
                    string idReservasi = GenerateReservasiId();

                    // Insert data reservasi
                    string insertQuery = @"INSERT INTO Reservasi (Id_Reservasi, Id_Pasien, Id_Dokter, TanggalReservasi, Jam_Konsultasi, Status_Reservasi) 
                                   VALUES (@Id_Reservasi, @Id_Pasien, @Id_Dokter, @TanggalReservasi, @Jam_Konsultasi, @Status_Reservasi)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connNew))
                    {
                        cmd.Parameters.AddWithValue("@Id_Reservasi", idReservasi);
                        cmd.Parameters.AddWithValue("@Id_Pasien", idPasien);
                        cmd.Parameters.AddWithValue("@Id_Dokter", idDokter);
                        cmd.Parameters.AddWithValue("@TanggalReservasi", tanggalReservasiDipilih);
                        cmd.Parameters.AddWithValue("@Jam_Konsultasi", jamKonsultasi);
                        cmd.Parameters.AddWithValue("@Status_Reservasi", "Terkonfirmasi");

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show(
                                $"Reservasi berhasil disimpan!\n\n" +
                                $"ID Reservasi: {idReservasi}\n" +
                                $"Dokter: {dokterNama}\n" +
                                $"Spesialisasi: {spesialisasi}\n" +
                                $"Jadwal: {hari}, {jamMulai} - {jamSelesai}\n" +
                                $"Tanggal: {tanggalReservasiDipilih.ToShortDateString()}\n" +
                                $"Status: Terkonfirmasi\n\n" +
                                $"Silakan datang sesuai jadwal yang telah ditentukan.",
                                "Reservasi Berhasil",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            // Reset form setelah berhasil
                            ResetForm();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menyimpan reservasi.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saat menyimpan reservasi: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            ResetSelection();
            cmbSpesialisasi.SelectedIndex = -1;
            cmbDokter.Items.Clear();
            cmbDokter.SelectedIndex = -1;
            dgvJadwalDokter.DataSource = null;
        }

        private void dgvJadwalDokter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvJadwalDokter.Rows.Count)
            {
                // Check if Id_Jadwal column exists and has value
                if (dgvJadwalDokter.Columns.Contains("Id_Jadwal") &&
                    dgvJadwalDokter.Rows[e.RowIndex].Cells["Id_Jadwal"].Value != null)
                {
                    selectedJadwalId = dgvJadwalDokter.Rows[e.RowIndex].Cells["Id_Jadwal"].Value.ToString();
                    selectedRow = dgvJadwalDokter.Rows[e.RowIndex];

                    string hari = dgvJadwalDokter.Rows[e.RowIndex].Cells["Hari"].Value?.ToString() ?? "";
                    string jamMulai = dgvJadwalDokter.Rows[e.RowIndex].Cells["JamMulai"].Value?.ToString() ?? "";
                    string jamSelesai = dgvJadwalDokter.Rows[e.RowIndex].Cells["JamSelesai"].Value?.ToString() ?? "";

                    // Highlight the selected row
                    dgvJadwalDokter.ClearSelection();
                    dgvJadwalDokter.Rows[e.RowIndex].Selected = true;

                    MessageBox.Show($"Jadwal dipilih:\n{hari}, {jamMulai} - {jamSelesai}\n\nKlik tombol Simpan untuk membuat reservasi.",
                                   "Jadwal Terpilih", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Data jadwal tidak valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ResetSelection();
                }
            }

        }

        private void ResetSelection()
        {
            selectedJadwalId = "";
            selectedRow = null;
            if (dgvJadwalDokter.Rows.Count > 0)
                dgvJadwalDokter.ClearSelection();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            base.OnFormClosed(e);
        }

        private void btnLihatReservasi_Click(object sender, EventArgs e)
        {
           lihatreservasi form = new lihatreservasi();
            form.ShowDialog();
            
        }
    }
}






            


    

