namespace MiniPloomes.Core.Usuarios.Entity;

public abstract class Entity
{
    public Entity()
    {
        if (Id == Guid.Empty)
        {
            Id = Guid.NewGuid();
        }
        
    }

    public Guid Id { get; set; }
}
