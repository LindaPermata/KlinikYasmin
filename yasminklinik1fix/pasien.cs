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
    public partial class pasien : Form
    {
        private readonly string connectionString = "Data Source=LAPTOP-0G05EAT6\\LINDAPERMATA;" +
             "Initial Catalog=klinikyasmin1;Integrated Security=True";
        public pasien()
        {
            InitializeComponent();
            dgvPasien.CellClick += new DataGridViewCellEventHandler(dgvPasien_CellClick);



        }

        private void FormPasien_Load(object sender, EventArgs e)
        {
            cmbJenisKelamin.Items.Add("Laki-laki");
            cmbJenisKelamin.Items.Add("Perempuan");

            cmbJenisKelamin.SelectedIndex = 0;

            txtIdPasien.Text = GenerateIdPasien();
            txtIdPasien.Visible = false;
            txtIdPasien.TabStop = false;

            LoadDataPasien();
        }

        private string GenerateIdPasien()
        {
            string newId = "PS001"; // Default ID format

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 Id_Pasien FROM Pasien ORDER BY Id_Pasien DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                var result = cmd.ExecuteScalar();

                if (result != null && !string.IsNullOrEmpty(result.ToString()))
                {
                    string lastId = result.ToString(); // Assuming format like PS001, PS002
                    int number = int.Parse(lastId.Substring(2)) + 1;
                    newId = "PS" + number.ToString("D3"); // Format to 3 digits
                }
                else
                {
                    newId = "PS001"; // If no records exist, start with PS001
                }
            }

            return newId;
        }

        private void LoadDataPasien()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Id_Pasien, Nama, TanggalLahir, JenisKelamin, NoTelepon, Email, Password FROM Pasien ORDER BY Nama";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dgvPasien.DataSource = dataTable;

                    dgvPasien.Columns["Id_Pasien"].HeaderText = "ID Pasien";
                    dgvPasien.Columns["Nama"].HeaderText = "Nama Lengkap";
                    dgvPasien.Columns["TanggalLahir"].HeaderText = "Tanggal Lahir";
                    dgvPasien.Columns["JenisKelamin"].HeaderText = "Jenis Kelamin";
                    dgvPasien.Columns["NoTelepon"].HeaderText = "Nomor Telepon";
                    dgvPasien.Columns["Email"].HeaderText = "Email";

                    dgvPasien.AutoResizeColumns();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }


        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text))
            {
                MessageBox.Show("Nama pasien harus diisi!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNoTelepon.Text))
            {
                MessageBox.Show("Nomor telepon harus diisi!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Email tidak valid!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password harus diisi!");
                return;
            }

            string password = txtPassword.Text;

            // Validasi password minimal 6 karakter dan mengandung huruf dan angka
            if (password.Length < 6 ||
                !password.Any(char.IsLetter) ||
                !password.Any(char.IsDigit))
            {
                MessageBox.Show("Password harus minimal 6 karakter dan mengandung huruf serta angka!");
                return;
            }

            string idPasien = txtIdPasien.Text;
            string nama = txtNama.Text;
            DateTime tanggalLahir = dtpTanggalLahir.Value;
            string jenisKelamin = cmbJenisKelamin.Text;
            string noTelepon = txtNoTelepon.Text;
            string email = txtEmail.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Pasien (Id_Pasien, Nama, TanggalLahir, JenisKelamin, NoTelepon, Email, Password) " +
                                   "VALUES (@Id_Pasien, @Nama, @TanggalLahir, @JenisKelamin, @NoTelepon, @Email, @Password)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id_Pasien", idPasien);
                    cmd.Parameters.AddWithValue("@Nama", nama);
                    cmd.Parameters.AddWithValue("@TanggalLahir", tanggalLahir);
                    cmd.Parameters.AddWithValue("@JenisKelamin", jenisKelamin);
                    cmd.Parameters.AddWithValue("@NoTelepon", noTelepon);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data pasien berhasil disimpan!");

                    LoadDataPasien();

                    // Reset form
                    txtNama.Clear();
                    txtNoTelepon.Clear();
                    txtEmail.Clear();
                    txtPassword.Clear();
                    cmbJenisKelamin.SelectedIndex = 0;
                    dtpTanggalLahir.Value = DateTime.Now;
                    txtIdPasien.Text = GenerateIdPasien();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menyimpan data: " + ex.Message);
                }
            }
        }


        private void btnHapus_Click(object sender, EventArgs e)
        {
            string idPasien = txtIdPasien.Text;

            if (string.IsNullOrEmpty(idPasien))
            {
                MessageBox.Show("ID Pasien tidak valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus data pasien ini?",
                                                  "Konfirmasi Hapus",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Pasien WHERE Id_Pasien = @Id_Pasien";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Id_Pasien", idPasien);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data pasien berhasil dihapus!");

                            LoadDataPasien(); // Refresh DataGridView

                            // Reset form
                            txtNama.Clear();
                            txtNoTelepon.Clear();
                            txtEmail.Clear();
                            cmbJenisKelamin.SelectedIndex = 0;
                            dtpTanggalLahir.Value = DateTime.Now;
                            txtIdPasien.Text = GenerateIdPasien();
                            txtPassword.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Data pasien tidak ditemukan atau sudah dihapus.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menghapus data: " + ex.Message);
                    }
                }
            }
        }

        private void dgvPasien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pastikan baris yang diklik valid
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPasien.Rows[e.RowIndex];

                txtIdPasien.Text = row.Cells["Id_Pasien"].Value.ToString();
                txtNama.Text = row.Cells["Nama"].Value.ToString();
                dtpTanggalLahir.Value = Convert.ToDateTime(row.Cells["TanggalLahir"].Value);
                cmbJenisKelamin.Text = row.Cells["JenisKelamin"].Value.ToString();
                txtNoTelepon.Text = row.Cells["NoTelepon"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();

            }
        }

        

        private void btnLogin(object sender, EventArgs e)
        {
            Login formLogin = new Login();
            formLogin.Show();
            this.Hide();
        }

        
    }
}
