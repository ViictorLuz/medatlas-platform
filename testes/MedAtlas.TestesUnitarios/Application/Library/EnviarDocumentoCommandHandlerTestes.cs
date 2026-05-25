using Xunit;
using FluentAssertions;
using NSubstitute;
using MedAtlas.Domain.Exceptions;
using MedAtlas.Domain.Modules.Library.Interfaces;
using MedAtlas.Application.Features.Library.Commands.UploadDocument;
using MassTransit;

namespace MedAtlas.TestesUnitarios.Application.Library;

public class EnviarDocumentoCommandHandlerTestes
{
    private readonly IStorageService _storageServiceMock;
    private readonly IPublishEndpoint _publishEndpointMock;
    private readonly EnviarDocumentoCommandHandler _handler;

    public EnviarDocumentoCommandHandlerTestes()
    {
        _storageServiceMock = Substitute.For<IStorageService>();
        _publishEndpointMock = Substitute.For<IPublishEndpoint>();
        _handler = new EnviarDocumentoCommandHandler(_storageServiceMock, _publishEndpointMock);
    }

    [Fact]
    public async Task ArquivoInvalido_QuandoNaoForPdfReal_DeveLancarExcecaoDeDominio()
    {
        var conteudoFake = "TEXTO_QUALQUER_NAO_E_PDF"u8.ToArray();
        using var streamFake = new MemoryStream(conteudoFake);

        var comando = new EnviarDocumentoCommand(
            Guid.NewGuid(),
            "apostila_anatomia.pdf",
            streamFake,
            "application/pdf",
            conteudoFake.Length);

        Func<Task> acao = async () => await _handler.Handle(comando, CancellationToken.None);

        await acao.Should().ThrowAsync<ExcecaoDeDominio>()
            .WithMessage("O arquivo enviado não é um PDF válido.");

        await _storageServiceMock.DidNotReceiveWithAnyArgs().SalvarArquivo(default!, default!, default!, default!);
    }
}