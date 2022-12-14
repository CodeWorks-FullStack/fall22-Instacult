namespace Instacult.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CultsController : ControllerBase
{
  private readonly CultsService _cs;
  private readonly Auth0Provider _a0;

  public CultsController(CultsService cs, Auth0Provider auth0Provider)
  {
    _cs = cs;
    _a0 = auth0Provider;
  }

  [HttpGet]
  public ActionResult<List<Cult>> Get([FromQuery] string offset, [FromQuery] string take)
  {
    try
    {
      var cults = _cs.GetCults();
      return Ok(cults);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("{id}")]
  public ActionResult<Cult> GetById(int id)
  {
    try
    {
      var cult = _cs.GetCultById(id);
      return Ok(cult);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }


  [HttpGet("{cultId}/members")]
  public ActionResult<List<CultMember>> GetCultMembersByCultId(int cultId)
  {
    try
    {
      List<CultMember> cultists = _cs.GetCultMembers(cultId);
      return Ok(cultists);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }


  #region Authorized Routes


  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Cult>> Create([FromBody] Cult cultData)
  {
    try
    {
      var userInfo = await _a0.GetUserInfoAsync<Account>(HttpContext);
      var cult = _cs.CreateCult(cultData, userInfo.Id);
      return Ok(cult);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [Authorize]
  [HttpPost("{cultId}/members")]
  public async Task<ActionResult<CultMember>> JoinCult([FromBody] SecretHandshake handshake, int cultId)
  {
    try
    {
      Profile userInfo = await _a0.GetUserInfoAsync<Profile>(HttpContext);
      CultMember cultist = _cs.JoinCult(cultId, handshake, userInfo);
      return Ok(cultist);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [Authorize]
  [HttpPut("{id}")]
  public async Task<ActionResult<Cult>> UpdateCult([FromBody] Cult cultData, int id)
  {
    try
    {
      var userInfo = await _a0.GetUserInfoAsync<Account>(HttpContext);
      var cult = _cs.UpdateCult(cultData, userInfo.Id);
      return Ok(cult);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [Authorize]
  [HttpDelete("{cultId}")]
  public async Task<ActionResult<string>> DeleteCult(int cultId)
  {
    try
    {
      var userInfo = await _a0.GetUserInfoAsync<Account>(HttpContext);
      _cs.DeleteCult(cultId, userInfo.Id);
      return Ok("Cult Deleted");
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  #endregion
}
