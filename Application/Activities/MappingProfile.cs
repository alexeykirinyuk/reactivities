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
                .ForMember(ua => ua.Username, opts => opts.MapFrom(a => a.AppUser.UserName))
                .ForMember(ua => ua.DisplayName, opts => opts.MapFrom(a => a.AppUser.DisplayName));
        }
    }
}