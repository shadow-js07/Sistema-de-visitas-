using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Capa_Entidad;
using System.Data;
using System.ComponentModel;

namespace Capa_Datos
{
    public class D_Visitas
    {
        SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conect"].ConnectionString);

        public void RegistrarUsuario(Usuarios usuario)
        {
            SqlCommand cmd = new SqlCommand("SP_AgregarUsuario", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
            cmd.Parameters.AddWithValue("@Tipo_Usuario", usuario.Tipo_Usuario);
            cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al agregar el usuario.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void ModificarUsuario(Usuarios usuario)
        {
            SqlCommand cmd = new SqlCommand("SP_ModificarUsuario", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID_Usuario", usuario.ID_Usuario);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
            cmd.Parameters.AddWithValue("@Tipo_Usuario", usuario.Tipo_Usuario);
            cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al modificar el usuario.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void EliminarUsuario(Usuarios usuario)
        {
            SqlCommand cmd = new SqlCommand("SP_EliminarUsuario", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID_Usuario", usuario.ID_Usuario);
            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar el usuario.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public (bool Autenticado, string Tipo_Usuario) AutenticarUsuario(Usuarios usuario)
        {
            SqlCommand cmd = new SqlCommand("SP_AutenticarUsuario", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
            cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);

            try
            {
                conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    bool autenticado = reader.GetInt32(0) == 1;
                    string tipoUsuario = reader.IsDBNull(1) ? null : reader.GetString(1);
                    conexion.Close();
                    return (autenticado, tipoUsuario);
                }
                conexion.Close();
                return (false, null);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al autenticar el usuario.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public Usuarios ObtenerUsuario(Usuarios usuario)
        {
            SqlCommand cmd = new SqlCommand("SP_ObtenerUsuario", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UsuarioID", usuario.ID_Usuario);

            try
            {
                conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Usuarios usuarioResult = null;

                if (reader.Read())
                {
                    usuarioResult = new Usuarios
                    {
                        ID_Usuario = (int)reader["ID_Usuario"],
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        FechaNacimiento = (DateTime)reader["FechaNacimiento"],
                        Tipo_Usuario = reader["Tipo_Usuario"].ToString(),
                        Contraseña = reader["Contraseña"].ToString()
                    };
                }

                reader.Close();
                conexion.Close();

                return usuarioResult;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener el usuario.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public DataTable ListarUsuarios()
        {
            DataTable du = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_ListarUsuarios", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conexion.Open();
                SqlDataAdapter user = new SqlDataAdapter(cmd);
                user.Fill(du);
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener los usuarios registrados.", ex);
            }
            return du;
        }

        public void RegistrarEdificio(Edificios edificio)
        {
            SqlCommand cmd = new SqlCommand("SP_AgregarEdificio", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Nombre_Edificio", edificio.Nombre_Edificio);
            cmd.Parameters.AddWithValue("@Nombre_Encargado", edificio.Nombre_Encargado);
            cmd.Parameters.AddWithValue("@Apellido_Encargado", edificio.Apellido_Encargado);
            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al agregar el Edificio.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void ModificarEdificio(Edificios edificio)
        {
            SqlCommand cmd = new SqlCommand("SP_ModificarEdificio", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID_Edificio", edificio.ID_Edificio);
            cmd.Parameters.AddWithValue("@Nombre_Edificio", edificio.Nombre_Edificio);
            cmd.Parameters.AddWithValue("@Nombre_Encargado", edificio.Nombre_Encargado);
            cmd.Parameters.AddWithValue("@Apellido_Encargado", edificio.Apellido_Encargado);

            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al modificar el edificio.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public List<KeyValuePair<int, string>> ObtenerEdificios()
        {
            SqlCommand cmd = new SqlCommand("SP_ListarEdificios", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            List<KeyValuePair<int, string>> edificios = new List<KeyValuePair<int, string>>();

            try
            {
                conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nombre = reader.GetString(1);
                    edificios.Add(new KeyValuePair<int, string>(id, nombre));
                }
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener los edificios.", ex);
            }

            return edificios;
        }

        public DataTable ListarEdificios()
        {
            DataTable de = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_ListarEdificios", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conexion.Open();
                SqlDataAdapter edificio = new SqlDataAdapter(cmd);
                edificio.Fill(de);
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener los edificios registrados.", ex);
            }
            return de;
        }

        public Edificios ObtenerEdificio(Edificios edificio)
        {
            SqlCommand cmd = new SqlCommand("SP_ObtenerEdificio", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EdificioID", edificio.ID_Edificio);

            try
            {
                conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Edificios edificioResult = null;

                if (reader.Read())
                {
                    edificioResult = new Edificios
                    {
                        ID_Edificio = (int)reader["ID_Edificio"],
                        Nombre_Edificio = reader["Nombre_Edificio"].ToString(),
                        Nombre_Encargado = reader["Nombre_Encargado"].ToString(),
                        Apellido_Encargado = reader["Apellido_Encargado"].ToString(),
                    };
                }

                reader.Close();
                conexion.Close();

                return edificioResult;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener el edificio.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void EliminarEdificio(Edificios edificio)
        {
            SqlCommand cmd = new SqlCommand("SP_EliminarEdificio", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID_Edificio", edificio.ID_Edificio);
            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar el Edificio.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void RegistrarAula(Aulas aula)
        {
            SqlCommand cmd = new SqlCommand("SP_AgregarAula", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EdificioID", aula.ID_Edificio);
            cmd.Parameters.AddWithValue("@Nombre_Aula", aula.Nombre_Aula);
            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al agregar el Aula.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void ModificarAula(Aulas aula)
        {
            SqlCommand cmd = new SqlCommand("SP_ModificarAula", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID_Aula", aula.ID_Aula);
            cmd.Parameters.AddWithValue("@Nombre_Aula", aula.Nombre_Aula);
            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al modificar el Aula.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public List<KeyValuePair<int, string>> ObtenerAulasPorEdificio(int edificioId)
        {
            SqlCommand cmd = new SqlCommand("SP_ObtenerEdificioYAulaID", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EdificioID", edificioId);
            List<KeyValuePair<int, string>> aulas = new List<KeyValuePair<int, string>>();

            try
            {
                conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nombre = reader.GetString(1);
                    aulas.Add(new KeyValuePair<int, string>(id, nombre));
                }
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener las aulas.", ex);
            }

            return aulas;
        }

        public DataTable ListarAulas()
        {
            DataTable da = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_ListarAulas", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conexion.Open();
                SqlDataAdapter aula = new SqlDataAdapter(cmd);
                aula.Fill(da);
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener las aulas registradas.", ex);
            }
            return da;
        }

        public Aulas ObtenerAula(Aulas aula)
        {
            SqlCommand cmd = new SqlCommand("SP_ObtenerAula", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AulaID", aula.ID_Aula);

            try
            {
                conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Aulas aulaResult = null;

                if (reader.Read())
                {
                    aulaResult = new Aulas
                    {
                        ID_Aula = (int)reader["ID_Aula"],
                        ID_Edificio = (int)reader["EdificioID"],
                        Nombre_Aula = reader["Nombre_Aula"].ToString(),
                    };
                }

                reader.Close();
                conexion.Close();

                return aulaResult;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener el aula.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void EliminarAula(Aulas aula)
        {
            SqlCommand cmd = new SqlCommand("SP_EliminarAula", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID_Aula", aula.ID_Aula);
            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar el Aula.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void RegistrarVisita(Visitantes visita)
        {
            SqlCommand cmd = new SqlCommand("SP_AgregarVisita", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Nombre", visita.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", visita.Apellido);
            cmd.Parameters.AddWithValue("@Carrera", visita.Carrera);
            cmd.Parameters.AddWithValue("@Correo", visita.Correo);
            cmd.Parameters.AddWithValue("@EdificioID", visita.Edificio_ID);
            cmd.Parameters.AddWithValue("@AulaID", visita.Aula_ID);
            cmd.Parameters.AddWithValue("@Hora_Entrada", visita.Hora_Entrada);
            cmd.Parameters.AddWithValue("@Hora_Salida", visita.Hora_Salida);
            cmd.Parameters.AddWithValue("@Motivo", visita.Motivo);
            cmd.Parameters.AddWithValue("@Foto", visita.Foto ?? (object)DBNull.Value);

            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar la visita.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public DataTable ListarVisitas()
        {
            DataTable dv = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_ListarVisitas", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conexion.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dv);
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al listar las visitas.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            return dv;

        }

        public void ModificarVisita(Visitantes visita)
        {
            SqlCommand cmd = new SqlCommand("SP_ModificarVisita", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", visita.ID);
            cmd.Parameters.AddWithValue("@Nombre", visita.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", visita.Apellido);
            cmd.Parameters.AddWithValue("@Carrera", visita.Carrera);
            cmd.Parameters.AddWithValue("@Correo", visita.Correo);
            cmd.Parameters.AddWithValue("@EdificioID", visita.Edificio_ID);
            cmd.Parameters.AddWithValue("@AulaID", visita.Aula_ID);
            cmd.Parameters.AddWithValue("@Hora_Entrada", visita.Hora_Entrada);
            cmd.Parameters.AddWithValue("@Hora_Salida", visita.Hora_Salida);
            cmd.Parameters.AddWithValue("@Motivo", visita.Motivo);
            cmd.Parameters.AddWithValue("@Foto", visita.Foto);
            try
            {
                conexion.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new Exception("No se encontró la visita con el ID especificado.");
                }
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al modificar la visita.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void EliminarVisita(Visitantes visita)
        {
            SqlCommand cmd = new SqlCommand("SP_EliminarVisita", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", visita.ID);
            try
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar la visita.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public Visitantes ObtenerVisitas(Visitantes visita)
        {
            SqlCommand cmd = new SqlCommand("SP_ObtenerVisita", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VisitaID", visita.ID);

            try
            {
                conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Visitantes visitaResult = null;

                if (reader.Read())
                {
                    visitaResult = new Visitantes
                    {
                        ID = (int)reader["ID"],
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        Carrera = reader["Carrera"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        Nombre_Edificio = reader["Nombre_Edificio"].ToString(),
                        Hora_Entrada = (DateTime)reader["Hora_Entrada"],
                        Hora_Salida = (DateTime)reader["Hora_Salida"],
                        Motivo = reader["Motivo"].ToString(),
                        Nombre_Aula = reader["Nombre_Aula"] != DBNull.Value ? reader["Nombre_Aula"].ToString() : null,
                        Foto = reader["Foto"] != DBNull.Value ? (byte[])reader["Foto"] : null
                    };
                }

                reader.Close();
                conexion.Close();

                return visitaResult;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener el usuario.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public DataTable ObtenerVisitasEncargado(string Usuario, string Contraseña)
        {
            DataTable dv = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_ObtenerVisitaEncargado", conexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@usuario", Usuario);
            cmd.Parameters.AddWithValue("@contraseña", Contraseña);
            try
            {
                conexion.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dv);
                conexion.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al listar las visitas.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            return dv;
        }
    }
}
