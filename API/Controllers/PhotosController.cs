using System.Threading.Tasks;
using Application.Photos;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public sealed class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Photo>> Add([FromForm] Add.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("{id}/setMain")]
        public async Task<ActionResult<Unit>> SetMain([FromRoute] string id)
        {
            return await Mediator.Send(new SetMain.Command { Id = id});
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete([FromRoute] string id)
        {
            return await Mediator.Send(new Delete.Command { Id = id });
        }
    }
}