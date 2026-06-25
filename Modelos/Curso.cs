namespace EduDataAPI.Modelos
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int IdProfesor { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime Fecha_Creacion { get; set; }
        public string Creado_Por { get; set; } = string.Empty;
        public DateTime? Fecha_Modificacion { get; set; }
        public string? Modificado_Por { get; set; }
    }
}
