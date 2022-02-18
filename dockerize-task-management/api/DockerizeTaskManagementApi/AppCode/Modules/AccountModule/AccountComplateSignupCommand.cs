using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule
{
    public class AccountComplateSignupCommand : IRequest<JsonResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }


        public class AccountComplateSignupCommandHandler : IRequestHandler<AccountComplateSignupCommand, JsonResponse>
        {
            readonly TaskManagementDbContext db;
            readonly IActionContextAccessor ctx;
            readonly UserManager<AppUser> userManager;
            readonly IConfiguration configuration;

            public AccountComplateSignupCommandHandler(TaskManagementDbContext db, UserManager<AppUser> userManager, IActionContextAccessor ctx, IConfiguration configuration)
            {
                this.db = db;
                this.ctx = ctx;
                this.userManager = userManager;
                this.configuration = configuration;
            }

            public async Task<JsonResponse> Handle(AccountComplateSignupCommand request, CancellationToken cancellationToken)
            {
                var signupToken = ctx.GetHeaderValue("signupToken")?.Decrypt();

                if (string.IsNullOrWhiteSpace(signupToken))
                    goto invalidToken;

                var tokenMatchResult = signupToken.ReleaseComplateSignupToken();

                if (!tokenMatchResult.Success)
                    goto invalidToken;

                int userId = tokenMatchResult.Read<int>("id");
                string email = tokenMatchResult.Read<string>("email");
                string currentPassword = tokenMatchResult.Read<string>("pwd");

                if (!email.IsEmail() || !email.Equals(request.Email))
                    goto invalidToken;

                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.Email.Equals(request.Email), cancellationToken);

                if (user == null)
                    goto invalidToken;

                if (user.EmailConfirmed)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Bu hesab artıq təsdiqlənib"
                    };
                }

                var passwordChange = await userManager.ChangePasswordAsync(user, currentPassword, request.Password);

                if (!passwordChange.Succeeded)
                    return new JsonResponse
                    {
                        Error = true,
                        Message = passwordChange.Errors.First().Description
                    };

                user.EmailConfirmed = true;

                await db.SaveChangesAsync(cancellationToken);


                return new JsonResponse
                {
                    Error = false,
                    Message = "Şifrəniz uğurla dəyişdirildi.Hesabınız artıq aktivdir."
                };

            invalidToken:   //INVALID TOKEN LABEL
                return new JsonResponse
                {
                    Error = true,
                    Message = "Xətalı token."
                };
            }
        }
    }
}
