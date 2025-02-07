namespace TrilhaApiDesafio.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public EnumStatusTarefa Status { get; set; }
    }
}