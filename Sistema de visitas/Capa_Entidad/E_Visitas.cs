using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    
        public class Usuarios
        {
            public int ID_Usuario { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime FechaNacimiento { get; set; }    
            public string Tipo_Usuario { get; set; }
            public string Usuario { get; set; }
            public string Contraseña { get; set; }
        }

        public class Edificios
        {
            public int ID_Edificio { get; set; }
            public string Nombre_Edificio { get; set; }
            public string Nombre_Encargado { get; set; }
            public string Apellido_Encargado { get; set; }
        }

        public class Aulas
        {
            public int ID_Aula { get; set; }
            public string Nombre_Aula { get; set; }
            public int ID_Edificio {  set; get; }
        }

        public class Visitantes
        {
            public int ID { get; set; }
            public string Usuario { get; set; }
            public string Contraseña { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Carrera { get; set; }
            public string Correo { get; set; }
            public int Edificio_ID { get; set; }
            public string Nombre_Edificio { set; get; }
            public int Aula_ID { get; set; }
            public string Nombre_Aula {  set; get; }
            public DateTime Hora_Entrada { get; set; }
            public DateTime Hora_Salida { get; set; }
            public string Motivo { get; set; }
            public byte[] Foto { get; set; }
        }
    
}

