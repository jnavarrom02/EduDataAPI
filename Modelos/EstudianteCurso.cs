namespace EduDataAPI.Modelos
{
    public class EstudianteCurso
    {
        public int Id { get; set; }
        public int IdEstudiante { get; set; }
        public int IdCurso { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime Fecha_Creacion { get; set; }
        public string Creado_Por { get; set; } = string.Empty;
        public DateTime? Fecha_Modificacion { get; set; }
        public string? Modificado_Por { get; set; }
    }
}
