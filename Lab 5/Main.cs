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
        string path, text;

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

        public override void writeFile(string text)
        {
            if (text != null)
            {
                using (Stream stream = File.OpenWrite(path))
                {
                    byte[] array = new byte[stream.Length];

                    array = System.Text.Encoding.Default.GetBytes(text);

                    stream.Write(array, 0, array.Length);
                }
            }
        }
        public void setPath(string path)
        {
            this.path = path;
        }
    }

    class Compress : Default
    {
        public void Zip(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                base.writeFile(System.Text.Encoding.Default.GetString(mso.ToArray()));                
            }
        }

        public string Unzip()
        {
            byte[] bytes;
            bytes = System.Text.Encoding.Default.GetBytes(base.readFile());
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    try
                    {
                        gs.CopyTo(mso);
                    }
                    catch
                    {
                        base.readFile();
                    }

                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }


    }

    class Encrypting : Default
    {
        string key = "sblw-3hn8-sqoy19";
        public string Encrypt(string input)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            base.writeFile(Convert.ToBase64String(resultArray, 0, resultArray.Length));
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string Decrypt()
        {
            //try
            //{
            string str;
                byte[] inputArray = System.Text.Encoding.Default.GetBytes(base.readFile());
                str = System.Text.Encoding.Default.GetString(inputArray);
                inputArray = Convert.FromBase64String(str);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            //}
            //catch
            //{
            //    return "Error";
            //}
        }
    }


}
