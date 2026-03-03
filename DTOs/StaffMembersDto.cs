namespace ct_foco_backend.DTOs
{
    public class StaffMembersDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public IFormFile? Foto { get; set; }
        public Boolean Status { get; set; } = true;
    }
}
