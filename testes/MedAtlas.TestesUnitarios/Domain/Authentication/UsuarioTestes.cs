using Xunit;
using FluentAssertions;
using MedAtlas.Domain.Modules.Authentication.Entities;
using MedAtlas.Domain.Modules.Authentication.ValueObjects;
using MedAtlas.Domain.Exceptions;

namespace MedAtlas.TestesUnitarios.Domain.Authentication;

public class UsuarioTestes
{
    [Fact]
    public void DadosValidos_QuandoCriarUsuario_DeveInstanciarComSucesso()
    {
        var id = Guid.NewGuid();
        var nome = "Dr. Victor Hugo";
        var email = Email.Criar("victor@medatlas.com");
        var senhaHash = "$2a$12$HashSeguroProjetadoComBcrypt...";

        var usuario = new Usuario(id, nome, email, senhaHash);

        usuario.Should().NotBeNull();
        usuario.Id.Should().Be(id);
        usuario.Nome.Should().Be(nome);
        usuario.Email.Should().Be(email);
        usuario.CriadoEm.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void NomeInvalido_QuandoCriarUsuario_DeveLancarExcecaoDeDominio(string nomeInvalido)
    {
        var email = Email.Criar("victor@medatlas.com");
        var senhaHash = "hash_valido";

        Action acao = () => _ = new Usuario(Guid.NewGuid(), nomeInvalido, email, senhaHash);

        acao.Should().Throw<ExcecaoDeDominio>()
            .WithMessage("O nome do usuário é obrigatório.");
    }
}