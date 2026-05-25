namespace MedAtlas.Application.Features.Library.Events;

public sealed record DocumentoEnviadoEvent(
    Guid DocumentoId,
    Guid UsuarioId,
    string ChaveStorage,
    string NomeOriginal);