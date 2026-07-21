using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.CQRS.Pharmacy.Commands;

namespace Pharmasy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PharmacyController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PharmacyResponse>> Create(PharmacyRequest request)
    {
        var resourse = await mediator.Send(new CreatePharmacyCommand(request));
        return Ok(resourse);
    }

    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateName([FromBody] string newName)
    {
        var pharmacyId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdatePharmacyNameCommand(pharmacyId, newName));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateAddres([FromBody] string nawAddress)
    {
        var pharmacyId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdatePharmacyAddressCommand(pharmacyId, nawAddress));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateEmail([FromBody] string newEmail)
    {
        var pharmacyId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdatePharmacyEmailCommand(pharmacyId,newEmail));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdatePhoneNumber([FromBody] string nawNumber)
    {
        var pharmacyId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdatePharmacyPhoneNumberCommand(pharmacyId,nawNumber));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<PharmacyResponse>> Update([FromBody] PharmacyRequest request)
    {
        var pharmacyId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdatePharmacyCommand(pharmacyId,request));
        return Ok(response);
    }
}