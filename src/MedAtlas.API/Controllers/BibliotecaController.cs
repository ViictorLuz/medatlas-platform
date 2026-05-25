using MediatR;
using Microsoft.AspNetCore.Mvc;
using MedAtlas.Application.Features.Library.Commands.UploadDocument;

namespace MedAtlas.API.Controllers;

[ApiController]
[Route("api/biblioteca")]
[DisableRequestSizeLimit]
public sealed class BibliotecaController : ControllerBase
{
    private readonly IMediator _mediator;

    public BibliotecaController(IMediator _mediator)
    {
        this._mediator = _mediator;
    }

    /// <summary>
    /// Realiza o upload seguro de um arquivo PDF para a biblioteca do estudante.
    /// </summary>
    /// <param name="usuarioId">ID do usuário (Simulado temporariamente até termos o Token JWT extraído)</param>
    /// <param name="arquivo">O arquivo PDF físico vindo do formulário</param>
    [HttpPost("usuarios/{usuarioId:guid}/upload")]
    [DisableRequestSizeLimit]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EnviarDocumento(Guid usuarioId, IFormFile arquivo, CancellationToken cancellationToken)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest("Nenhum arquivo foi enviado.");

        // Abrimos o stream do arquivo para processamento sob demanda (ganho de memória)
        using var fluxoArquivo = arquivo.OpenReadStream();

        var comando = new EnviarDocumentoCommand(
            usuarioId,
            arquivo.FileName,
            fluxoArquivo,
            arquivo.ContentType,
            arquivo.Length
        );

        // Disbucha para o nosso Handler na camada de Aplicação
        Guid documentoId = await _mediator.Send(comando, cancellationToken);

        return CreatedAtAction(nameof(EnviarDocumento), new { id = documentoId }, new { id = documentoId });
    }
}