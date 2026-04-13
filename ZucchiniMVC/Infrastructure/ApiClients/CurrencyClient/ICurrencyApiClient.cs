using Zucchinimvc.Models.DTOs.CurrencyDTOs;

namespace Zucchinimvc.Infrastructure.ApiClients.CurrencyClient
{
    public interface ICurrencyApiClient
    {
        Task<CurrencyRateDTO>
    }
}
