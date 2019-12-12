using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SubmitBug.BLL
{
    public class MD5DataEncryption
    {
        private SymmetricAlgorithm mCSP;
        private const string CIV = "TafasMM=";//密钥
        private const string CKEY = "TOdiaMM=";//初始化向量
        /// <summary>
        ///加密字符串
        /// </summary>
        /// <param name="Value">要加密的字符</param>
        /// <returns>加密了的字符串</returns>
        public string EncryptString(string Value)
        {
            ICryptoTransform ct;//定义基本的加密运算符
            MemoryStream ms;
            CryptoStream cs;//定义将数据流转链接到加密转换的流
            byte[] byt;
            mCSP = new DESCryptoServiceProvider();
            //CreateEncryptor创建加密对象
            ct = mCSP.CreateEncryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV));

            byt = Encoding.UTF8.GetBytes(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="Value">要解密字符串</param>
        /// <returns>解密了的字符串</returns>
        public string DecryptString(string Value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP = new DESCryptoServiceProvider();
            ct = mCSP.CreateDecryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV));

            byt = Convert.FromBase64String(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());
        }


        public string MD5Encrypt(string strPwd)
        {
            var bytes = Encoding.Default.GetBytes(strPwd);
            var Md5 = new MD5CryptoServiceProvider().ComputeHash(bytes);
            return Convert.ToBase64String(Md5);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public string RSAEncrypt(string strPwd)
        {
            var bytes = Encoding.Default.GetBytes(strPwd);
            var encryptBytes = new RSACryptoServiceProvider(new CspParameters()).Encrypt(bytes, false);
            return Convert.ToBase64String(encryptBytes);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public string RSADecrypt(string strPwd)
        {
            try
            {
                var bytes= Convert.FromBase64String(strPwd);
                var DecryptBytes =new RSACryptoServiceProvider(new CspParameters()).Decrypt(bytes,false);
                return Encoding.Default.GetString(DecryptBytes);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

    }
}