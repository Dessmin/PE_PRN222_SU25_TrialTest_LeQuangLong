using BusinessLogic.Interfaces;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LionPetManagement_LeQuangLong.Pages.LionProfilePage
{
    [Authorize(Roles = "2")]
    public class DeleteModel : PageModel
    {
        private readonly ILionProfileService _lionProfileService;

        public DeleteModel(ILionProfileService lionProfileService)
        {
            _lionProfileService = lionProfileService;
        }

        [BindProperty]
        public LionProfile LionProfile { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lionprofile = await _lionProfileService.GetByIdAsync(id.Value);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lionprofile = await _lionProfileService.GetByIdAsync(id.Value);
            if (lionprofile != null)
            {
                LionProfile = lionprofile;
                await _lionProfileService.DeleteAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
