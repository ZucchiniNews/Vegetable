namespace Zucchinimvc.Application.Services.CMS.DTOs;

public class StrapiResponse<T>
{
    public required List<T> Data { get; set; }
}