using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace TIMSServerModel
{
    
    public class AuthorizationKey
    {
        private Random RNG;
        private SHA256 Hasher;
        private byte[] Hash;
        public bool Success;
        public int ID;

        public AuthorizationKey()
        {
            RNG = new Random();
            Hasher = SHA256.Create();
            Hash = Hasher.ComputeHash(Encoding.ASCII.GetBytes(RNG.Next().ToString()));
            ID = RNG.Next();
        }

        private AuthorizationKey(bool bypass)
        {
            Hash = Hasher.ComputeHash(Encoding.ASCII.GetBytes("3ncrYqtEdbypa$$K3yF0rInt3rna1u$e"));
            ID = RNG.Next();
        }

        public void Regenerate()
        {
            for (int i = 0; i != 7; i++)
                Hash = Hasher.ComputeHash(Hash);
        }

        public bool Match(AuthorizationKey key)
        {
            return Hash == key.Hash || key.Hash == Hasher.ComputeHash(Encoding.ASCII.GetBytes("3ncrYqtEdbypa$$K3yF0rInt3rna1u$e"));
        }

        internal static AuthorizationKey CreateBypassKey()
        {
            return new AuthorizationKey(true);
        }
    }
}
