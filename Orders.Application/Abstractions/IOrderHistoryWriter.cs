using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Abstractions
{
    public interface IOrderHistoryWriter
    {
        Task WriteAsync(object record, CancellationToken ct);
    }
}
