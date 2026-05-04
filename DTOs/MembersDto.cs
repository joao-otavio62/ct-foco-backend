namespace ct_foco_backend.DTOs
{
    public class MembersDto
    {
        public int Id { get; set; }
        
        public string Nome { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public string Pagamento { get; set; } = string.Empty;
        
        public DateTime? Vencimento { get; set; }
        
        public string Telefone { get; set; } = string.Empty;
        
        public DateOnly DataNascimento { get; set; }
        
        public int Altura { get; set; }
        
        public string Modalidade { get; set; } = string.Empty;
        
        public string Horario { get; set; } = string.Empty;
        
        public DateOnly DataEntrada { get; set; }
    }
}