using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Investment.Application.Utilities.Abstractions;

public interface IObjectStoreService
{
    Task<bool> IsBucketExistsAsync(string bucketName);
    Task<(MemoryStream stream, string contentType)> DownloadFileAsync(string objectName, string bucketName);
    Task UploadAsync(IFormFile file, string bucketName);
    Task CreateBucketAsync(string bucketName);
}
