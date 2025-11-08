using BusinessLogic.Interfaces;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LionPetManagement_LeQuangLong.Pages.LionProfilePage
{
    [Authorize(Roles = "2")]

    public class EditModel : PageModel
    {
        private readonly ILionProfileService _context;

        public EditModel(ILionProfileService context)
        {
            _context = context;
        }

        [BindProperty]
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
            LionProfile = lionprofile;
            ViewData["LionTypeId"] = new SelectList(await _context.GetLionTypesAsync(), "LionTypeId", "LionTypeName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Quick validation
            ValidateModel();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.UpdateAsync(LionProfile);

            return RedirectToPage("./Index");
        }

        // Custom validation method

        private void ValidateModel()
        {
            // LionTypeId
            if (LionProfile.LionTypeId < 1)
                ModelState.AddModelError("LionProfile.LionTypeId", "Please select a LionType.");

            // LionName
            if (string.IsNullOrWhiteSpace(LionProfile.LionName))
                ModelState.AddModelError("LionProfile.LionName", "LionName is required.");
            else
            {
                if (LionProfile.LionName.Length < 4)
                    ModelState.AddModelError("LionProfile.LionName", "LionName must be at least 4 characters.");
                // Regex giải thích:
                // ^                     : Bắt đầu chuỗi
                // [A-Z]                 : Ký tự đầu tiên phải là chữ hoa
                // [a-z]*                : Tiếp theo 0 hoặc nhiều chữ thường
                // (?: … )*              : Non-capturing group, lặp lại 0 hoặc nhiều lần
                //    ␣                  : Một dấu cách phân tách các từ
                //    [A-Z]              : Từ sau dấu cách phải bắt đầu bằng chữ hoa
                //    [a-z]*             : Theo sau là 0 hoặc nhiều chữ thường
                // $                     : Kết thúc chuỗi
                if (!System.Text.RegularExpressions.Regex.IsMatch(LionProfile.LionName, @"^[A-Z][a-z]*(?: [A-Z][a-z]*)*$"))
                    ModelState.AddModelError("LionProfile.LionName", "Each word must start with a capital letter and contain only letters and spaces.");
            }

            // Weight
            if (LionProfile.Weight <= 30)
                ModelState.AddModelError("LionProfile.Weight", "Weight must be greater than 30.");

            // Characteristics
            if (string.IsNullOrWhiteSpace(LionProfile.Characteristics))
                ModelState.AddModelError("LionProfile.Characteristics", "Characteristics is required.");

            // Warning
            if (string.IsNullOrWhiteSpace(LionProfile.Warning))
                ModelState.AddModelError("LionProfile.Warning", "Warning is required.");
        }
    }
}
