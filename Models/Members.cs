namespace ct_foco_backend.Models
{
    public class Members
    {
        public int Id { get; set; }


        public string Nome { get; set; } = "";


        public string Email { get; set; } = "";


        public string Pagamento { get; set; } = "";


        public string Telefone { get; set; } = "";


        public DateTime DataNascimento  { get; set; }


        public int Altura { get; set; }


        public string Modalidade { get; set; } = "";


        public int Horario { get; set; }


        public DateTime DataEntrada { get; set; }

    }
}
