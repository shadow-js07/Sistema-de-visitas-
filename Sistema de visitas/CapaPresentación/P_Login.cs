using Capa_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentación
{
    public partial class P_Login : Form
    {
        private P_admin form2Instance;
        private General form3Instance;
        private P_RegistroVisitantes form4Instance;
        N_Visitas objNegocio = new N_Visitas();
        public P_Login()
        {
            InitializeComponent();
            form2Instance = new P_admin();
            form3Instance = new General();
            form4Instance = new P_RegistroVisitantes();

        }
        private void Iniciar_Click(object sender, EventArgs e)
        {
            string Usuario = usuario.Text;
            string Contraseña = contraseña.Text;

            var resultado = objNegocio.N_AutenticarUsuarios(Usuario, Contraseña);

            if (resultado.Autenticado)
            {
                if (resultado.Tipo_Usuario == "Administrador")
                {
                    form2Instance.Show();
                    MessageBox.Show("Bienvenido/a, Administrador/a!");
                }
                else if (resultado.Tipo_Usuario == "General")
                {
                    form3Instance.SetCredenciales(Usuario, Contraseña);
                    form3Instance.Show();
                    DataTable visitas = objNegocio.N_ObtenerVisitaEncargado(Usuario, Contraseña);
                    form3Instance.SetDataGridViewData(visitas);

                    MessageBox.Show("Bienvenido/a, Usuario/a general!");
                }
                usuario.Text = "";
                contraseña.Text = "";
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            form4Instance.Show();
        }

        private void P_Login_Load(object sender, EventArgs e)
        {

        }
    }
}
