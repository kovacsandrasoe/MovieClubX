namespace MovieClubX.Endpoint.Helpers
{
    public class ErrorModel
    {
        public string Message { get;  }

        public ErrorModel(string message)
        {
            this.Message = message;
        }
    }
}