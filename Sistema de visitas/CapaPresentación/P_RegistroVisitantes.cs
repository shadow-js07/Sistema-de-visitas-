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
    public partial class P_RegistroVisitantes : Form
    {

        N_Visitas objNegocio = new N_Visitas();

        private string rutaImagen;

        public P_RegistroVisitantes()
        {
            InitializeComponent();
        }

        private void P_RegistroVisitantes_Load(object sender, EventArgs e)
        {
            List<KeyValuePair<int, string>> edificios = objNegocio.ObtenerEdificios();

            Edificios.DisplayMember = "Value";
            Edificios.ValueMember = "Key";
            Edificios.DataSource = edificios;

            Edificios.SelectedIndexChanged += EdificiosItemSeleccionado;
        }

        private void EdificiosItemSeleccionado(object sender, EventArgs e)
        {
            if (Edificios.SelectedItem != null)
            {
                int edificioId = ((KeyValuePair<int, string>)Edificios.SelectedItem).Key;
                List<KeyValuePair<int, string>> aulas = objNegocio.ObtenerAulasPorEdificio(edificioId);

                Aulas.DisplayMember = "Value";
                Aulas.ValueMember = "Key";
                Aulas.DataSource = aulas;
            }
        }

        private void imagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                rutaImagen = openFileDialog.FileName;
                usuarioImagen.Image = Image.FromFile(rutaImagen);
                usuarioImagen.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private byte[] ConvertirImagen(string rutaArchivo)
        {
            return System.IO.File.ReadAllBytes(rutaArchivo);
        }

        private void Registro_Click(object sender, EventArgs e)
        {
            string Nombre = nombre.Text;
            string Apellido = apellido.Text;
            string Carrera = carrera.Text;
            string Correo = correo.Text;
            int Edificio = ((KeyValuePair<int, string>)Edificios.SelectedItem).Key;
            int Aula = ((KeyValuePair<int, string>)Aulas.SelectedItem).Key;
            DateTime HoraEntrada = horaEntrada.Value;
            DateTime HoraSalida = horaSalida.Value;
            string Motivo = motivo.Text;
            byte[] Foto = string.IsNullOrEmpty(rutaImagen) ? null : ConvertirImagen(rutaImagen);

            objNegocio.N_RegistrarVisitas(Nombre, Apellido, Carrera, Correo, Edificio, Aula, HoraEntrada, HoraSalida, Motivo, Foto);

            MessageBox.Show("Su visita ha sido registrada con éxito");

            nombre.Text = "";
            apellido.Text = "";
            carrera.Text = "";
            correo.Text = "";
            Edificios.Text = "";
            Aulas.Text = "";
            motivo.Text = "";
        }

    }
}
