using System.Linq;
using AutoMapper;
using Domain;

namespace Application.Activities
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile() : base()
        {
            CreateMap<Activity, ActivityDto>()
                .ForMember(a => a.Attendees, opts => opts.MapFrom(a => a.UserActivities));
            CreateMap<UserActivity, AttendeeDto>()
                .ForMember(ua => ua.Username, o => o.MapFrom(a => a.AppUser.UserName))
                .ForMember(ua => ua.DisplayName, o => o.MapFrom(a => a.AppUser.DisplayName))
                .ForMember(
                    ua => ua.Image,
                    o => o.MapFrom(a => a.AppUser.Photos.FirstOrDefault(u => u.IsMain).Url));
        }
    }
}