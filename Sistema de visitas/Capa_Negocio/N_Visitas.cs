using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos;
using Capa_Entidad;
using System.Data;

namespace Capa_Negocio
{
    public class N_Visitas
    {
        D_Visitas objDato = new D_Visitas();

        public void N_RegistrarUsuarios(string nombre, string apellido, DateTime fechaN, string tipoUsuario, string contraseña)
        {
            Usuarios usuario = new Usuarios
            {
                Nombre = nombre,
                Apellido = apellido,
                FechaNacimiento = fechaN,
                Tipo_Usuario = tipoUsuario,
                Contraseña = contraseña
            };
            objDato.RegistrarUsuario(usuario);
        }

        public void N_ModificarUsuarios(int ID, string nombre, string apellido, DateTime fechaN, string tipoUsuario, string contraseña)
        {
            Usuarios usuario = new Usuarios
            {
                ID_Usuario = ID,
                Nombre = nombre,
                Apellido = apellido,
                FechaNacimiento = fechaN,
                Tipo_Usuario = tipoUsuario,
                Contraseña = contraseña
            };
            objDato.ModificarUsuario(usuario);
        }

        public void N_EliminarUsuarios(int ID)
        {
            Usuarios usuario = new Usuarios
            {
                ID_Usuario = ID,
            };
            objDato.EliminarUsuario(usuario);
        }

        public (bool Autenticado, string Tipo_Usuario) N_AutenticarUsuarios(string Usuario, string contraseña)
        {
            Usuarios usuario = new Usuarios
            {
                Usuario = Usuario,
                Contraseña = contraseña
            };
            return objDato.AutenticarUsuario(usuario);
        }

        public Usuarios N_ObtenerUsuario(int Usuario_ID)
        {
            Usuarios usuario = new Usuarios
            {
                ID_Usuario = Usuario_ID,
            };

            return objDato.ObtenerUsuario(usuario);
        }

        public DataTable N_ListarUsuarios()
        {
            return objDato.ListarUsuarios();
        }

        public void N_RegistrarEdificios(string nombreEdificio, string nombreEncargado, string ApellidoEncargado)
        {
            Edificios edificio = new Edificios
            {
                Nombre_Edificio = nombreEdificio,
                Nombre_Encargado = nombreEncargado,
                Apellido_Encargado = ApellidoEncargado
            };
            objDato.RegistrarEdificio(edificio);
        }

        public void N_ModificarEdificios(int edificioId, string nombreEdificio, string nombreEncargado, string apellidoEncargado)
        {
            Edificios edificio = new Edificios
            {
                ID_Edificio = edificioId,
                Nombre_Edificio = nombreEdificio,
                Nombre_Encargado = nombreEncargado,
                Apellido_Encargado = apellidoEncargado
            };
            objDato.ModificarEdificio(edificio);
        }


        public List<KeyValuePair<int, string>> ObtenerEdificios()
        {
            return objDato.ObtenerEdificios();
        }

        public DataTable N_ListarEdificios()
        {
            return objDato.ListarEdificios();
        }

        public Edificios N_ObtenerEdificio(int Edificio_ID)
        {
            Edificios edificio = new Edificios
            {
                ID_Edificio = Edificio_ID,
            };

            return objDato.ObtenerEdificio(edificio);
        }

        public void N_EliminarEdificios(int IDEdificio)
        {
            Edificios edificio = new Edificios
            {
                ID_Edificio = IDEdificio
            };
            objDato.EliminarEdificio(edificio);
        }

        public void N_RegistrarAulas(int ID_Edificio, string nombreAula)
        {
            Aulas aula = new Aulas
            {
                ID_Edificio = ID_Edificio,
                Nombre_Aula = nombreAula
            };
            objDato.RegistrarAula(aula);
        }

        public void N_ModificarAulas(int ID_Aula, string nombreAula)
        {
            Aulas aula = new Aulas
            {
                ID_Aula = ID_Aula,
                Nombre_Aula = nombreAula
            };
            objDato.ModificarAula(aula);
        }

        public List<KeyValuePair<int, string>> ObtenerAulasPorEdificio(int edificioId)
        {
            return objDato.ObtenerAulasPorEdificio(edificioId);
        }

        public DataTable N_ListarAulas()
        {
            return objDato.ListarAulas();
        }

        public Aulas N_ObtenerAula(int Aula_ID)
        {
            Aulas aula = new Aulas
            {
                ID_Aula = Aula_ID,
            };

            return objDato.ObtenerAula(aula);
        }

        public void N_EliminarAulas(int ID_Aula)
        {
            Aulas aula = new Aulas
            {
                ID_Aula = ID_Aula
            };
            objDato.EliminarAula(aula);
        }

        public void N_RegistrarVisitas(string Nombre, string Apellido, string Carrera, string Correo, int EdificioID, int AulaID, DateTime HoraEntrada, DateTime HoraSalida, string Motivo, byte[] Foto)
        {
            Visitantes visitante = new Visitantes
            {
                Nombre = Nombre,
                Apellido = Apellido,
                Carrera = Carrera,
                Correo = Correo,
                Edificio_ID = EdificioID,
                Aula_ID = AulaID,
                Hora_Entrada = HoraEntrada,
                Hora_Salida = HoraSalida,
                Motivo = Motivo,
                Foto = Foto
            };

            objDato.RegistrarVisita(visitante);
        }

        public DataTable N_ListarVisitas()
        {
            return objDato.ListarVisitas();
        }

        public Visitantes N_ObtenerVisita(int visita_ID)
        {
            Visitantes visita = new Visitantes
            {
                ID = visita_ID,
            };

            return objDato.ObtenerVisitas(visita);
        }

        public DataTable N_ObtenerVisitaEncargado(string usuario, string contraseña)
        {
            return objDato.ObtenerVisitasEncargado(usuario, contraseña);
        }

        public void N_ModificarVisitas(int ID_Visita, string nombre, string apellido, string carrera, string correo, int ID_Edificio, int ID_Aula, DateTime horaEntrada, DateTime horaSalida, string motivo, byte[] foto)
        {
            Visitantes visita = new Visitantes
            {
                ID = ID_Visita,
                Nombre = nombre,
                Apellido = apellido,
                Carrera = carrera,
                Correo = correo,
                Edificio_ID = ID_Edificio,
                Aula_ID = ID_Aula,
                Hora_Entrada = horaEntrada,
                Hora_Salida = horaSalida,
                Motivo = motivo,
                Foto = foto
            };
            objDato.ModificarVisita(visita);
        }

        public void N_EliminarVisitas(int ID_Visita)
        {
            Visitantes visita = new Visitantes
            {
                ID = ID_Visita
            };
            objDato.EliminarVisita(visita);
        }
    }
}

