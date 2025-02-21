using System;

namespace MinioWebApi.API.Services;

using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.IO;
using System.Threading.Tasks;

public class MinioService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public MinioService(IConfiguration configuration)
    {
        var minioConfig = configuration.GetSection("Minio");
        _bucketName = minioConfig["BucketName"];
        _minioClient = new MinioClient()
            .WithEndpoint(minioConfig["Endpoint"])
            .WithCredentials(minioConfig["AccessKey"], minioConfig["SecretKey"])
            .WithSSL(Convert.ToBoolean(minioConfig["UseSSL"]))
            .Build();
    }

    public async Task UploadPdfAsync(string objectName, Stream data)
    {
        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(data)
            .WithObjectSize(data.Length)
            .WithContentType("application/pdf"));
    }

    public async Task<Stream> GetPdfAsync(string objectName)
    {
        var ms = new MemoryStream();
        await _minioClient.GetObjectAsync(new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithCallbackStream((stream) => stream.CopyTo(ms)));
        ms.Position = 0;
        return ms;
    }

    public async Task DeletePdfAsync(string objectName)
    {
        await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName));
    }
}