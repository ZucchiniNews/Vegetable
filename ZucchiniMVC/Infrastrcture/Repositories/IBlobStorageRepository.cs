namespace Zucchinimvc.Infrastrcture.Repositories;

public interface IBlobStorageRepository
{
    Task<string> UploadFileAsync(string fileName, string contentType, Stream fileStream);
    Task<bool> DeleteFileAsync(string fileName);
}