namespace Fitin.Application.Products.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(Stream stream, string fileName);
}
