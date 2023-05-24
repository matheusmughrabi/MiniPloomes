using MiniPloomes.Core.Usuarios.Entity;
using System.Data.SqlClient;

namespace MiniPloomes.Core.Usuarios.DataAccess.Repository;

public class UsuarioRepository
{
    private readonly SqlConnectionStringBuilder _connectionStringBuilder;

    public UsuarioRepository(SqlConnectionStringBuilder connectionStringBuilder)
    {
        _connectionStringBuilder = connectionStringBuilder;
    }

    public async Task<UsuarioEntity> ObterPorId(Guid id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ToString()))
        {
            await connection.OpenAsync();

            string query = $"SELECT Id, Nome, Email FROM Usuarios WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    await reader.ReadAsync();

                    return new UsuarioEntity()
                    {
                        Id = reader.GetGuid(0),
                        Nome = reader.GetString(1),
                        Email = reader.GetString(2)
                    };
                }
            }
        }
    }

    public async Task<List<ClienteEntity>> ObterClientes(Guid id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ToString()))
        {
            await connection.OpenAsync();

            string query = $"SELECT C.Id As Id, C.Nome As Nome FROM Usuarios U INNER JOIN Clientes C ON U.Id = C.UsuarioId WHERE U.Id = @Id;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    var clientes = new List<ClienteEntity>();
                    while (await reader.ReadAsync())
                    {
                        var cliente = new ClienteEntity()
                        {
                            Id = reader.GetGuid(0),
                            Nome = reader.GetString(1)
                        };

                        clientes.Add(cliente);
                    }

                    return clientes;
                }
            }
        }
    }

    public async Task Inserir(UsuarioEntity usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ToString()))
        {
            await connection.OpenAsync();

            string commandQuery = $"INSERT INTO Usuarios (Id, Nome, Email) VALUES (@Id, @Nome, @Email)";

            using (SqlCommand command = new SqlCommand(commandQuery, connection))
            {
                command.Parameters.AddWithValue("@Id", usuario.Id);
                command.Parameters.AddWithValue("@Nome", usuario.Nome);
                command.Parameters.AddWithValue("@Email", usuario.Email);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task InserirCliente(ClienteEntity cliente)
    {
        using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ToString()))
        {
            await connection.OpenAsync();

            string commandQuery = $"INSERT INTO Clientes (Id, Nome, CreateDate, UsuarioId) VALUES (@Id, @Nome, GETUTCDATE(), @UsuarioId)";

            using (SqlCommand command = new SqlCommand(commandQuery, connection))
            {
                command.Parameters.AddWithValue("@Id", cliente.Id);
                command.Parameters.AddWithValue("@Nome", cliente.Nome);
                command.Parameters.AddWithValue("@UsuarioId", cliente.UsuarioId);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateCliente(ClienteEntity cliente)
    {
        using (SqlConnection connection = new SqlConnection(_connectionStringBuilder.ToString()))
        {
            await connection.OpenAsync();

            string commandQuery = $"UPDATE Clientes SET Nome = @Nome WHERE Id = @Id;";

            using (SqlCommand command = new SqlCommand(commandQuery, connection))
            {
                command.Parameters.AddWithValue("@Id", cliente.Id);
                command.Parameters.AddWithValue("@Nome", cliente.Nome);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
