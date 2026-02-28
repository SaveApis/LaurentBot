using Backend.Domains.Role.Application.Exceptions;
using Backend.Domains.Role.Application.Mapping;
using Backend.Domains.Role.Application.Models.HTTP.Responses;
using Backend.Domains.Role.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Domains.Role.Application.Controllers;

[ApiController]
[Route("roles")]
public class RolesController(IRoleService service, IRoleMapper mapper) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<RoleResponse>>> Get(CancellationToken cancellationToken = default)
    {
        var roles = await service.GetAllAsync(cancellationToken).ConfigureAwait(false);
        var dtos = mapper.ToResponse(roles);

        return Ok(dtos);
    }

    [HttpGet("by-key/{key}")]
    public async Task<ActionResult<RoleResponse>> GetByKey(string key)
    {
        try
        {
            var role = await service.GetByKeyAsync(key).ConfigureAwait(false);
            var dto = mapper.ToResponse(role);

            return Ok(dto);
        }
        catch (RoleNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }
}
