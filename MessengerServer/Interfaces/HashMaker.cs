namespace MessengerServer.Interfaces
{
    public interface IHashMaker
    {
        bool IsCorrect(string password, User user);

        bool SetNewPassword(string oldPassword, string newPassword, User user);
    }
}