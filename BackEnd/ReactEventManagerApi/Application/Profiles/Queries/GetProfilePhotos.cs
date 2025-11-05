using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles.Queries
{
    public class GetProfilePhotos
    {
        public class Query : IRequest<Result<List<Photo>>>
        {
            public required string UserId { get; set; }

        }

        public class Handler(IAppDbContext appDbContext) : IRequestHandler<Query, Result<List<Photo>>>
        {
            public async Task<Result<List<Photo>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var photos = await appDbContext.Users.Where(x => x.Id == request.UserId).SelectMany(x => x.Photos).ToListAsync(cancellationToken);
                return Result<List<Photo>>.Success(photos);
            }
        }
    }
}
