using System.Threading.Tasks;

namespace Kubectl.Web.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string from, string to,string subject, string body);
    }
}