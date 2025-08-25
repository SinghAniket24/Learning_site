using System.ComponentModel.DataAnnotations;

namespace Learning_site.Models
{
    public class management
    {
        [Key]
        

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [StringLength(100, ErrorMessage = "Channel cannot exceed 100 characters")]
        public string Channel { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Daily time is required")]
        [DataType(DataType.Time, ErrorMessage = "Please enter a valid time in hours and minutes")]
        public TimeSpan DailyTime { get; set; }

        [Required(ErrorMessage = "Days are required")]
        [Range(1, 365, ErrorMessage = "Days must be between 1 and 365")]
        public int Days { get; set; }


    }
}
