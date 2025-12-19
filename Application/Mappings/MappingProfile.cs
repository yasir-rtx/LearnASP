using AutoMapper;
using LearnASP.Application.DTOs.Authors;
using LearnASP.Application.DTOs.Books;
using LearnASP.Application.DTOs.Categories;
using LearnASP.Domain.Entities;

namespace LearnASP.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ConfigureBookMappings();
            ConfigureAuthorMappings();
            ConfigureCategoryMappings();
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

        private void ConfigureCategoryMappings()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
        }
    }
}