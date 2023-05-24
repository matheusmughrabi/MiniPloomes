namespace MiniPloomes.Core.Usuarios.Entity;

public class ClienteEntity : Entity
{
    public string Nome { get; set; }
    public DateTime DataDeCriacao { get; set; }
    public Guid UsuarioId { get; set; }
}
