using System;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Authentication;
using BattleshipGame.BLL.DTOs;
using BattleshipGame.BLL.Services.Interfaces;
using BattleshipGame.DAL.Repositories.Interfaces;

namespace BattleshipGame.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IJWTAuthenticationManager _authManager;

        public AuthService(IMapper mapper, IUserRepository userRepository, IJWTAuthenticationManager authManager)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _authManager = authManager;
        }
        public async Task<UserForAuthResponseDTO> Login(UserForLoginDTO user)
        {
            var usr = await _userRepository.GetByLogin(user.Login);
            if(usr == null)
                throw new Exception("User with given login does not exist.");
            if(!usr.Password.Equals(user.Password))
                throw new Exception("Invalid password.");

            var responseUser = _mapper.Map<UserForAuthResponseDTO>(usr);
            var token = _authManager.Authenticate(responseUser.Login, responseUser.ID);
            responseUser.Token = token;
            return responseUser;
        }

        public async Task<UserForAuthResponseDTO> Register(UserForRegisterDTO user)
        {
            var usr = await _userRepository.GetByLogin(user.Login);
            if(usr != null)
                throw new Exception("User with given login already exists.");
            return new UserForAuthResponseDTO();
        }
    }
}