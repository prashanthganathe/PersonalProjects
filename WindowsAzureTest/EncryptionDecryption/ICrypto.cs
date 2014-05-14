using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionDecryption
{
    interface ICrypto
    {
        string Encrypt(string toEncrypt, bool useHashing);

        string Decrypt(string cipherString, bool useHashing);
    }
}
