using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XPlan.Model;
using XPlan.Repository.Abstracts;

namespace XPlan.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private IRepository<Plan> _planRepository;

        public PlanController(IRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        public class PlanResult
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public DateTime CreateTime { get; set; }
            public string UserName { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _planRepository.FromSqlAsync<PlanResult>("select r.Id,r.Text,r.CreateTime,t.Name As UserName from `plan` As r,`user` As t where r.UserId=t.Id");
            return Ok(data);
        }
    }
}
