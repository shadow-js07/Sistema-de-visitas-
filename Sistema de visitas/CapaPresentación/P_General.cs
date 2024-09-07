using Capa_Negocio;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CapaPresentación
{
    public partial class General : Form
    {
        N_Visitas objNegocio = new N_Visitas();
        P_RegistroVisitantes FormInstance;
        private string Usuario;
        private string Contraseña;

        public General()
        {
            InitializeComponent();
        }

        private void P_General_Load(object sender, EventArgs e)
        {
            FormInstance = new P_RegistroVisitantes();
            ActualizarDataGridView();
        }

        public void SetDataGridViewData(DataTable data)
        {
            dataGridView1.DataSource = data;
        }

        public void SetCredenciales(string usuario, string contraseña)
        {
            this.Usuario = usuario;
            this.Contraseña = contraseña;
        }

        public void ActualizarDataGridView()
        {
            DataTable dv = objNegocio.N_ObtenerVisitaEncargado(Usuario, Contraseña);
            SetDataGridViewData(dv);
        }

        private void Listar_Click(object sender, EventArgs e)
        {
            ActualizarDataGridView();
        }

        private void registrar_Click(object sender, EventArgs e)
        {
            FormInstance.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
