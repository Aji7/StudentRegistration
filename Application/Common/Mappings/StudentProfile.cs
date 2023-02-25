using AutoMapper;
using StudentRegistration.Application.Students.DTOs;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Common.Mappings;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDto>()
			.ForMember(destination => destination.StudentIDNumber,
					   option => option.MapFrom(source => source.Id));
	}
}
