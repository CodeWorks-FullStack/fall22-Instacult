namespace Instacult.Services;

public class CultsService
{
  private readonly CultsRepository _cultsRepo;
  private readonly CultMembersRepository _cmRepo;

  public CultsService(CultsRepository cultsRepo, CultMembersRepository cmRepo)
  {
    _cultsRepo = cultsRepo;
    _cmRepo = cmRepo;
  }


  public List<Cult> GetCults()
  {
    return _cultsRepo.Get();
  }

  public Cult GetCultById(int id)
  {
    var cult = _cultsRepo.GetById(id);
    if (cult == null)
    {
      throw new Exception("Bad cult Id");
    }
    return cult;
  }

  public Cult CreateCult(Cult cultData, string userId)
  {
    cultData.LeaderId = userId;

    var cult = _cultsRepo.Create(cultData);
    // TODO add current user as a cult member....
    var cm = _cmRepo.Create(new FreshMeatDTO()
    {
      AccountId = userId,
      CultId = cult.Id,
      MemberRole = "Leader"
    });

    return cult;
  }


  public void DeleteCult(int cultId, string userId)
  {
    var cult = GetCultById(cultId);
    if (cult.LeaderId != userId)
    {
      throw new Exception("Not Yours....");
    }

    _cultsRepo.Delete(cultId);
  }

  internal List<CultMember> GetCultMembers(int cultId)
  {
    List<CultMember> cultists = _cmRepo.GetCultMembersByCultId(cultId);
    return cultists;
  }

  public Cult UpdateCult(Cult cultData, string userId)
  {
    var originalCult = GetCultById(cultData.Id);
    if (originalCult.LeaderId != userId)
    {
      throw new Exception("Get Out... You have no power here....");
    }

    originalCult.Name = cultData.Name;
    originalCult.Description = cultData.Description;

    originalCult.CoverImg = cultData.CoverImg.Length == 0 ? cultData.CoverImg : originalCult.CoverImg;
    originalCult.Fee = cultData.Fee;

    return _cultsRepo.Update(originalCult);
  }

  internal CultMember JoinCult(int cultId, SecretHandshake handshake, Profile userInfo)
  {
    if (handshake.Handshake != "👋🤚🖐️🖖✌️")
    {
      throw new Exception("🗡️ ---- 🩸 ---- 💀⚰️");
    }

    var cult = GetCultById(cultId);

    var isMember = _cmRepo.Get(userInfo.Id, cultId);

    if (isMember != null)
    {
      throw new Exception("You are already a member of this cult!!!");
    }

    // PROCESS PAYMENT


    var newCultist = new FreshMeatDTO()
    {
      AccountId = userInfo.Id,
      CultId = cultId,
      MemberRole = "Cultist"
    };

    var freshMeat = _cmRepo.Create(newCultist);

    return new CultMember()
    {
      Id = userInfo.Id,
      Bio = userInfo.Bio,
      Name = userInfo.Name,
      Picture = userInfo.Picture,
      CoverImg = userInfo.CoverImg,
      CultId = freshMeat.CultId,
      CreatedAt = freshMeat.CreatedAt,
      UpdatedAt = freshMeat.UpdatedAt,
      MemberRole = freshMeat.MemberRole,
      CultMemberId = freshMeat.CultMemberId,
    };




  }
}
