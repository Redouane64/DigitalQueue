using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Common;

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
        var session = context.Principal.FindFirstValue(ClaimTypesDefaults.Session);
        var user = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

        var isValidSession = await _context.Sessions.AnyAsync(s => s.SecurityStamp == session && s.UserId == user);
        if (!isValidSession)
        {
            context.Fail("Invalid access token");
            return;
        }

        context.Success();
    }
}
