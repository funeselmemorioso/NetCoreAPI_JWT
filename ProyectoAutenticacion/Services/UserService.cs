using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProyectoAutenticacion.Models;
using ProyectoAutenticacion.Requests;
using ProyectoAutenticacion.Responses;
using ProyectoAutenticacion.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ProyectoAutenticacion.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User> { new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" } };

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }         

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
            if (user != null)
            {
                var token = HelperJWT.GenerateToken(user, _appSettings.Secret);
                return new AuthenticateResponse(user, token);
            }           
            return null;
        }        

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }       
    }
}