namespace MiniPloomes.Core.Usuarios.DataAccess.Migrations;

public interface IMigration
{
    string MigrationId { get; }
    void Aplicar();
}
