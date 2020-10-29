using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using XPlan.Model;
using XPlan.Model.Plans;
using XPlan.Repository.Abstracts;
using XPlan.Repository.EntityFrameworkCore.Extensions;

namespace XPlan.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private IRepository<Plan> _planRepository;
        private IMapper _mapper;

        public PlanController(IRepository<Plan> planRepository, IMapper mapper)
        {
            _planRepository = planRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PlanQuery query)
        {
            Expression<Func<Plan, bool>> where = r => true;

            if (query.StartTime != null)
            {
                where = where.ExpressionAnd(r => r.CreateTime > query.StartTime);
            }

            if (query.EndTime != null)
            {
                where = where.ExpressionAnd(r => r.CreateTime < query.EndTime.Value.AddDays(1));
            }

            if (query.State != null)
            {
                where = where.ExpressionAnd(r => r.State == query.State);
            }

            var data = await _planRepository.GetListAsync(where, query.Page, query.Size, r => r.CreateTime, OrderByType.Desc);
            return Ok(_mapper.Map<Page<PlanDto>>(data));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlanInput input)
        {
            var plan = new Plan
            {
                CreateTime = DateTime.Now,
                Text = input.Text,
                State = EnumPlanState.Todo
            };
            await _planRepository.AddAsync(plan);

            return CreatedAtAction(nameof(Get), plan.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PlanUpdateInput input)
        {
            var plan = await _planRepository.GetAsync(id);
            if (plan == null)
            {
                return BadRequest("id无效!");
            }

            plan.State = input.State;
            await _planRepository.UpdateAsync(plan);

            return NoContent();
        }
    }
}
