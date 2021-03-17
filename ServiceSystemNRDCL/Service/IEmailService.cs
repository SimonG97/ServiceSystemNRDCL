using ServiceSystemNRDCL.Models;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Service
{
    public interface IEmailService
    {
        
        Task SendEmailConfirmation(UserEmailOptions userEmailOptions);
        Task SendForgotPasswordEmail(UserEmailOptions userEmailOptions);
    }
}