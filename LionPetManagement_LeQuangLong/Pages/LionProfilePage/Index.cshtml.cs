using BusinessLogic.Interfaces;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LionPetManagement_LeQuangLong.Pages.LionProfilePage
{
    [Authorize(Roles = "2, 3")]
    public class IndexModel : PageModel
    {
        private readonly ILionProfileService _lionProfileService;

        public IndexModel(ILionProfileService lionProfileService)
        {
            _lionProfileService = lionProfileService;
        }

        public IList<LionProfile> Items { get; set; } = new List<LionProfile>();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Weight { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        private const int PageSize = 3;
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            PageIndex = pageIndex ?? 1;

            var (items, totalCount) = await _lionProfileService.GetAllAsync(PageIndex, PageSize, SearchString, Weight);

            Items = items;
            TotalCount = totalCount;
            return Page();
        }
    }
}
