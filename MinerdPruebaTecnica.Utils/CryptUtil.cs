using System;
using System.Collections.Generic;
using System.Text;

namespace MinerdPruebaTecnica.Utils
{
    public static class CryptUtil
    {
        public static string HashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        public static bool VerifyPassword(string password, string passwordHash)
            => BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
