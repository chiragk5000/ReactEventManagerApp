using Application.Activities.Command;
using Application.Activities.DTO;
using FluentValidation;

namespace Application.Activities.Validator
{
    public class CreateActivityValdiator : BaseActivityValidator<CreateActivity.Command,CreateActivityDTO>
    {
        public CreateActivityValdiator():base(x=>x.ActivityDto)
        {
            
        }
    }
}
