using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace gb_manager.Utils
{
    public class CryptoUtil
    {
        #region ConfiguracaoCriptografia
        private const int SALT_BYTE_SIZE = 24;
        private const int HASH_BYTE_SIZE = 24;
        private const int PBKDF2_ITERATIONS = 1000;

        //private const int ITERATION_INDEX = 0;
        private const int SALT_INDEX = 0;
        private const int PBKDF2_INDEX = 1;
        #endregion

        public string Hash { get; set; }

        public static string CriarHash(string senha)
        {
            using (var csprng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[SALT_BYTE_SIZE];
                csprng.GetBytes(salt);

                //Cria o Hash do password
                var hash = PBKDF2(senha, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
                //return PBKDF2_ITERATIONS + ":" +
                //    Convert.ToBase64String(salt) + ":" +
                //    Convert.ToBase64String(hash);
                return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
            }
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt) { IterationCount = iterations })
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        private static bool SlowEquals(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            var diff = (uint)a.Count ^ (uint)b.Count;
            for (var i = 0; i < a.Count && i < b.Count; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        public bool Equals(string senha)
        {
            char[] delimiter = { ':' };
            var split = Hash.Split(delimiter);
            var iterations = PBKDF2_ITERATIONS;
            var salt = Convert.FromBase64String(split[SALT_INDEX]);
            var hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

            var testHash = PBKDF2(senha, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        public static bool Equals(string hashBase, string senha)
        {
            char[] delimiter = { ':' };
            var split = hashBase.Split(delimiter);

            if (split.Length < 2)
                return false;

            var iterations = PBKDF2_ITERATIONS;
            var salt = Convert.FromBase64String(split[SALT_INDEX]);
            var hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

            var testHash = PBKDF2(senha, salt, iterations, hash.Length);
            var equals = SlowEquals(hash, testHash);
            return equals;
        }

        public static string PasswordGenarator()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");

            Random clsRan = new Random();
            Int32 tamanhoSenha = clsRan.Next(6, 18);

            string senha = string.Empty;
            for (Int32 i = 0; i <= tamanhoSenha; i++)
            {
                senha += guid.Substring(clsRan.Next(1, guid.Length), 1);
            }

            return senha;
        }

        public static int GetRandomNumberFromRange(int intervaloInit, int intervaloFim)
        {
            Random randNum = new Random();
            return randNum.Next(intervaloInit, intervaloFim);
        }
    }
}
