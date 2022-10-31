namespace Instacult.Services;

public class CultsService
{
  private readonly CultsRepository _cultsRepo;

  public CultsService(CultsRepository cultsRepo)
  {
    _cultsRepo = cultsRepo;
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


}
