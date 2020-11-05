using System;
using System.Collections.Generic;
using System.Text;

namespace XPlan.Repository.Abstracts.Plans
{
    public class PlanDto
    {
        public int Id { get; set; }
        public string CreateTime { get; set; }
        public string Text { get; set; }
        public EnumPlanState State { get; set; }
    }
}
