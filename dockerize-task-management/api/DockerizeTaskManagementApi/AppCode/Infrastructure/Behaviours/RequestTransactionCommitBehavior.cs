using DockerizeTaskManagementApi.Models.DataContexts;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Infrastructure.Behaviors
{
    public class RequestTransactionCommitBehavior<TRequest, TResponse>
        : IRequestPostProcessor<TRequest, TResponse>
    {
        private readonly TaskManagementDbContext db;

        public RequestTransactionCommitBehavior(TaskManagementDbContext db)
        {
            this.db = db;
        }

        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            //make log
            if (db.Database.CurrentTransaction != null)
                await db.Database.CurrentTransaction?.CommitAsync(cancellationToken);
        }
    }
}
