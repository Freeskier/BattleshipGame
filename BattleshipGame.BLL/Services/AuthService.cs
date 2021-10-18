using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BattleshipGame.BLL.Authentication;
using BattleshipGame.BLL.DTOs;
using BattleshipGame.BLL.Services.Interfaces;
using BattleshipGame.DAL.Entities;
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

            if(!await VerifyPasswordHash(user.Password, usr.PasswordHash, usr.PasswordSalt))
                throw new Exception("Authorization problem.");

            var responseUser = _mapper.Map<UserForAuthResponseDTO>(usr);
            var token = _authManager.Authenticate(responseUser.Login, responseUser.ID);
            responseUser.Token = token;
            return responseUser;
        }

        public async Task Register(UserForRegisterDTO user)
        {
            var usr = await _userRepository.GetByLogin(user.Login);
            if(usr != null)
                throw new Exception("User with given login already exists.");
            
            var userToReg = _mapper.Map<User>(user);
            var (hash, salt) = await CreatePasswordHash(user.Password);
            userToReg.PasswordSalt = salt;
            userToReg.PasswordHash = hash;
            await _userRepository.AddAsync(userToReg);
            await _userRepository.SaveAsync();
        }

        private async Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(passwordSalt))
            {
                var computedHash = await hmac.ComputeHashAsync(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(password)));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        private async Task<(byte[] hash, byte[] salt)> CreatePasswordHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = await hmac.ComputeHashAsync(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(password)));
                return(passwordHash, passwordSalt);
            }
        }
    }
}