using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Text;

namespace XPlan.Repository.Abstracts.Plans
{
    public class Plan
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
        public int UserId { get; set; }
        public EnumPlanState State { get; set; }
    }
}
