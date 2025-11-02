using PensionFund.Application.Models;
using PensionFund.Domain.Entities;

namespace PensionFund.Application.Extensions;
public static class ConfigurationDtoExtensions
{
    public static List<ConfigurationDto> ToGetconfigurations(
       this List<FundConfiguration> config)
    {
        return config.Select(x => new ConfigurationDto
        {
            Name = x.FundName,
            Type = x.Category
        }).ToList();
    }
}
