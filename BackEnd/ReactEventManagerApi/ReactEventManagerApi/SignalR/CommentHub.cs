using Application.Activities.Command;
using Application.Activities.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ReactEventManagerApi.SignalR
{
   
    public class CommentHub(IMediator mediator) :Hub
    {
        public async Task SendComment(AddComment.Command command)
        {
            var comment = await mediator.Send(command);
            await Clients.Group(command.ActivityId).SendAsync("ReciveComment", comment.Value);
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext?.Request.Query["activityId"];
            if (string.IsNullOrEmpty(activityId))
            {
                throw new HubException("No acitivity with this id ");

                    
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId!);
            var result = await mediator.Send(new GetComments.Query { ActivityId = activityId! });
            await Clients.Caller.SendAsync("LoadComments", result.Value);

        }

    }
}
