using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Learning_site.Models.Entities
{
    [Table("daily_plans")] // Table name in Supabase
    public class DailyPlanEntity : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("plan_id")]
        public int PlanId { get; set; }

        [Column("day")]
        public int Day { get; set; }

        [Column("videos")]
        public string Videos { get; set; }
    }
}