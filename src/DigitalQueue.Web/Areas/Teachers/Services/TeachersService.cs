using System.Security.Claims;

using DigitalQueue.Web.Areas.Teachers.Dtos;
using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Teachers.Services;

public class TeachersService
{

    public TeachersService()
    {
    }

    public async Task<TeacherSearchResult> Search(string query)
    {
        return new TeacherSearchResult(new[]
        {
            new { text = "Jack", id="33D2C7A7-CF3E-4DFB-9922-D7ABC4052BE1" }, 
            new { text = "Karim", id="63E0C9D4-D33C-4A09-BD56-0DB17F4236E6" }, 
            new { text = "Joe", id="BB56761A-B815-4450-81A6-356511C31DF8" }, 
        }, new {more = false});
    }
}
