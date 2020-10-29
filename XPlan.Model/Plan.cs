using System;
using System.Collections.Generic;
using System.Text;

namespace XPlan.Model
{
    public class Plan
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
        public int UserId { get; set; }
    }
}
