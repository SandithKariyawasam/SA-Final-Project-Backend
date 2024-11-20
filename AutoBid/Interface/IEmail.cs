namespace Authentication.Interface
{
    public interface IEmail
    {
        Task SendTestEmailAsync(string reciverName, string reciverEmail, string reciverMessage);
    }
}
