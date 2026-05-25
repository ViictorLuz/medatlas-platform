using MediatR;
using MedAtlas.Domain.Exceptions;
using MedAtlas.Domain.Modules.Library.Entities;
using MedAtlas.Domain.Modules.Library.Interfaces;
using MedAtlas.Application.Features.Library.Common;
using MedAtlas.Application.Features.Library.Events;
using MassTransit;

namespace MedAtlas.Application.Features.Library.Commands.UploadDocument;

public sealed class EnviarDocumentoCommandHandler : IRequestHandler<EnviarDocumentoCommand, Guid>
{
    private readonly IStorageService _storageService;
    private readonly IPublishEndpoint _publishEndpoint;

    public EnviarDocumentoCommandHandler(IStorageService storageService, IPublishEndpoint publishEndpoint)
    {
        _storageService = storageService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(EnviarDocumentoCommand request, CancellationToken cancellationToken)
    {
        if (!ValidadorPdf.ValidarAssinaturaPdf(request.FluxoArquivo))
            throw new ExcecaoDeDominio("O arquivo enviado não é um PDF válido.");

        var documentoId = Guid.NewGuid();
        var nomeUnicoStorage = $"biblioteca/usuarios/{request.UsuarioId}/{documentoId}.pdf";

        var documento = new Documento(
            documentoId,
            request.UsuarioId,
            request.NomeOriginal,
            nomeUnicoStorage,
            request.TamanhoBytes);

        string chaveStorage = await _storageService.SalvarArquivo(
            documento.ChaveStorage,
            request.FluxoArquivo,
            request.ContentType,
            cancellationToken);

        await _publishEndpoint.Publish(new DocumentoEnviadoEvent(
            documento.Id,
            documento.UsuarioId,
            documento.ChaveStorage,
            documento.NomeOriginal
        ), cancellationToken);

        return documento.Id;
    }
}