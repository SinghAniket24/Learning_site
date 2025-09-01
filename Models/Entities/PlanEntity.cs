using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Learning_site.Models.Entities
{
    [Table("plans")] // Table name in Supabase
    public class PlanEntity : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; } // Maps to 'id' in table

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("daily_time")]
        public double DailyTime { get; set; }

        [Column("days")]
        public int Days { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } // Optional
    }
}
