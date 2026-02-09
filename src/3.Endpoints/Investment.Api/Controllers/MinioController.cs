using System.IO;
using System.Security.AccessControl;
using Investment.Api.Utilities;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Minio.DataModel;

namespace Investment.Api.Controllers;

#if DEBUG
[ApiRoute(1, "Minio")]
public class MinioController : Controller
{
    public IObjectStoreService objectStoreService { get; set; }
    public RegistrationVerifyEmailSettings verifyEmailSettings { get; set; }
    public MinioController(IObjectStoreService objectStoreService, IOptions<RegistrationVerifyEmailSettings> options)
    {
        this.objectStoreService = objectStoreService;
        verifyEmailSettings = options.Value;
    }

    [HttpPost("Bucket/{BucketName}/Upload")]
    public async Task<IActionResult> UploadFile(IFormFile formFile, [FromRoute] string BucketName)
    {
        bool isExist = await objectStoreService.IsBucketExistsAsync(BucketName);

        if (!isExist) return BadRequest($"{BucketName} is not exist");

        await objectStoreService.UploadAsync(formFile, BucketName);

        return Ok();
    }

    [HttpPost("Bucket")]
    public async Task<IActionResult> CreateBucket([FromBody] string BucketName)
    {
        await objectStoreService.CreateBucketAsync(BucketName);

        return Ok();
    }
    [HttpPost("{BucketName}/{ObjectName}/Download")]
    public async Task<IActionResult> DownloadFile(
        [FromRoute] string ObjectName, [FromRoute] string BucketName)
    {
        (MemoryStream memoryStream, string contentType) =
             await objectStoreService.DownloadFileAsync(
                 objectName: ObjectName, bucketName: BucketName);

        return File(memoryStream, contentType, ObjectName);
    }
}
#endif