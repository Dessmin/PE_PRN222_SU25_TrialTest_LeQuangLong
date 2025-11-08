using BusinessLogic.Interfaces;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LionPetManagement_LeQuangLong.Pages.LionProfilePage
{
    [Authorize(Roles = "2, 3")]

    public class DetailsModel : PageModel
    {
        private readonly ILionProfileService _context;

        public DetailsModel(ILionProfileService context)
        {
            _context = context;
        }

        public LionProfile LionProfile { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lionprofile = await _context.GetByIdAsync(id.Value);
            if (lionprofile == null)
            {
                return NotFound();
            }
            else
            {
                LionProfile = lionprofile;
            }
            return Page();
        }
    }
}
