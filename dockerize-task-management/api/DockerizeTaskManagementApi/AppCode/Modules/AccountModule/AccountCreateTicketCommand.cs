using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule
{
    public class AccountCreateTicketCommand : IRequest<JsonResponse>
    {
        public string OrganisationName { get; set; }
        public string OrganisationPhoneNumber { get; set; }
        public string OrganisationAddress { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }


        public class AccountCreateTicketCommandHandler : IRequestHandler<AccountCreateTicketCommand, JsonResponse>
        {
            readonly TaskManagementDbContext db;
            readonly UserManager<AppUser> userManager;
            readonly IActionContextAccessor ctx;
            readonly IConfiguration configuration;

            public AccountCreateTicketCommandHandler(TaskManagementDbContext db, UserManager<AppUser> userManager, IActionContextAccessor ctx, IConfiguration configuration)
            {
                this.db = db;
                this.ctx = ctx;
                this.userManager = userManager;
                this.configuration = configuration;
            }

            public async Task<JsonResponse> Handle(AccountCreateTicketCommand request, CancellationToken cancellationToken)
            {
                int currentUserId = ctx.GetPrincipalId().Value;

                var user = await userManager.FindByEmailAsync(request.Email);

                if (user != null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Bu e-poçt artıq istifadə edilib."
                    };
                }

                user = await userManager.FindByNameAsync(request.UserName);

                if (user != null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Bu istifadəçi adı artıq istifadə edilib."
                    };
                }

                var currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

                user = new AppUser
                {
                    UserName = request.UserName,
                    Email = request.Email,

                };

                // phone to regular format
                request.OrganisationPhoneNumber = request.OrganisationPhoneNumber.ClearPhone();

                if (currentUser?.OrganisationId == null)//oz organisationu yoxdusa emeliyyati icra eden istifadecinin
                {
                    Organisation organisation = await db.Organisations.FirstOrDefaultAsync(o => o.PhoneNumber.Equals(request.OrganisationPhoneNumber), cancellationToken);

                    if (organisation == null)
                    {
                        organisation = new Organisation
                        {
                            Name = request.OrganisationName,
                            PhoneNumber = request.OrganisationPhoneNumber,
                            Address = request.OrganisationAddress
                        };

                        db.Organisations.Add(organisation);
                        await db.SaveChangesAsync(cancellationToken);
                    }

                    user.OrganisationId = organisation.Id;
                }
                else //cari istifadecinin oldugu organisationa elave edilir
                {
                    user.OrganisationId = currentUser.OrganisationId;
                }

                string temporaryPassword = Guid.NewGuid().ToString().Replace("-", "").ToLower();

                var createUserResult = await userManager.CreateAsync(user, temporaryPassword);

                if (!createUserResult.Succeeded)
                {
                    db.Database.CurrentTransaction?.Rollback();

                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Yeni istifadəçi yaradılarkən xəta baş verdi zəhmət olmasa birazdan yenidən cəht edin."
                    };
                }

                #region AddToRole
                if (await userManager.IsInRoleAsync(currentUser, "SuperAdmin"))
                {
                    await userManager.AddToRoleAsync(user, "OrganisationAdmin");
                }
                else if (await userManager.IsInRoleAsync(currentUser, "OrganisationAdmin"))
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
                #endregion

                string complateToken = WebUtility.UrlEncode($"{user.Id}#{user.Email}#{temporaryPassword}#{ctx.GetPrincipalId()}".Encrypt());

                string link = $"{configuration["frontEnd:domain"]}/complate-signup?token={complateToken}";

                var emailResult = await configuration.SendEmailAsync($"{configuration["emailAccount:displayName"]} Registration", $"Zəhmət olmasa <a href=\"{link}\">linkə daxil olub</a> qeydiyyatınızı tamamlayın", user.Email);

                if (emailResult.Item1)// error occured or not
                {
                    db.Database.CurrentTransaction?.Rollback();

                    return new JsonResponse
                    {
                        Error = true,
                        Message = "E-poçt göndərərkən xəta baş verdi zəhmət olmasa birazdan yenidən cəht edin."
                    };
                }

                return new JsonResponse
                {
                    Error = false,
                    Message = "E-poçt addresinizə göndərilən linklə qeydiyyatınızı tamamlaya bilərsiniz"
                };
            }
        }
    }
}
