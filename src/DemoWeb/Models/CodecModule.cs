using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DemoWeb.Models
{
    public class CodecModule
    {

        public static byte[] EncrytString(string encKey, string rawText)
        {
            return DESEncrypt(encKey, Encoding.UTF8.GetBytes(rawText));
        }

        public static List<string> DecryptData(string encKey, List<byte[]> data)
        {
            return data.Select(o =>
            {
                return Encoding.UTF8.GetString(DESDecrypt(o, encKey));
            }).ToList();
        }

        //REF: https://dotblogs.com.tw/supershowwei/2016/01/11/135230
        static byte[] HashByMD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            return md5.ComputeHash(Encoding.UTF8.GetBytes(source));
        }

        static byte[] DESEncrypt(string key, byte[] data)
        {
            var des = new DESCryptoServiceProvider();
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, HashByMD5(key));
            des.Key = rfc2898.GetBytes(des.KeySize / 8);
            des.IV = rfc2898.GetBytes(des.BlockSize / 8);
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();

                return ms.ToArray();
            }
        }

        static byte[] DESDecrypt(byte[] encData, string encKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(encKey, HashByMD5(encKey));
            des.Key = rfc2898.GetBytes(des.KeySize / 8);
            des.IV = rfc2898.GetBytes(des.BlockSize / 8);
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(encData, 0, encData.Length);
                cs.FlushFinalBlock();

                return ms.ToArray();
            }
        }

    }
}