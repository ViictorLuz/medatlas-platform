using MediatR;

namespace MedAtlas.Application.Features.Library.Commands.UploadDocument;

public sealed record EnviarDocumentoCommand(
    Guid UsuarioId,
    string NomeOriginal,
    Stream FluxoArquivo,
    string ContentType,
    long TamanhoBytes) : IRequest<Guid>;