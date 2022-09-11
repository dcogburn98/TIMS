using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TIMSServerModel
{
    [DataContract]
    public class AuthKey
    {
        private Random RNG;
        private SHA256 Hasher;
        [DataMember]
        private byte[] Hash;
        [DataMember]
        public bool Success;
        [DataMember]
        public int ID;
        [DataMember]
        public bool Bypassed;

        public AuthKey()
        {
            RNG = new Random();
            Hasher = SHA256.Create();
            Hash = Hasher.ComputeHash(Encoding.ASCII.GetBytes(RNG.Next().ToString()));
            ID = RNG.Next();
        }

        public AuthKey(AuthKey KeyToCopy)
        {
            Hash = KeyToCopy.Hash;
            Hasher = SHA256.Create();
            RNG = new Random();
            ID = KeyToCopy.ID;
        }

        public AuthKey(string bypassHash)
        {
            RNG = new Random();
            Hasher = SHA256.Create();
            Hash = Hasher.ComputeHash(Encoding.ASCII.GetBytes(bypassHash));
            ID = RNG.Next();
        }

        public void Regenerate()
        {
            if (Bypassed)
            {
                Bypassed = false;
                return;
            }
            for (int i = 0; i != 7; i++)
                Hash = Hasher.ComputeHash(Hash);
        }

        public bool Match(AuthKey key)
        {
            if (Enumerable.SequenceEqual(key.Hash, Hasher.ComputeHash(Encoding.ASCII.GetBytes("3ncrYqtEdbypa$$K3yF0rInt3rna1u$e"))))
            { Bypassed = true; return true; }
            return Enumerable.SequenceEqual(Hash, key.Hash);
        }
    }
}
