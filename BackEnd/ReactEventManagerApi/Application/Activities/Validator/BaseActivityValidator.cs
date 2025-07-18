using Application.Activities.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Validator
{
    public class BaseActivityValidator<T, TdTo> : AbstractValidator<T> where TdTo : BaseActivityDTO
    {
        public BaseActivityValidator(Func<T, TdTo> func)
        {
            RuleFor(x => func(x).Title).NotEmpty().WithMessage("Title is required").MaximumLength(100).WithMessage("Title must not exceeds 100 characters");
            RuleFor(x => func(x).Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => func(x).Date).GreaterThan(DateTime.UtcNow).WithMessage("Date must be in future");
            RuleFor(x => func(x).Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => func(x).City).NotEmpty().WithMessage("City is required");
            RuleFor(x => func(x).Venue).NotEmpty().WithMessage("Venue is required");
            RuleFor(x => func(x).Latitude).NotEmpty().WithMessage("Latitiue is required").InclusiveBetween(-90, 90).WithMessage("Latitue must be between -90 and 90");
            RuleFor(x => func(x).Longitutde).NotEmpty().WithMessage("Longitute is required").InclusiveBetween(-180, 180).WithMessage("Longitutde must be between -180 and 180");

        }
    }
}
