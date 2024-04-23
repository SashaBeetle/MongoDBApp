using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBApp.Infrastructure.Interfaces
{
    public interface INumberHelper
    {
        public Task<string> GetRandomAvatar();

    }
}
