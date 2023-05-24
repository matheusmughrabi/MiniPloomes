using Microsoft.AspNetCore.Mvc;
using MiniPloomes.Core.Usuarios.DataAccess.Repository;
using MiniPloomes.Core.Usuarios.Dtos;
using MiniPloomes.Core.Usuarios.Entity;

namespace MiniPloomes.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    // TODO - Muito importante implementar autenticação para que os endpoints não precisem receber o id do usuário
    // Do contrário qualquer usuário poderá ver os dados do outro e alterá-los
    private readonly UsuarioRepository _usuarioRepository;

    public UsuariosController(UsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpPost("Criar")]
    public async Task<IActionResult> CriarUsuario(CriarUsuarioDto dto)
    {
        var usuarioEntity = new UsuarioEntity()
        {
            Nome = dto.Nome,
            Email = dto.Email
        };

        await _usuarioRepository.Inserir(usuarioEntity);

        return Ok();
    }

    [HttpGet("Obter/Perfil")]
    public async Task<IActionResult> ObterPerfilUsuario(Guid id)
    {
        var usuario = await _usuarioRepository.ObterPorId(id);
        return Ok(usuario);
    }

    [HttpPost("Cliente/Criar")]
    public async Task<IActionResult> CriarCliente(CriarClienteDto dto)
    {
        var entity = new ClienteEntity()
        {
            Nome = dto.Nome,
            UsuarioId = dto.UsuarioId
        };

        await _usuarioRepository.InserirCliente(entity);

        return Ok();
    }

    [HttpGet("Obter/Clientes")]
    public async Task<IActionResult> ObterClientes(Guid usuarioId)
    {
        var usuario = await _usuarioRepository.ObterClientes(usuarioId);
        return Ok(usuario);
    }

    [HttpPut("Cliente/Atualizar")]
    public async Task<IActionResult> AtualizarCliente(AtualizarClienteDto dto)
    {
        var entity = new ClienteEntity()
        {
            Id = dto.Id,
            Nome = dto.Nome,
        };

        await _usuarioRepository.UpdateCliente(entity);

        return Ok();
    }
}
