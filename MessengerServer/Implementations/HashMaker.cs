using System;
using System.Collections.Generic;
using System.Text;
using Argon2id.PasswordHasher; 
using MessengerServer.Interfaces;

namespace MessengerServer.Implementations
{
    internal class HashMaker : IHashMaker
    {
        private readonly Argon2idPasswordHasher _hasher = new();

        public bool IsCorrect(string password, User user)
        {
            return _hasher.VerifyPassword(password, user.Password);
        }

        public bool SetNewPassword(string oldPassword, string newPassword, User user)
        {
            if(_hasher.VerifyPassword(oldPassword, user.Password))
            {
                user.Password = _hasher.HashPassword(newPassword);
                return true;
            }
            return false;
        }
    }
}
