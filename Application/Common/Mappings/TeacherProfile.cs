using AutoMapper;
using StudentRegistration.Application.Teachers.DTOs;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Common.Mappings;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        CreateMap<Teacher, TeacherDto>()
			.ForMember(destination => destination.TeacherIDNumber,
					   option => option.MapFrom(source => source.Id));
	}
}
