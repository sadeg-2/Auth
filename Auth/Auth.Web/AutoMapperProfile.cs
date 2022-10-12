using Auth.Web.Models;
using Auth.Web.ViewModel;
using AutoMapper;

namespace Auth.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {

            CreateMap<User, UserViewModel>().ForMember(userviewModel => userviewModel.Email , x => x.MapFrom(user => user.Email) /*x.Ignore()*/);
            CreateMap<User, CreateUserViewModel>();

        }
    }
}
