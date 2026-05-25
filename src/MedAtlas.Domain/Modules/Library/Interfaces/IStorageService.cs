namespace MedAtlas.Domain.Modules.Library.Interfaces;

public interface IStorageService
{
    /// <summary>
    /// Faz o upload de um arquivo para o storage seguro e retorna a chave única de armazenamento.
    /// </summary>
    Task<string> SalvarArquivo(string nomeArquivo, Stream fluxoArquivo, string contentType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um arquivo do storage seguro através da sua chave única.
    /// </summary>
    Task DeletarArquivo(string chaveStorage, CancellationToken cancellationToken = default);
}