using BusinessLogic.Interfaces;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LionPetManagement_LeQuangLong.Pages.LionProfilePage
{
    [Authorize(Roles = "2")]
    public class CreateModel : PageModel
    {
        private readonly ILionProfileService _context;

        public CreateModel(ILionProfileService context)
        {
            _context = context;
        }

        public SelectList LionTypes { get; set; } = default!;
        public void OnGet()
        {
            var lionTypes = _context.GetLionTypesAsync().Result;
            LionTypes = new SelectList(lionTypes, "LionTypeId", "LionTypeName");
        }

        [BindProperty]
        public LionProfile LionProfile { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var lionTypes = _context.GetLionTypesAsync().Result;
                LionTypes = new SelectList(lionTypes, "LionTypeId", "LionTypeName");
                return Page();
            }

            var lion = new LionProfile
            {
                LionName = LionProfile.LionName,
                Weight = LionProfile.Weight,
                LionTypeId = LionProfile.LionTypeId,
                Characteristics = LionProfile.Characteristics,
                Warning = LionProfile.Warning,
                ModifiedDate = DateTime.Now
            };

            await _context.AddAsync(lion);

            return RedirectToPage("./Index");
        }
    }
}
