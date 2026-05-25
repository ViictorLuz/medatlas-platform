using MedAtlas.Domain.Common;
using MedAtlas.Domain.Modules.Authentication.ValueObjects;
using MedAtlas.Domain.Exceptions;

namespace MedAtlas.Domain.Modules.Authentication.Entities
{
    public sealed class Usuario : Entidade
    {
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public string SenhaHash { get; private set; }
        public string? TokenAtualizacao { get; private set; }
        public DateTime? DataExpiracaoTokenAtualizacao { get; private set; }
        public DateTime CriadoEm { get; private set; }

        private Usuario() { }

        public Usuario(Guid id, string nome, Email email, string senhaHash)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ExcecaoDeDominio("O nome do usuário é obrigatório.");

            if (string.IsNullOrWhiteSpace(senhaHash))
                throw new ExcecaoDeDominio("O hash da senha é obrigatório");

            Id = id;
            Nome = nome.Trim();
            Email = email;
            SenhaHash = senhaHash;
            CriadoEm = DateTime.UtcNow;
        }

        public void AtualizarTokenAtualizacao(string token, DateTime dataExpiracao)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ExcecaoDeDominio("O token de atualização não poder ser nulo ou vazio.");
        
            TokenAtualizacao = token;
            DataExpiracaoTokenAtualizacao = dataExpiracao;
        }

        public void RevogarTokenAtualizacao()
        {
            TokenAtualizacao = null;
            DataExpiracaoTokenAtualizacao = null;
        }

        public bool ValidarTokenAtualizacao(string token)
        {
            return TokenAtualizacao == token && DataExpiracaoTokenAtualizacao > DateTime.UtcNow;
        }
    }
}
