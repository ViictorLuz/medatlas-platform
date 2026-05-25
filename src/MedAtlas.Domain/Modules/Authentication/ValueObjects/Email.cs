using MedAtlas.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace MedAtlas.Domain.Modules.Authentication.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Valor { get; }

        private Email(string valor)
        {  Valor = valor; }

        public static Email Criar(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ExcecaoDeDominio("O endereço de e-mail não pode ser vazio.");

            string valorLimpo = valor.Trim();

            if (!EmailRegex.IsMatch(valorLimpo))
                throw new ExcecaoDeDominio("O formato do e-mail fornecido é invalido.");

            return new Email(valorLimpo);
        }

        public bool Equals(Email? outro) => outro != null && Valor == outro.Valor;
        public override bool Equals(object? obj) => obj is Email outro && Equals(outro);
        public override int GetHashCode() => Valor.GetHashCode();
        public static bool operator == (Email? left, Email? right) => Equals(left, right);
        public static bool operator != (Email? left, Email? right) => !Equals(left, right);
    }
}
