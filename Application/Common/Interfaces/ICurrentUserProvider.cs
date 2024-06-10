using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Application.Common.Interfaces
{
    public interface ICurrentUserProvider
    {
        Guid? UserId { get; }
        string? UserName { get; }
    }
}
