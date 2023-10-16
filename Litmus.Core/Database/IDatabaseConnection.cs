using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Litmus.Core.Database
{
    public interface IDatabaseConnection
    {
        Task<DbConnection> OpenConnection(CancellationToken cancellationToken);
    }
}
