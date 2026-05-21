using Zucchinimvc.Application.Services.Analytics.DTOs;
using ZucchiniCore.Entities;

namespace Zucchinimvc.Models.ViewModels;

public class AdminDashboardViewModel
{
    public AnalyticsSummaryDto Analytics { get; set; } = new();
    public  IEnumerable<User> Users { get; set; } = [];
}
