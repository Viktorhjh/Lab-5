using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Lab_5
{
    abstract class Main
    {
        abstract public string readFile();
        abstract public void writeFile(string text);
    }

    class Default : Main
    {
        string path, text;
        public override string readFile()
        {
            try
            {
                using (Stream stream = File.OpenRead(path))
                {
                    byte[] array = new byte[stream.Length];
                    stream.Read(array, 0, array.Length);
                    text = System.Text.Encoding.Default.GetString(array);
                    return text;
                }
            }
            catch
            {
            }
            return "";
        }

        public override void writeFile(string input)
        {
            using (Stream stream = File.Create(path))
            {
                byte[] array = new byte[stream.Length];
                array = System.Text.Encoding.Default.GetBytes(input);
                stream.Write(array, 0, array.Length);
            }
        }

        public void setPath(string path)
        {
            this.path = path;
        }
    }

    class Compress : Default
    {
        public void compress(string input, string path)
        {
            string str;
            byte[] bytes = System.Text.Encoding.Default.GetBytes(input);
            using (Stream s = File.Create(path))
            using (Stream ds = new DeflateStream(s, CompressionMode.Compress))                                
                for(int i = 0; i < bytes.Length; i++)
                {
                    ds.WriteByte(bytes[i]);
                }
        }

        public string decompress(string path)
        {
            try
            {
                byte[] bytes = System.Text.Encoding.Default.GetBytes(readFile());
                using (Stream s = File.OpenRead(path))
                using (Stream ds = new DeflateStream(s, CompressionMode.Decompress))
                    ds.Read(bytes, 0, bytes.Length);
                string str = System.Text.Encoding.Default.GetString(bytes);
                return str;
            }
            catch
            {
                return "er";
            }
            
        }
    }
    
    class Encrypting : Default
    {

        string hash = "qwe12@v";

        public string Encrypt(string input, string path)
        {
            string str;
            byte[] bytes = System.Text.Encoding.Default.GetBytes(input);
            using (MD5CryptoServiceProvider m = new MD5CryptoServiceProvider())
            {
                byte[] key = m.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] res = transform.TransformFinalBlock(bytes, 0, bytes.Length);
                    str = Convert.ToBase64String(res, 0, res.Length);                    
                    base.writeFile(str);
                    return Convert.ToBase64String(res, 0, res.Length);
                }
            }            
        }

        public string Decrypt(string path)
        {
            try
            {
                byte[] data = Convert.FromBase64String(base.readFile());
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] key = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(hash));
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateDecryptor();
                        byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                        return System.Text.Encoding.Default.GetString(result, 0, result.Length);
                    }
                }
            }
            catch
            {
                return "";
            }
            
        }

    }
}
