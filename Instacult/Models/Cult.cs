using System.ComponentModel.DataAnnotations;

namespace Instacult.Models;

public class Cult : DbItem<int>
{
  [Required]
  [MinLength(3)]
  [MaxLength(15)]
  public string Name { get; set; }
  [Required]
  [MinLength(25)]
  public string Description { get; set; }

  [Range(typeof(decimal), "0", "9999999", ErrorMessage = "That fee is simply unacceptable... the value must be between {0}, {1}")]
  public decimal Fee { get; set; }
  public string CoverImg { get; set; } = "";
  public string LeaderId { get; set; }

  // populated properties
  public int MemberCount { get; set; }
  public Profile Leader { get; set; }

}


// public class CultDTO
// {
//   // DTO Data Transfer Object
//   public int Id { get; set; }
//   public string Name { get; set; }
//   public string Description { get; set; }
//   public decimal Fee { get; set; }
//   public string CoverImg { get; set; }
//   public string LeaderId { get; set; }
// }
