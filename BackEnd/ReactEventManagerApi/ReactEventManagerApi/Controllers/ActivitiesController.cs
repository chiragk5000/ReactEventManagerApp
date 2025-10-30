using Application.Activities.Command;
using Application.Activities.DTO;
using Application.Activities.Queries;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactEventManagerApi.DTOs;

namespace ReactEventManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : BaseApiController
    {
        private readonly AppDbContext _context;
        //private readonly IMediator _mediator;

        public ActivitiesController(AppDbContext context)
        {
            _context = context;
           // _mediator = mediator;
        }

        // GET: api/Activities
        [HttpGet]

        public async Task<ActionResult<IEnumerable<ActivityDTO>>> GetActivities()
        {
            return await Mediator.Send(new GetActivityList.Query());
        }

        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDTO>> GetActivity(string id)
        {
            return HandleResult(await Mediator.Send(new GetActivityDetails.Query { Id=id}));
        }

        // PUT: api/Activities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(string id, Activity activity)
        {
            if (id != activity.Id)
            {
                return BadRequest();
            }

            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Activities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(Activity activity)
        {
            _context.Activities.Add(activity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ActivityExists(activity.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<string>> CreateActivity(CreateActivityDTO activitydto)
        {
            return HandleResult(await Mediator.Send(new CreateActivity.Command { ActivityDto = activitydto }));

        }

        [HttpPut("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task <ActionResult<Activity>> EditActivty(string id,EditActivityDTO activity)
        {
            activity.Id = id;
             return HandleResult(await Mediator.Send(new EditActivity.Command { ActivityDto=activity}));
        }
        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<IActionResult> DeleteActivity(string id)
        {
            return HandleResult(await Mediator.Send(new DeleteActivity.Command { Id = id }));
        }

        /*
        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(string id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */

        private bool ActivityExists(string id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }

        [HttpPost("{id}/attend")]
        public async Task<ActionResult> Attend(string id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command { Id = id }));
        }

    }
}
