using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WolfDecryptor
{
    public class Decrypt
    {
        public const int HeaderLength = 4;

        public static bool IsValidKey(byte[] header, string key)
        {
            if (header == null)
                throw new ArgumentException($"Null header.", "header");

            if (header.Length != HeaderLength)
                throw new ArgumentException($"Header length must be {HeaderLength} bytes, but it {header.Length} bytes.", "header");
            
            return TestKey(header, StringKeyToBytes(key));
        }

        private static byte[] StringKeyToBytes(string key)
        {
            if (key.Length % 2 == 1)
                throw new ArgumentException("The binary key cannot have an odd number of digits.", "key");

            int charsCount = key.Length;
            byte[] bytes = new byte[charsCount / 2];
            for (int i = 0; i < charsCount; i += 2)
                bytes[i / 2] = Convert.ToByte(key.Substring(i, 2), 16);
            return bytes;
        }

        private static bool TestKey(byte[] header, byte[] key)
        {
            byte[] numArray = new byte[4]
                              {
                                  (byte) ((uint) header[0] ^ (uint) key[0]),
                                  (byte) ((uint) header[1] ^ (uint) key[1]),
                                  (byte) 0,
                                  (byte) ((uint) header[3] ^ (uint) key[3])
                              };
            if ((int)numArray[0] == 68 && (int)numArray[1] == 88)
                return (int)numArray[3] == 0;
            return false;
        }
    }
}
