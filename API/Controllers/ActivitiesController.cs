using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public sealed class ActivitiesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ActivityDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task Update(Guid id, Edit.Command command)
        {
            command.Id = id;
            await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await Mediator.Send(new Delete.Command { Id = id });
        }

        [HttpPost("{id}/attend")]
        public async Task Attend(Guid id)
        {
            await Mediator.Send(new Attend.Command { ActivityId = id });
        }

        [HttpDelete("{id}/attend")]
        public async Task Unattend(Guid id)
        {
            await Mediator.Send(new Unattend.Command { ActivityId = id });
        }
    }
}