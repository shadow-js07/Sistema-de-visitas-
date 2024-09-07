using Capa_Entidad;
using Capa_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentación
{
    public partial class P_admin : Form
    {
        N_Visitas objNegocio = new N_Visitas();
        private string rutaImagen;
        int usuarioId;
        int edificioId;
        int aulaId;
        int visitaId;

        public P_admin()
        {
            InitializeComponent();
        }

        private void P_admin_Load(object sender, EventArgs e)
        {
            ListarUsuarios();
            ListarEdificios();
            ListarAulas();
            ListarVisitas();

            List<KeyValuePair<int, string>> edificios = objNegocio.ObtenerEdificios();

            Edificios.DisplayMember = "Value";
            Edificios.ValueMember = "Key";
            Edificios.DataSource = edificios;

            Edificios.SelectedIndexChanged += EdificiosItemSeleccionado;
        }

        public void ListarUsuarios()
        {
            DataTable du = objNegocio.N_ListarUsuarios();
            dataGridView1.DataSource = du;

            nombre.Text = "";
            apellido.Text = "";
            contraseña.Text = "";
            tipoUsuario.Text = "";
        }
        public void ListarEdificios()
        {
            DataTable de = objNegocio.N_ListarEdificios();
            dataGridView2.DataSource = de;
            dataGridView4.DataSource = de;

            nombreEncargado.Text = "";
            apellidoEncargado.Text = "";
            nombreEdificio.Text = "";
        }
        public void ListarAulas()
        {
            DataTable da = objNegocio.N_ListarAulas();
            dataGridView3.DataSource = da;

            idEdificioAula.Text = "";
            nombreAula.Text = "";
        }
        public void ListarVisitas()
        {
            DataTable dv = objNegocio.N_ListarVisitas();

            if (!dv.Columns.Contains("Imagen"))
            {
                dv.Columns.Add("Imagen", typeof(Image));
            }

            foreach (DataRow row in dv.Rows)
            {
                if (row["Foto"] != DBNull.Value)
                {
                    byte[] imageData = (byte[])row["Foto"];
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image originalImage = Image.FromStream(ms);
                        Image resizedImage = ResizeImage(originalImage, 50, 50);
                        row["Imagen"] = resizedImage;
                    }
                }
            }

            dataGridView5.DataSource = dv;

            visitaNombre.Text = "";
            visitaApellido.Text = "";
            carrera.Text = "";
            correo.Text = "";
            Edificios.Text = "";
            Aulas.Text = "";
            motivo.Text = "";

            if (dataGridView5.Columns["Foto"] != null)
            {
                dataGridView5.Columns["Foto"].Visible = false;
            }

            if (!dataGridView5.Columns.Contains("Imagen"))
            {
                DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
                {
                    Name = "Imagen",
                    HeaderText = "Foto",
                    DataPropertyName = "Imagen",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dataGridView5.Columns.Add(imageColumn);
            }

            dataGridView5.RowTemplate.Height = 50;

            foreach (DataGridViewColumn column in dataGridView5.Columns)
            {
                if (column.Name != "Imagen")
                {
                    column.Width = 100;
                }
            }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }


        private void registroUsuario_Click(object sender, EventArgs e)
        {
            string Nombre = nombre.Text;
            string Apellido = apellido.Text;
            DateTime FechaN = fechaNacimiento.Value;
            string Contraseña = contraseña.Text;
            string Tipo = tipoUsuario.Text;

            objNegocio.N_RegistrarUsuarios(Nombre, Apellido, FechaN, Tipo, Contraseña);
            MessageBox.Show("Los datos del usuario se han ingresado correctamente.\n\nEl usuario se genera automaticamente\ncon la primera letra del nombre y el apellido + 3 digitos,\n puede visualizarlo en la lista de Usuarios registrados.");
            ListarUsuarios();
        }

        private void eliminar_Click(object sender, EventArgs e)
        {
            objNegocio.N_EliminarUsuarios(usuarioId);
            ListarUsuarios();
            MessageBox.Show("Usuario eliminado correctamente.");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                usuarioId = Convert.ToInt32(fila.Cells[0].Value);
                CargarDatosUsuario(usuarioId);
            }
        }

        private void CargarDatosUsuario(int usuarioId)
        {
            var usuario = objNegocio.N_ObtenerUsuario(usuarioId);

            if (usuario != null)
            {
                nombre.Text = usuario.Nombre;
                apellido.Text = usuario.Apellido;
                fechaNacimiento.Value = usuario.FechaNacimiento;
                tipoUsuario.SelectedItem = usuario.Tipo_Usuario;
                contraseña.Text = usuario.Contraseña;

            }
            else
            {
                MessageBox.Show("Usuario no encontrado.");
            }
        }

        private void modificar_Click(object sender, EventArgs e)
        {
            int usuarioId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            int ID_Usuario = usuarioId;
            string Nombre = nombre.Text;
            string Apellido = apellido.Text;
            DateTime FechaNacimiento = fechaNacimiento.Value;
            string Tipo_Usuario = tipoUsuario.SelectedItem.ToString();
            string Contraseña = contraseña.Text;

            try
            {
                objNegocio.N_ModificarUsuarios(ID_Usuario, Nombre, Apellido, FechaNacimiento, Tipo_Usuario, Contraseña);
                MessageBox.Show("Usuario modificado correctamente.");
                ListarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el usuario: " + ex.Message);
            }
        }

        private void registroEdificio_Click(object sender, EventArgs e)
        {
            string nombre = nombreEdificio.Text;
            string nombre_Encargado = nombreEncargado.Text;
            string apellido_Encargado = apellidoEncargado.Text;
            objNegocio.N_RegistrarEdificios(nombre, nombre_Encargado, apellido_Encargado);
            MessageBox.Show("Edificio registrado correctamente.");
            ListarEdificios();
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView2.Rows[e.RowIndex];
                edificioId = Convert.ToInt32(fila.Cells[0].Value);
                CargarDatosEdificio(edificioId);
            }
        }

        private void CargarDatosEdificio(int edificioId)
        {
            var edificio = objNegocio.N_ObtenerEdificio(edificioId);

            if (edificio != null)
            {
                nombreEdificio.Text = edificio.Nombre_Edificio;
                nombreEncargado.Text = edificio.Nombre_Encargado;
                apellidoEncargado.Text = edificio.Apellido_Encargado;

            }
            else
            {
                MessageBox.Show("Edificio no encontrado.");
            }
        }

        private void eliminarEdificio_Click(object sender, EventArgs e)
        {
            objNegocio.N_EliminarEdificios(edificioId);
            MessageBox.Show("edificio eliminado correctamente.");
            ListarEdificios();
            ListarAulas();
        }

        private void modificarEdificio_Click(object sender, EventArgs e)
        {
            int edificioId = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);

            int ID_Edificio = edificioId;
            string Nombre = nombreEdificio.Text;
            string Nombre_Encargado = nombreEncargado.Text;
            string Apellido_Encargado = apellidoEncargado.Text;

            try
            {
                objNegocio.N_ModificarEdificios(ID_Edificio, Nombre, Nombre_Encargado, Apellido_Encargado);
                MessageBox.Show("Edificio modificado correctamente.");
                ListarEdificios();
                ListarAulas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el edificio: " + ex.Message);
            }
        }

        private void registroAula_Click(object sender, EventArgs e)
        {
            int edificio_ID = edificioId;
            string nombre = nombreAula.Text;
            objNegocio.N_RegistrarAulas(edificio_ID, nombre);
            MessageBox.Show("Aula registrada correctamente.");
            ListarAulas();

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView3.Rows[e.RowIndex];
                aulaId = Convert.ToInt32(fila.Cells[0].Value);
                CargarDatosAula(aulaId);
            }
        }

        private void CargarDatosAula(int aulaId)
        {
            var aula = objNegocio.N_ObtenerAula(aulaId);

            if (aula != null)
            {
                idEdificioAula.Text = aula.ID_Edificio.ToString();
                nombreAula.Text = aula.Nombre_Aula;

            }
            else
            {
                MessageBox.Show("aula no encontrada.");
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView4.Rows[e.RowIndex];
                edificioId = Convert.ToInt32(fila.Cells[0].Value);
                CargarDatosEdificioAula(edificioId);
            }
        }

        private void CargarDatosEdificioAula(int edificioAulaId)
        {
            var aula = objNegocio.N_ObtenerEdificio(edificioAulaId);

            if (aula != null)
            {
                idEdificioAula.Text = aula.ID_Edificio.ToString();

            }
            else
            {
                MessageBox.Show("Edificio no encontrado.");
            }
        }

        private void eliminarAula_Click(object sender, EventArgs e)
        {
            objNegocio.N_EliminarAulas(aulaId);
            ListarAulas();
            MessageBox.Show("Aula eliminada correctamente.");
        }

        private void modificarAula_Click(object sender, EventArgs e)
        {
            int aulaId = Convert.ToInt32(dataGridView3.CurrentRow.Cells[0].Value);

            int ID_Aula = aulaId;
            string Nombre = nombreAula.Text;

            try
            {
                objNegocio.N_ModificarAulas(ID_Aula, Nombre);
                MessageBox.Show("Aula modificada correctamente.");
                ListarAulas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el aula: " + ex.Message);
            }
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

        private void RegistrarVisita_Click(object sender, EventArgs e)
        {
            string Nombre = visitaNombre.Text;
            string Apellido = visitaApellido.Text;
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
            ListarVisitas();
        }

        private void eliminarVisita_Click(object sender, EventArgs e)
        {
            objNegocio.N_EliminarVisitas(visitaId);
            ListarVisitas();
            MessageBox.Show("Visita eliminada correctamente.");
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView5.Rows[e.RowIndex];
                visitaId = Convert.ToInt32(fila.Cells[0].Value);
                CargarDatosVisita(visitaId);
            }
        }

        private void CargarDatosVisita(int visitaId)
        {
            var visita = objNegocio.N_ObtenerVisita(visitaId);

            if (visita != null)
            {
                visitaNombre.Text = visita.Nombre.ToString();
                visitaApellido.Text = visita.Apellido.ToString();
                carrera.Text = visita.Carrera.ToString();
                correo.Text = visita.Correo.ToString();
                horaEntrada.Value = visita.Hora_Entrada;
                horaSalida.Value = visita.Hora_Salida;
                nombreEdificio.Text = visita.Nombre_Edificio.ToString();
                nombreAula.Text = visita.Nombre_Aula.ToString();

                if (visita.Foto != null)
                {
                    using (MemoryStream ms = new MemoryStream(visita.Foto))
                    {
                        usuarioImagen.Image = Image.FromStream(ms);
                        usuarioImagen.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                else
                {
                    usuarioImagen.Image = null;
                }

                motivo.Text = visita.Motivo.ToString();
            }
            else
            {
                MessageBox.Show("Visita no encontrada.");
            }
        }

        private void modificarVisita_Click(object sender, EventArgs e)
        {
            int visitaId = Convert.ToInt32(dataGridView5.CurrentRow.Cells[0].Value);

            int ID = visitaId;
            string Nombre = visitaNombre.Text;
            string Apellido = visitaApellido.Text;
            string Carrera = carrera.Text;
            string Correo = correo.Text;
            int Edificio = ((KeyValuePair<int, string>)Edificios.SelectedItem).Key;
            int Aula = ((KeyValuePair<int, string>)Aulas.SelectedItem).Key;
            DateTime HoraEntrada = horaEntrada.Value;
            DateTime HoraSalida = horaSalida.Value;
            string Motivo = motivo.Text;
            byte[] Foto = string.IsNullOrEmpty(rutaImagen) ? null : ConvertirImagen(rutaImagen);

            try
            {
                objNegocio.N_ModificarVisitas(ID, Nombre, Apellido, Carrera, Correo, Edificio, Aula, HoraEntrada, HoraSalida, Motivo, Foto);
                MessageBox.Show("Visita modificada correctamente.");
                ListarVisitas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar la visita: " + ex.Message + "\n" + ex.InnerException?.Message);
            }
        }

        private void Imagen_Click(object sender, EventArgs e)
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

        public void cerrarSesion()
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cerrarSesion();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cerrarSesion();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cerrarSesion();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cerrarSesion();
        }
    }
}