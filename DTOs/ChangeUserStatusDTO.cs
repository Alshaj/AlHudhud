using System.ComponentModel.DataAnnotations;

namespace BestPriceStore.DTOs;

public class ChangeUserStatusDTO
{
    [Required]
    public bool IsActive { get; set; }
}
