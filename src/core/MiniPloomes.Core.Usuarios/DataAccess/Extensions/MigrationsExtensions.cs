using MiniPloomes.Core.Usuarios.DataAccess.Migrations;
using System.Data.SqlClient;

namespace MiniPloomes.Core.Usuarios.DataAccess.Extensions;

public static class MigrationsExtensions
{
    public static bool IsApplied(this IMigration migration, SqlConnection connection)
    {
        string migrationId = migration.MigrationId;
        string query = $"SELECT COUNT(*) FROM Controle_Migrations WHERE MigrationId = '{migrationId}'";

        SqlCommand command = new SqlCommand(query, connection);
        int count = (int)command.ExecuteScalar();

        return count > 0;
    }

    public static void MarkAsApplied(this IMigration migration, SqlConnection connection)
    {
        string migrationId = migration.MigrationId;
        string insertQuery = $"INSERT INTO Controle_Migrations (MigrationId, DataAplicacao) VALUES ('{migrationId}', GETUTCDATE())";

        SqlCommand command = new SqlCommand(insertQuery, connection);
        command.ExecuteNonQuery();
    }
}
