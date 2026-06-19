namespace EduDataAPI.Modelos
{
    public class Usuario
    {

        public int Id { get; set; }

        public string Apodo { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Clave { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;

        public string Rol { get; set; } = "Usuario";

        public DateTime Fecha_Creacion { get; set; }

        public string Creado_Por { get; set; } = string.Empty;

        public DateTime? Fecha_Modificacion { get; set; }

        public string? Modificado_Por { get; set; }

    }
}
