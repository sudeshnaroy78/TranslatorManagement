using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TranslationManagement.Api.Service;
using TranslationManagement.Api.Model;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace TranslationManagement.Api.Controllers
{
   
    [ApiController]
    [Route("api/user/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IJobAssignmentService _JobAssignmentService;
        public UserController(IConfiguration configuration, IJobAssignmentService jobAssignmentService)
        {
            _configuration = configuration;
            _JobAssignmentService = jobAssignmentService;
        }
        private Users AuthenticateUser(Users user)
        {
            Users _user = null;
            // futur implementation for fetching credentials from vault
            if (user.username == "admin" && user.password == "password")
            {
                _user = new Users { username = "Sudeshna" };

            }
            return _user;
        }
            private string GenerateToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["JWT:ValidIssuer"], _configuration["JWT:ValidAudience"], null,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users user)
        {
            IActionResult result = Unauthorized();
            var _user = AuthenticateUser(user);
            if (_user != null)
            {
                var token = GenerateToken();
                result = Ok(new { token = token });
            }

            return result;
        }
        [AllowAnonymous]
        [HttpPost]
        public bool JobAssignToCertifiedTranslator(int jobId, int translaotorId)
        {
            bool result =_JobAssignmentService.JobAssignToCertifiedTranslator(jobId, translaotorId);

            return result;
        }
        [HttpGet]
        public List<TranslationJob> GetAllJobAssignedToTranslator(int translatorId)
        {
            var jobs = _JobAssignmentService.GetAllJobAssignedToTranslator(translatorId);
            return jobs;
        }


    }
}

