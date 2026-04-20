namespace Zucchinimvc.Models.DTOs.StrapiDTOs;

public class StrapiResponse<T>
{
    public required List<T> Data { get; set; }
}