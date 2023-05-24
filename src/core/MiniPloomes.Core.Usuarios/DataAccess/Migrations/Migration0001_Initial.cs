using System.Data.Common;
using System.Data.SqlClient;

namespace MiniPloomes.Core.Usuarios.DataAccess.Migrations;

public class Migration0001_Initial : IMigration
{
    private readonly SqlConnectionStringBuilder _connectionStringBuilder;

    public Migration0001_Initial(SqlConnectionStringBuilder connectionStringBuilder)
    {
        _connectionStringBuilder = connectionStringBuilder;
    }

    public string MigrationId => "Migration0001_Initial";

    public void Aplicar()
    {
        using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ToString()))
        {
            connection.Open();
            string usuariosQuery = @"CREATE TABLE Usuarios (
                                        Id UNIQUEIDENTIFIER NOT NULL, 
                                        Nome NVARCHAR(100) NOT NULL,
                                        Email NVARCHAR(100) NOT NULL
                                        CONSTRAINT PK_Usuario PRIMARY KEY (Id)
                                     );";

            using (SqlCommand command = new SqlCommand(usuariosQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string clientesQuery = @"CREATE TABLE Clientes (
                                        Id UNIQUEIDENTIFIER NOT NULL, 
                                        Nome NVARCHAR(100) NOT NULL,
                                        CreateDate DateTime NOT NULL,
                                        UsuarioId UNIQUEIDENTIFIER NOT NULL
                                        CONSTRAINT PK_Clientes PRIMARY KEY (Id)
                                        CONSTRAINT FK_Clientes_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                                     );";

            using (SqlCommand command = new SqlCommand(clientesQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
