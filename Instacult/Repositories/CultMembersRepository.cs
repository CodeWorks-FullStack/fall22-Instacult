namespace Instacult.Repositories;

public class CultMembersRepository : BaseRepo, IRepository<FreshMeatDTO, int>
{
  public CultMembersRepository(IDbConnection db) : base(db)
  {
  }

  public FreshMeatDTO Create(FreshMeatDTO data)
  {
    var sql = @"
      INSERT INTO cult_members(
        accountId,
        cultId,
        member_role
      )
      VALUES(
        @AccountId,
        @CultId,
        @MemberRole
      );
      SELECT LAST_INSERT_ID()
    ;";
    data.CreatedAt = DateTime.Now;
    data.UpdatedAt = DateTime.Now;
    data.CultMemberId = _db.ExecuteScalar<int>(sql, data);
    return data;
  }

  public void Delete(int id)
  {
    var sql = "DELETE FROM cult_members WHERE id = @id;";
    _db.Execute(sql, new { id });
  }

  public List<FreshMeatDTO> Get()
  {
    var sql = @"
      SELECT * FROM cult_members 
    ;";
    return _db.Query<FreshMeatDTO>(sql).ToList();
  }

  public FreshMeatDTO GetById(int cultMemberId)
  {
    var sql = @"
      SELECT * FROM cult_members WHERE id = @cultMemberId
    ;";
    return _db.Query<FreshMeatDTO>(sql, new { cultMemberId }).FirstOrDefault();
  }

  public FreshMeatDTO Update(FreshMeatDTO data)
  {
    var sql = @"
      UPDATE cult_members SET
        member_role = @MemberRole
      WHERE id = @CultMemberId
    ;";
    _db.Execute(sql, data);
    return data;
  }

  internal List<CultMember> GetCultMembersByCultId(int cultId)
  {
    var sql = @"
      SELECT 
        cm.cultId,
        cm.member_role AS MemberRole,
        cm.id AS CultMemberId,
        a.* 
      FROM cult_members cm
      JOIN accounts a ON a.id = cm.accountId
      WHERE cm.cultId = @cultId
    ;";

    return _db.Query<CultMember>(sql, new { cultId }).ToList();
  }

  internal List<FreshMeatDTO> Get(string accountId)
  {
    var sql = @"
      SELECT * FROM cult_members WHERE accountId = @accountId
    ;";
    return _db.Query<FreshMeatDTO>(sql, new { accountId }).ToList();
  }

  internal FreshMeatDTO Get(string accountId, int cultId)
  {
    var sql = @"
      SELECT * FROM cult_members WHERE accountId = @accountId AND cultId = @cultId
    ;";
    return _db.Query<FreshMeatDTO>(sql, new { accountId, cultId }).FirstOrDefault();
  }
}