using Investment.Application.Utilities.Configs;
using Investment.Application.Utilities.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;

namespace Investment.Infrastructure.Services;

public class ObjectStoreService : IObjectStoreService
{
    private readonly IMinioClient _minio;
    private readonly ObjectStoreConfig _objectSroreConfig;
    public ObjectStoreService(IOptions<ObjectStoreConfig> config)
    {
        _objectSroreConfig = config.Value;
        _minio = new MinioClient()
        .WithEndpoint(_objectSroreConfig.Hostname)
        .WithCredentials(_objectSroreConfig.AccessKey, _objectSroreConfig.SecretAccessKey)
        .WithSSL(_objectSroreConfig.IsSecure)
        .Build();
    }
    public async Task<(MemoryStream stream, string contentType)> DownloadFileAsync(string objectName, string bucketName)
    {
        var stat = await _minio.StatObjectAsync(
                                new StatObjectArgs().WithBucket(bucketName).WithObject(objectName)
                          );

        var ms = new MemoryStream();

        await _minio.GetObjectAsync(new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)

            .WithCallbackStream(stream => stream.CopyTo(ms)));

        ms.Position = 0;
        return (ms, stat.ContentType ?? "application/octet-stream");
    }

    public async Task<bool> IsBucketExistsAsync(string bucketName)
    {
        bool isExist = await _minio.BucketExistsAsync(
           new BucketExistsArgs().WithBucket(bucketName));

        return isExist;
    }
    public async Task CreateBucketAsync(string bucketName)
    {
        await _minio.MakeBucketAsync(
            new MakeBucketArgs().WithBucket(bucketName));
    }
    public async Task UploadAsync(IFormFile file, string bucketName)
    {

        var objectName = file.FileName;

        await using var stream = file.OpenReadStream();

        var putArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType ?? "application/octet-stream");

        PutObjectResponse uploadResponse = await _minio.PutObjectAsync(putArgs);

        if (uploadResponse.ResponseStatusCode != System.Net.HttpStatusCode.OK) throw new HttpRequestException(uploadResponse.ResponseContent);
    }
}
