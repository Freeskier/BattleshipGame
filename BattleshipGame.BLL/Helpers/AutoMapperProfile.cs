using AutoMapper;
using BattleshipGame.BLL.DTOs;
using BattleshipGame.DAL.Entities;

namespace BattleshipGame.BLL.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserForAuthResponseDTO>();
            CreateMap<UserForRegisterDTO, User>();
            CreateMap<UserForLoginDTO, User>();
        }
    }
}