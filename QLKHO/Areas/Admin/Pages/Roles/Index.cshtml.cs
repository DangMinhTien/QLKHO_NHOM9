using QLKHO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QLKHO.Areas.Admin.Pages.Roles
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class IndexModel : RolePageModel
    {
        public List<IdentityRole> roles { get; set; }
        public IndexModel(RoleManager<IdentityRole> _rolemanage, AppDbContext _dbcontext): base(_rolemanage, _dbcontext)
        {
            
        }
        public async Task OnGet()
        {
            roles = await rolemanage.Roles.ToListAsync();
        }
        public void OnPost()
        {
           RedirectToPage();
        }
    }
}
