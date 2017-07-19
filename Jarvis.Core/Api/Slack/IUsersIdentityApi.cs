using Jarvis.Core.Models;
using System.Threading.Tasks;

namespace Jarvis.Core.Api.Slack
{
    public interface IUsersIdentityApi
    {
        Task<Identity> GetIdentity(string token);
    }
}