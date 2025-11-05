using Application.Core;
using Application.Interfaces;
using Application.Prtofiles.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles.Queries
{
    public class GetProfile
    {
        public class Query : IRequest<Result<UserProfile>>
        {
            public required string UserId { get; set; }
        }
        public class Handler(IAppDbContext appDbContext, IMapper mapper) : IRequestHandler<Query, Result<UserProfile>>
        {
            public async Task<Result<UserProfile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profile = await appDbContext.Users
                    .ProjectTo<UserProfile>(mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

                return profile == null
                    ? Result<UserProfile>.Failure("Profile not found", (int)HttpStatusCode.NotFound)
                    :Result<UserProfile>.Success(profile);
            }
        }
    }

}
