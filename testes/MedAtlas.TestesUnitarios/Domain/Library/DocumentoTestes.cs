using Xunit;
using FluentAssertions;
using MedAtlas.Domain.Modules.Library.Entities;
using MedAtlas.Domain.Exceptions;

namespace MedAtlas.TestesUnitarios.Domain.Library;

public class DocumentoTestes
{
    [Fact]
    public void DadosValidos_QuandoCriarDocumento_DeveInstanciarComSucesso()
    {
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var nomeOriginal = "anatomia_cardiorrespiratoria.pdf";
        var chaveStorage = "biblioteca/2026/05/uuid-gerado.pdf";
        long tamanhoBytes = 1024 * 1024 * 5;

        var documento = new Documento(id, usuarioId, nomeOriginal, chaveStorage, tamanhoBytes);

        documento.Should().NotBeNull();
        documento.Id.Should().Be(id);
        documento.UsuarioId.Should().Be(usuarioId);
        documento.NomeOriginal.Should().Be(nomeOriginal);
        documento.ChaveStorage.Should().Be(chaveStorage);
        documento.TamanhoBytes.Should().Be(tamanhoBytes);
        documento.CriadoEm.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-500)]
    public void TamanhoInvalido_QuandoCriarDocumento_DeveLancarExcecaoDeDominio(long tamanhoInvalido)
    {
        Action acao = () => _ = new Documento(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "resumo.pdf",
            "chave/resumo.pdf",
            tamanhoInvalido);

        acao.Should().Throw<ExcecaoDeDominio>()
            .WithMessage("O tamanho do arquivo deve ser maior que zero.");
    }
}