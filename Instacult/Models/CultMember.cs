namespace Instacult.Models;

public class CultMember : Profile
{
  public int CultId { get; set; }
  public int CultMemberId { get; set; }
  public string MemberRole { get; set; }
}
