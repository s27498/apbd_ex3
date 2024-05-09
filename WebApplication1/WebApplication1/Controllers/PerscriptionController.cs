
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PerscriptionController : ControllerBase
{
    private readonly PerscriptionRepository _perscriptionRepository;
    
    public PerscriptionController(PerscriptionRepository perscriptionRepository)
    {
        _perscriptionRepository = perscriptionRepository;
    }
    [HttpGet]
    //[Route("{doctorLast}")]
    public async Task<IActionResult> GetPerscription([FromQuery]string? doctorLast)
    {

        List<Perscription> perscription;
        if (doctorLast is null)
        {
            perscription = await _perscriptionRepository.getPerscription();
        }
        else
        {
            if (!await _perscriptionRepository.DoesDoctorExist(doctorLast))
            {
                return NotFound("Doctor not found!");
            }
            perscription = await _perscriptionRepository.getPerscription(doctorLast);
        }
        return Ok(perscription);
    }
    [HttpPost]
    public async Task<IActionResult> AddPerscription([FromBody]NewPerscription newPerscription)
    {
        if (newPerscription.DueDate <= newPerscription.Date)
        {
            return BadRequest("Due date is later than date");
        }
        await _perscriptionRepository.addPerscription(newPerscription);
        return Ok();
    }
}