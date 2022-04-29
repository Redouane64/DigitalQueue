using System.Security.Claims;

using DigitalQueue.Web.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Infrastructure;

public class DigitalQueueJwtEvents : JwtBearerEvents
{
    private readonly DigitalQueueContext _context;

    public DigitalQueueJwtEvents(DigitalQueueContext context)
    {
        _context = context;
    }
    
    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var sessionId = context.Principal.FindFirstValue(ClaimTypesDefaults.Session);
        var userId = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

        var isValidSession = await _context.Sessions.AnyAsync(s => s.Id == sessionId && s.UserId == userId);
        if (!isValidSession)
        {
            context.Fail("Invalid session");
        }
        
        context.Success();
    }
}
