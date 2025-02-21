using Microsoft.AspNetCore.Mvc;
using MinioWebApi.API.Services;


[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly MinioService _minioService;

    public PdfController(MinioService minioService)
    {
        _minioService = minioService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadPdf([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        await _minioService.UploadPdfAsync(file.FileName, stream);
        return Ok("File uploaded successfully.");
    }

    [HttpGet("download/{objectName}")]
    public async Task<IActionResult> GetPdf(string objectName)
    {
        var stream = await _minioService.GetPdfAsync(objectName);
        return File(stream, "application/pdf", objectName);
    }

    [HttpDelete("delete/{objectName}")]
    public async Task<IActionResult> DeletePdf(string objectName)
    {
        await _minioService.DeletePdfAsync(objectName);
        return Ok("File deleted successfully.");
    }
}

