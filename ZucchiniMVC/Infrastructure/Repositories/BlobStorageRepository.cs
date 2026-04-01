namespace Zucchinimvc.Infrastructure.Repositories;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
public class BlobStorageRepository : IBlobStorageRepository
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<BlobStorageRepository> _logger;

    public BlobStorageRepository(IConfiguration configuration, ILogger<BlobStorageRepository> logger)
    {
        _logger = logger;
        _containerClient = new BlobContainerClient(
            configuration.GetConnectionString("AzureStorage"), "zucchiniimages"
            );
    }

    public async Task<string> UploadFileAsync(string fileName, string contentType, Stream fileStream)
    {
        try
        {
            await _containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            return await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            throw;
        }
    }
}