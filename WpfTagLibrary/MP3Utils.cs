using System;
using System.Collections;
using System.Text;

namespace TagLib.Utils
{
    class Mp3Utils
    {
        /// <summary>
        /// Übersetzt ein Array von Bytes in eine Zeichenfolge
        /// </summary>
        /// <param name="argBytes"></param>
        /// <returns></returns>
        public static string BytesToText(byte[] argBytes)
        {
            ArrayList arrayList = new ArrayList((ICollection)argBytes);
            return Encoding.ASCII.GetString(Array.FindAll<byte>(argBytes, new Predicate<byte>(isAlphaNumeric)));
        }

        /// <summary>
        /// Übersetzt eine Zeichenfolge in einen Array von Bytes
        /// </summary>
        /// <param name="argText"></param>
        /// <returns></returns>
        public static byte[] TextToBytes(string argText)
        {
            ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(argText);
        }

        /// <summary>
        /// Überprüft, ob ein Byte ein alphanumerisches Zeichen in der ASCII-Tabelle ist.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool isAlphaNumeric(byte b)
        {
            if (b > 31 & b < 128)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Ruft die codierte Größe eines ID3v2.2-Tags in Byte ab
        /// </summary>
        /// <param name="argBytes"></param>
        /// <returns></returns>
        public static int GetID3EncodedSizeV2(byte[] argBytes)
        {
            return (argBytes[0] * 256 * 256 * 256) + (argBytes[1] * 256 * 256) + (argBytes[2] * 256) + argBytes[3];
        }

        /// <summary>
        /// Ruft die codierte Größe eines ID3v2.3-Tags in Byte ab
        /// </summary>
        /// <param name="argBytes"></param>
        /// <returns></returns>
        public static int GetID3EncodedSizeV3(byte[] argBytes)
        {
            return (argBytes[0] * 256 * 256 * 256) + (argBytes[1] * 256 * 256) + (argBytes[2] * 256) + argBytes[3];
        }

        /// <summary>
        /// Ruft die codierte Größe eines ID3v2.4-Tags in Byte ab
        /// </summary>
        /// <param name="argBytes"></param>
        /// <returns></returns>
        public static int GetID3EncodedSizeV4(byte[] argBytes)
        {
            return (argBytes[0] * 128 * 128 * 128) + (argBytes[1] * 128 * 128) + (argBytes[2] * 128) + argBytes[3];
        }

        public static string BytesToBinaryString(byte[] argBytes)
        {
            string s = "";
            foreach (byte b in argBytes)
            {
                s += Convert.ToString(Convert.ToInt32(b), 2).PadLeft(8, '0');
            }
            return s;
        }
    }
}

