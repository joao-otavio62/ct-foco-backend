namespace ct_foco_backend.Models
{
    public class StaffMembers
    {
        public int Id { get; set; }

        public string Nome { get; set; } = "";

        public string Cargo { get; set; } = "";

        public string Email { get; set; } = "";

        
        public string Telefone { get; set; } = "";


        public string Especialidade { get; set; } = "";


        public string? Foto { get; set; }


        public Boolean Status { get; set; } = true;


    }
}
