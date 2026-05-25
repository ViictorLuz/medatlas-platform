using MedAtlas.Domain.Modules.Authentication.Entities;
using MedAtlas.Domain.Modules.Authentication.ValueObjects;

namespace MedAtlas.Domain.Modules.Authentication.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorId(Guid id, CancellationToken cancellationToken = default);
    Task<Usuario?> ObterPorEmail(Email email, CancellationToken cancellationToken = default);
    Task Adicionar(Usuario usuario, CancellationToken cancellationToken = default);
    Task Atualizar(Usuario usuario, CancellationToken cancellationToken = default);
    Task<bool> ExisteEmail(Email email, CancellationToken cancellationToken = default);
}