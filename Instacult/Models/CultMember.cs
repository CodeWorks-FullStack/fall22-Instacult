namespace Instacult.Models;

public class CultMember : Profile
{
  public int CultId { get; set; }
  public int CultMemberId { get; set; }
  public string MemberRole { get; set; }
}


public class FreshMeatDTO : DbItem<int>
{
  public int CultMemberId { get; set; }
  public int CultId { get; set; }
  public string MemberRole { get; set; }
  public string AccountId { get; set; }
}