using MiniPloomes.Core.Usuarios.DataAccess.Extensions;
using MiniPloomes.Core.Usuarios.DataAccess.Migrations;
using System.Data.SqlClient;

namespace MiniPloomes.Core.Usuarios.Bootstrapper;

public class MigrationsBootstrapper : IBootstrapper
{
    private readonly IEnumerable<IMigration> _migrations;
    private readonly SqlConnectionStringBuilder _connectionStringBuilder;

    public MigrationsBootstrapper(
        IEnumerable<IMigration> migrations,
        SqlConnectionStringBuilder connectionStringBuilder)
    {
        _migrations = migrations;
        _connectionStringBuilder = connectionStringBuilder;
    }

    public void Executar()
    {
        InicializarBanco(_connectionStringBuilder.InitialCatalog);
        AplicarMigrations();
    }

    private void AplicarMigrations()
    {
        using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ToString()))
        {
            connection.Open();

            foreach (var migration in _migrations)
            {
                if (migration.IsApplied(connection))
                    continue;

                migration.Aplicar();
                migration.MarkAsApplied(connection);
            }
        }
    }

    private void InicializarBanco(string nomeBanco)
    {
        bool bancoExiste;
        var databaseTemp = _connectionStringBuilder.InitialCatalog;
        _connectionStringBuilder.InitialCatalog = "master";

        using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ToString()))
        {
            connection.Open();

            var query = $"USE master; SELECT COUNT(*) FROM sys.databases WHERE name = '{nomeBanco}'";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                int databaseCount = (int)command.ExecuteScalar();
                bancoExiste = (databaseCount > 0);
            }

            if (bancoExiste == false)
            {
                string bancoQuery = $"CREATE DATABASE {nomeBanco}";

                using (SqlCommand command = new SqlCommand(bancoQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        _connectionStringBuilder.InitialCatalog = databaseTemp;

        if (bancoExiste == false)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ToString()))
            {
                connection.Open();

                string usuariosQuery = @"CREATE TABLE Controle_Migrations (
                                   MigrationId NVARCHAR(100) NOT NULL, 
                                   DataAplicacao DATETIME NOT NULL,
                                   CONSTRAINT PK_Migrations PRIMARY KEY (MigrationId)
                                  );";

                using (SqlCommand command = new SqlCommand(usuariosQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
