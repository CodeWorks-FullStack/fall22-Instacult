namespace Instacult.Repositories;

public class CultsRepository : BaseRepo, IRepository<Cult, int>
{
  public CultsRepository(IDbConnection db) : base(db)
  {
  }

  public Cult Create(Cult cult)
  {
    var sql = @"
      INSERT INTO cults(
        name, fee, description, coverImg, leaderId
      )
      VALUES(
        @Name, @Fee, @Description, @CoverImg, @LeaderId
      );
      SELECT LAST_INSERT_ID()
    ;";
    cult.CreatedAt = DateTime.Now;
    cult.UpdatedAt = DateTime.Now;
    cult.Id = _db.ExecuteScalar<int>(sql, cult);
    return cult;
  }

  public void Delete(int id)
  {
    var sql = "DELETE FROM cults WHERE id = @id LIMIT 1;";
    var rows = _db.Execute(sql, new { id });
    if (rows != 1) { throw new Exception("Uh-ooooo..... something bad happened the data is borked... or the id was bad"); }
    return;
  }

  public List<Cult> Get()
  {
    var sql = @"
      SELECT
        c.*,
        COUNT(cm.id) AS MemberCount,
        a.*
        FROM cults c
            JOIN accounts a ON a.id = c.leaderId
            LEFT JOIN cult_members cm ON cm.cultId = c.id
        GROUP BY c.id
    ;";

    return _db.Query<Cult, Profile, Cult>(sql, (cult, profile) =>
    {
      cult.Leader = profile;
      return cult;
    }).ToList();

  }

  public Cult GetById(int id)
  {
    var sql = @"
    SELECT
        c.*,
        COUNT(cm.id) AS MemberCount,
        a.*
        FROM cults c
            JOIN accounts a ON a.id = c.leaderId
            LEFT JOIN cult_members cm ON cm.cultId = c.id
            WHERE c.id = @id
        GROUP BY c.id
    
    ;";
    return _db.Query<Cult, Profile, Cult>(sql, (cult, profile) =>
    {
      cult.Leader = profile;
      return cult;
    }, new { id }).FirstOrDefault();

  }

  public Cult Update(Cult data)
  {
    var sql = @"
      UPDATE cults SET
        name = @Name,
        description = @Description,
        fee = @Fee, 
        coverImg = @CoverImg,
        leaderId = @LeaderId
      WHERE id = @Id LIMIT 1;
    ";
    var rows = _db.Execute(sql, data);
    if (rows < 1)
    {
      throw new Exception("changes not saved");
    }
    if (rows > 1)
    {
      throw new Exception("Call the DBA something is borked....");
    }
    return data;
  }
}
