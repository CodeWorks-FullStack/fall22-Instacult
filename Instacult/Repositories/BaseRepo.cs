namespace Instacult.Repositories;

public abstract class BaseRepo
{

  protected readonly IDbConnection _db;

  public BaseRepo(IDbConnection db)
  {
    _db = db;
  }

}
