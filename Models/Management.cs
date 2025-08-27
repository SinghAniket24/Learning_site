using System.ComponentModel.DataAnnotations;
using Postgrest.Models;

namespace Learning_site.Models
{
    public class Management : BaseModel
    {
        [Key]
        

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title")]
        public string Title { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Channel cannot exceed 100 characters")]
        public string Channel { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Daily time is required")]
        [DataType(DataType.Time, ErrorMessage = "Please enter a valid time in hours and minutes")]
        public TimeSpan DailyTime { get; set; }

        [Required(ErrorMessage = "Days are required")]
        [Range(1, 365, ErrorMessage = "Days must be between 1 and 365")]
        public int Days { get; set; }


    }
}
