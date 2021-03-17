
namespace ServiceSystemNRDCL.Models
{
    public class CustomResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object ResponseData { get; set; }

        public CustomResponse() { }

        public CustomResponse(int Status, string Message, object ResponseData)
        {
            this.Status = Status;
            this.Message = Message;
            this.ResponseData = ResponseData;
        }
    }
}
