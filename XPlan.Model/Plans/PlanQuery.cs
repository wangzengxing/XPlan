using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XPlan.Model.Plans
{
    public class PlanQuery
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [Range(1, int.MaxValue)]
        public int Page { get; set; }
        [Range(1, int.MaxValue)]
        public int Size { get; set; }
        public EnumPlanState? State { get; set; }
    }
}
