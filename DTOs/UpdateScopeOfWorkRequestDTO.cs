using System.ComponentModel.DataAnnotations;

namespace BestPriceStore.DTOs;

public class UpdateScopeOfWorkRequestDTO
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public bool IsNeedInspection { get; set; }
}
