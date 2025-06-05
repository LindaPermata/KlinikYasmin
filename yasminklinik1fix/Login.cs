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
    public partial class Login : Form
    {
        private string idPasienLogin;
        public Login()
        {
            InitializeComponent();
            // Matikan tombol 'Pilih Dokter' saat belum login
            btnPilihDokter.Enabled = false;
        }
    
        private void btnLogin_click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Email dan Password wajib diisi.");
                return;
            }

            string connectionString = "Data Source=LAPTOP-0G05EAT6\\LINDAPERMATA;" +
                                      "Initial Catalog=klinikyasmin1;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT Id_Pasien FROM Pasien WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            idPasienLogin = result.ToString();
                            MessageBox.Show("Login berhasil!");
                            btnPilihDokter.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Email atau Password salah.");
                            btnPilihDokter.Enabled = false;
                            idPasienLogin = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                }
            }
        }

        private void btnPilihDokter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(idPasienLogin))
            {
                pilihdokter formDokter = new pilihdokter(idPasienLogin);
                formDokter.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("ID pasien belum tersedia. Silakan login terlebih dahulu.");
            }
        }
    }
    
}
