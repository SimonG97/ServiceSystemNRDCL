namespace ServiceSystemNRDCL.Service
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
        bool IsAdmin();
    }
}