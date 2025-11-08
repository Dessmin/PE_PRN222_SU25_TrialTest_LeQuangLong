using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTOs
{
    public class LionProfileDto
    {
        [Required]
        public int LionTypeId { get; set; }
        [Required]
        [MinLength(4)]
        public string LionName { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public string Characteristics { get; set; }
        [Required]
        public string Warning { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
