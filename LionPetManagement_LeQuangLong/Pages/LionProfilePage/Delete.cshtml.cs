using BusinessLogic.Interfaces;
using BusinessObject.Models;
using LionPetManagement_LeQuangLong.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace LionPetManagement_LeQuangLong.Pages.LionProfilePage
{
    [Authorize(Roles = "2")]
    public class DeleteModel : PageModel
    {
        private readonly ILionProfileService _lionProfileService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public DeleteModel(ILionProfileService lionProfileService, IHubContext<SignalRHub> hubContext)
        {
            _lionProfileService = lionProfileService;
            _hubContext = hubContext;
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
                await _hubContext.Clients.All.SendAsync("ReceiveDelete");
            }

            return RedirectToPage("./Index");
        }
    }
}
