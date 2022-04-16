using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;
using System.Threading.Tasks;

namespace Console_Encoder
{
    public class TextEncoder
    {
        //private BitConverter bitConverter = new BitConverter();
        private CharConverter charConverter = new CharConverter();

        public IEnumerable<bool> Encode(string text)
        {
            foreach(byte ch in text.ToCharArray())            
                charConverter.OnRead(ch);
            var root = charConverter.Build();
            var encoder = new CharEncoder(root.Value);
            List<bool> bits = encoder.EncodeMetadata(root.Key);
            bits.AddRange(encoder.EncodeData(text));
            return bits;
        }
 
    }
}
