using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ZucchiniBackgroundJobs;

public class ImageResize
{
    private readonly ILogger<ImageResize> _logger;

    public ImageResize(ILogger<ImageResize> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ImageResize))]   // connection name - TBC.
    public async Task Run([BlobTrigger("/{name}", Connection = "zucchiniimages")] Stream stream, string name)
    {
        using var blobStreamReader = new StreamReader(stream);
        var content = await blobStreamReader.ReadToEndAsync();
        _logger.LogInformation("C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}", name, content);
    }
}