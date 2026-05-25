using Amazon.S3;
using Amazon.S3.Transfer;
using MedAtlas.Domain.Modules.Library.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MedAtlas.Infrastructure.Storage;

public sealed class MinIoStorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _nomeBucket;

    public MinIoStorageService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _nomeBucket = configuration["Storage:BucketName"] ?? "medatlas-biblioteca";
    }

    public async Task<string> SalvarArquivo(
        string nomeArquivo,
        Stream fluxoArquivo,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var bucketExiste = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _nomeBucket);
        if (!bucketExiste)
        {
            await _s3Client.PutBucketAsync(new Amazon.S3.Model.PutBucketRequest
            {
                BucketName = _nomeBucket
            }, cancellationToken);
        }

        using var transferUtility = new TransferUtility(_s3Client);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fluxoArquivo,
            Key = nomeArquivo,
            BucketName = _nomeBucket,
            ContentType = contentType
        };

        await transferUtility.UploadAsync(uploadRequest, cancellationToken);

        return nomeArquivo;
    }

    public async Task DeletarArquivo(string chaveStorage, CancellationToken cancellationToken = default)
    {
        await _s3Client.DeleteObjectAsync(new Amazon.S3.Model.DeleteObjectRequest
        {
            BucketName = _nomeBucket,
            Key = chaveStorage
        }, cancellationToken);
    }
}