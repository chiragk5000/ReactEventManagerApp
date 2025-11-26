using Application.Activities.DTO;
using Application.Profiles.DTOs;
using Application.Prtofiles.DTOs;
using AutoMapper;
using Domain.Entities;
using ReactEventManagerApi.DTOs;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string? currentUserId = null;
            CreateMap<Activity, Activity>();
            CreateMap<CreateActivityDTO, Activity>();
            CreateMap<EditActivityDTO, Activity>();
            CreateMap<Activity, ActivityDTO>()
                .ForMember(d => d.Attendees, o => o.MapFrom(s => s.Attendees))
                .ForMember(d => d.HostDisplayName, o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.IsHost)!.User.DisplayName)) // for manual  mapping 
                .ForMember(d => d.HostId, o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.IsHost)!.User.Id));

            CreateMap<ActivityAttendee, UserProfile>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.User.Bio))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.User.Id))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl))

                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.User.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.User.Followings.Count))
                 .ForMember(d => d.Following, o => o.MapFrom(s => s.User.Followers.Any(x => x.Follower.Id == currentUserId)));


            CreateMap<User, UserProfile>()
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.Followers.Any(x => x.Follower.Id == currentUserId)));



            CreateMap<Comment, CommentDTO>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl));

            CreateMap<Activity, UserActivityDTO>();

        }
    }
}
