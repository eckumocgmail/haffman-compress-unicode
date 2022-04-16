using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Encoder 
{
    public interface IBitConverter
    {
        public IEnumerable<bool> ToBinary(byte code);
        public IEnumerable<bool> ToBinary(int code);
        public int ToInt(IEnumerable<bool> binary);
        public byte ToByte(IEnumerable<bool> binary);
        public IEnumerable<byte> ToByte(int number);
        public int ToInt(byte b1, byte b2, byte b3, byte b4);

    }
    public class BitConverter: IBitConverter
    {
        public IEnumerable<bool> ToBinary(byte code)
        {
            var binary = new List<bool>();
            int x = code;
            while (x > 0)
            {
                int dev = x % 2;
                x = (x - dev) / 2;
                binary.Add((dev == 1) ? true : false);
            }
            while (binary.Count() < 8)
            {
                binary.Add(false);
            }
            binary.Reverse();
            return binary;
        }

        public IEnumerable<bool> ToBinary(int code)
        {
            var binary = new List<bool>();
            int x = code;
            while (x > 0)
            {
                int dev = x % 2;
                x = (x - dev) / 2;
                binary.Add((dev == 1) ? true : false);
            }
            while (binary.Count() < 8*4)
            {
                binary.Add(false);
            }
            binary.Reverse();
            return binary;
        }


        public int ToInt(IEnumerable<bool> binary)
        {
            int result = 0;
            var arr = binary.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if( arr[i])
                {
                    int rank = (arr.Length - i-1);
                    result += (int)Math.Pow(2, rank);
                }
            }
            return result;
        }

        public byte ToByte(IEnumerable<bool> binary)
        {
            byte result = 0;
            var arr = binary.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i])
                {
                    int rank = (arr.Length - i-1);
                    result += (byte)Math.Pow(2, rank);
                }
            }
            return result;
        }

        public IEnumerable<byte> ToByte(int number)
        {
            var arr = new List<bool>(ToBinary(number));
            byte b1 = 0;
            byte b2 = 0;
            byte b3 = 0;
            byte b4 = 0;
            for (int i=0; i<8; i++)
            {
                int rank = (8 - i - 1);
                if (arr[i])
                {
                    b1 += (byte)Math.Pow(2, rank);
                }
                if (arr[i + 8])
                {
                    b2 += (byte)Math.Pow(2, rank);
                }
                if (arr[i + 16])
                {
                    b3 += (byte)Math.Pow(2, rank);
                }
                if (arr[i + 24])
                {
                    b4 += (byte)Math.Pow(2, rank);
                }
            }
            return new List<byte>() { b1, b2, b3, b4 };
        }

        public int ToInt(byte b1,byte b2,byte b3,byte b4)
        {
            throw new Exception();
        }
    }
}
