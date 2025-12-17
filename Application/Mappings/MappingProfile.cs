using AutoMapper;
using LearnASP.Application.DTOs.Authors;
using LearnASP.Application.DTOs.Books;
using LearnASP.Domain.Entities;

namespace LearnASP.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ConfigureBookMappings();
            ConfigureAuthorMappings();
        }

        private void ConfigureBookMappings()
        {
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => src.AuthorId == 0 || src.Author == null
                    ? "Unknown Author"
                    : src.Author.FullName ?? "Unknown Author"));
            CreateMap<CreateBookRequest, Book>();
            CreateMap<UpdateBookRequest, Book>();
        }   

        private void ConfigureAuthorMappings()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<CreateAuthorRequest, Author>();
            CreateMap<UpdateAuthorRequest, Author>();
        }
    }
}