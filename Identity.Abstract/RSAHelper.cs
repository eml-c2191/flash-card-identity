using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Abstract
{
    public static class RSAHelper
    {
        public static RSACryptoServiceProvider PrivateKeyFromPemFile(string filePath)
        {
            using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                var data = new PemReader(privateKeyTextReader).ReadObject();

                RsaPrivateCrtKeyParameters privateKeyParams;
                if (data is RsaPrivateCrtKeyParameters tempPrivateKeyParams)
                {
                    privateKeyParams = tempPrivateKeyParams;
                }
                else if (data is AsymmetricCipherKeyPair readKeyPair)
                {
                    privateKeyParams = ((RsaPrivateCrtKeyParameters)readKeyPair.Private);
                }
                else
                {
                    throw new Exception("Invalid RSA secret key");
                }

                RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
                RSAParameters parms = new RSAParameters();

                parms.Modulus = privateKeyParams.Modulus.ToByteArrayUnsigned();
                parms.P = privateKeyParams.P.ToByteArrayUnsigned();
                parms.Q = privateKeyParams.Q.ToByteArrayUnsigned();
                parms.DP = privateKeyParams.DP.ToByteArrayUnsigned();
                parms.DQ = privateKeyParams.DQ.ToByteArrayUnsigned();
                parms.InverseQ = privateKeyParams.QInv.ToByteArrayUnsigned();
                parms.D = privateKeyParams.Exponent.ToByteArrayUnsigned();
                parms.Exponent = privateKeyParams.PublicExponent.ToByteArrayUnsigned();

                cryptoServiceProvider.ImportParameters(parms);

                return cryptoServiceProvider;
            }
        }
    }
}
