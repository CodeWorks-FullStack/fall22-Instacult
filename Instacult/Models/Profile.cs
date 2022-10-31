namespace Instacult.Models;

public class Profile : DbItem<string>
{
  public string Name { get; set; }
  public string Picture { get; set; }
  public string CoverImg { get; set; }
  public string Bio { get; set; }
}