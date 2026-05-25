using MedAtlas.Domain.Common;
using MedAtlas.Domain.Exceptions;

namespace MedAtlas.Domain.Modules.Library.Entities;

public sealed class Documento : Entidade
{
    public Guid UsuarioId { get; private set; }
    public string NomeOriginal { get; private set; }
    public string ChaveStorage { get; private set; }
    public long TamanhoBytes { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private Documento() { }

    public Documento(Guid id, Guid usuarioId, string nomeOriginal, string chaveStorage, long tamanhoBytes)
    {
        if (usuarioId == Guid.Empty)
            throw new ExcecaoDeDominio("O identificador do usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(nomeOriginal))
            throw new ExcecaoDeDominio("O nome original do arquivo é obrigatório.");

        if (string.IsNullOrWhiteSpace(chaveStorage))
            throw new ExcecaoDeDominio("A chave de armazenamento do arquivo é obrigatória.");

        if (tamanhoBytes <= 0)
            throw new ExcecaoDeDominio("O tamanho do arquivo deve ser maior que zero.");

        Id = id;
        UsuarioId = usuarioId;
        NomeOriginal = nomeOriginal.Trim();
        ChaveStorage = chaveStorage.Trim();
        TamanhoBytes = tamanhoBytes;
        CriadoEm = DateTime.UtcNow;
    }
}