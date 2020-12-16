using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ActivitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ActivitiesController> _logger;

        public ActivitiesController(IMediator mediator, ILogger<ActivitiesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> List()
        {
            return await _mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> Details(Guid id)
        {
            return await _mediator.Send(new Details.Query { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _logger.LogInformation("I'm in create command.");
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task Update(Guid id, Edit.Command command)
        {
            command.Id = id;
            await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _mediator.Send(new Delete.Command { Id = id });
        }
    }
}