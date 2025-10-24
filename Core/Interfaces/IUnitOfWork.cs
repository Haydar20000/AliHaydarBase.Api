using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository User { get; }
        int Complete();
    }
}