using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule
{
    public class SigninQuery : IRequest<JsonDataResponse<object>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public class SigninQueryHandler : IRequestHandler<SigninQuery, JsonDataResponse<object>>
        {
            readonly SignInManager<AppUser> signInManager;
            readonly UserManager<AppUser> userManager;
            readonly IConfiguration configuration;
            readonly IActionContextAccessor ctx;

            public SigninQueryHandler(SignInManager<AppUser> signInManager,
                UserManager<AppUser> userManager,
                IConfiguration configuration,
                IActionContextAccessor ctx)
            {
                this.signInManager = signInManager;
                this.userManager = userManager;
                this.configuration = configuration;
                this.ctx = ctx;
            }

            public async Task<JsonDataResponse<object>> Handle(SigninQuery request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.UserName))
                {
                    return new JsonDataResponse<object>
                    {
                        Error = true,
                        Message = "İstifadəçi adı göndərilməyib!"
                    };
                }
                else if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return new JsonDataResponse<object>
                    {
                        Error = true,
                        Message = "Şifrə göndərilməyib!"
                    };
                }

                AppUser user = null;

                if (request.UserName.IsEmail())
                {
                    user = await userManager.FindByEmailAsync(request.UserName);
                }
                else
                {
                    user = await userManager.FindByNameAsync(request.UserName);
                }

                if (user == null)
                {
                    return new JsonDataResponse<object>
                    {
                        Error = true,
                        Message = "İstifadəçi adı ve ya şifrə xətalıdır!"
                    };
                }

                var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

                if (result.IsLockedOut)
                {
                    return new JsonDataResponse<object>
                    {
                        Error = true,
                        Message = "Hesabınız keçici olaraq məhdudlaşdırılıb!"
                    };
                }
                else if (result.IsNotAllowed)
                {
                    return new JsonDataResponse<object>
                    {
                        Error = true,
                        Message = "Hesabınız məhdudlaşdırılıb!"
                    };
                }
                else if (result.Succeeded)
                    goto stepGenerage;
                else
                {
                    return new JsonDataResponse<object>
                    {
                        Error = true,
                        Message = "İstifadəçi adı ve ya şifrə xətalıdır!"
                    };
                }

            stepGenerage:
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

                if (!string.IsNullOrWhiteSpace(user.Name) || !string.IsNullOrWhiteSpace(user.Surname))
                {
                    claims.Add(new Claim("FullName", $"{user.Name} {user.Surname}"));
                }
                else if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                {
                    claims.Add(new Claim("FullName", $"{user.PhoneNumber}"));
                }
                else
                {
                    claims.Add(new Claim("FullName", $"{user.Email}"));
                }

                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                string issuer = configuration["jwt:issuer"];
                string audience = configuration["jwt:audience"];
                string secret = configuration["jwt:secret"];

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                DateTime expired = DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["jwt:expireMinutes"]));

                var tokenObj = new JwtSecurityToken(issuer, audience, claims,
                    expires: expired,
                    signingCredentials: creds
                    );

                return new JsonDataResponse<object>
                {
                    Error = false,
                    Message = "Uğurludur!",
                    Data = new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenObj),
                        Expired = expired
                    }
                };
            }
        }
    }
}
