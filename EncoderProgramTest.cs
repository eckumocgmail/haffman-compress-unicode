using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Console_Encoder
{
    using static System.Console;
    using static System.Text.Json.JsonSerializer;
    internal class EncoderProgramTest
    {
        internal static void Main(params string[] args)
        {
            TestCharConverter();
            
            TestTextConverter();
            TestBitConverter();

        }

        private static void TestCharConverter()
        {
            var bit = new BitConverter();
            for(int i=0; i<1024; i++)
            {
                if (bit.ToInt(bit.ToBinary(i)) != i)
                    throw new Exception("i="+i);
            }
            for (byte i = 0; i < 255; i++)
            {
                if (bit.ToInt(bit.ToBinary(i)) != i)
                    throw new Exception("i=" + i);
            }
        }

        private static void TestTextConverter()
        {
            var encoder = new TextEncoder();
            var bits = encoder.Encode("ABBCCCDDDDEEEEE");
            var decoder = new CharDecoder(bits);
            var bytes = decoder.Decode( );
            foreach (char ch in bytes)
                Write(ch);
        }

        static void s1()
        {
            
                // Create a UTF-16 encoding that supports a BOM.
                Encoding unicode = new UnicodeEncoding();

                // A Unicode string with two characters outside an 8-bit code range.
                String unicodeString =
                    "This Unicode string has 2 characters outside the " +
                    "ASCII range: \n" +
                    "Pi (\u03A0)), and Sigma (\u03A3).";
                Console.WriteLine("Original string:");
                Console.WriteLine(unicodeString);
                Console.WriteLine();

                // Encode the string.
                Byte[] encodedBytes = unicode.GetBytes(unicodeString);
                Console.WriteLine("The encoded string has {0} bytes.\n",
                                  encodedBytes.Length);

                // Write the bytes to a file with a BOM.
                var fs = new FileStream(@"d:\UTF8Encoding.txt", FileMode.Create);
                Byte[] bom = unicode.GetPreamble();
                fs.Write(bom, 0, bom.Length);
                fs.Write(encodedBytes, 0, encodedBytes.Length);
                Console.WriteLine("Wrote {0} bytes to the file.\n", fs.Length);
                fs.Close();

                // Open the file using StreamReader.
                var sr = new StreamReader(@"d:\UTF8Encoding.txt");
                String newString = sr.ReadToEnd();
                sr.Close();
                Console.WriteLine("String read using StreamReader:");
                Console.WriteLine(newString);
                Console.WriteLine();

                // Open the file as a binary file and decode the bytes back to a string.
                fs = new FileStream(@"d:\UTF8Encoding.txt", FileMode.Open);
                Byte[] bytes = new Byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                fs.Close();

                String decodedString = unicode.GetString(bytes);
                Console.WriteLine("Decoded bytes:");
                Console.WriteLine(decodedString);
            
        }
        private static void TestBitConverter()
        {
            s1();
            string hex = "";
            for(int i=0; i<9; i++)
            {
                hex += i;
            }
            for (char i = 'A'; i < 'Z'; i++)
            {
                hex += i;
                if (hex.Length == 16)
                    break;
            }
            /*foreach (char ch1 in hex)
                foreach (char ch2 in hex)
                    foreach (char ch3 in hex)
                        foreach (char ch4 in hex)
                            Console.WriteLine($"\\u{ch1}{ch2}{ch3}{ch4}");*/
          

            // The encoding.
            UnicodeEncoding unicode = new UnicodeEncoding();

            // Create a string that contains Unicode characters.
            String unicodeString =
                "This Unicode string contains two characters " +
                "with codes outside the traditional ASCII code range, " +
                "Pi (\u03a0) and Sigma (\u03a3).";
            Console.WriteLine("Original string:");
            Console.WriteLine(unicodeString);

            // Encode the string.
            Byte[] encodedBytes = unicode.GetBytes(unicodeString);
            Console.WriteLine();
            Console.WriteLine("Encoded bytes:");
            foreach (Byte b in encodedBytes)
            {
                Console.Write("[{0}]", b);
            }
            Console.WriteLine();

            // Decode bytes back to string.
            // Notice Pi and Sigma characters are still present.
            String decodedString = unicode.GetString(encodedBytes);
            Console.WriteLine();
            Console.WriteLine("Decoded bytes:");
            Console.WriteLine(decodedString);
            /*using (var stream = new StreamWriter(new FileStream("D:\\1.txt", FileMode.OpenOrCreate)))
            {
                var bitConverter = new BitConverter();
                List<byte> bytes = new List<byte>();
                List<int> characters = new List<int>();
                for (int b = 0;b <= 1024*8; b++)
                {
                    Console.WriteLine(b+"="+Encoding.Unicode.GetString(new byte[] { (byte)b }));
                    
                }
                
                //stream.WriteLine(Encoding.Unicode.GetString(bytes.ToArray())); 
                stream.Flush();
            }*/
        }
    }
}
