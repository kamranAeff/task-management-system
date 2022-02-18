using DockerizeTaskManagementApi.Models.DataContexts;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Infrastructure.Behaviors
{
    public class RequestTransactionBeginBehavior<TRequest>
        : IRequestPreProcessor<TRequest>
    {
        private readonly TaskManagementDbContext db;

        public RequestTransactionBeginBehavior(TaskManagementDbContext db)
        {
            this.db = db;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (db.Database.CurrentTransaction != null)
                await db.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}
