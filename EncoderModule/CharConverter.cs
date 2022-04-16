using System;
using System.Collections.Generic;
using System.Linq;
using static System.Text.Json.JsonSerializer;
using static System.Console;


namespace Console_Encoder
{
    public class CharNode
    {
        public List<byte> Characters { get; set; }
        public int Rating { get; set; }
        public CharNode Left { get; set; }
        public CharNode Right { get; set; }

        public CharNode(List<byte> list, int value)
        {
            this.Characters = list;
            this.Rating = value;
        }
        public bool Contains(byte ch) => Characters.Contains(ch);
        public string GetText()
        {
            string text = "";
            foreach (char ch in Characters)
                text+=(ch);
            return text;
        }
        public void Trace() => Trace(1);
        public void Trace(int level)
        {
            if (Left != null) Left.Trace(level + 1);

            for (int i = 1; i <= level; i++)
                Write("\t");
            foreach (char ch in Characters)
                Write(ch);
            WriteLine();
            if (Right != null) Right.Trace(level+1);
        }
    }
    public class CharConverter 
    {        
        private IDictionary<byte, int> Statistics = new SortedDictionary<byte, int>();
        private IDictionary<char, byte> ASCII = new SortedDictionary<char, byte>();
        private IDictionary<char, int> UniCode = new SortedDictionary<char, int>();
        private IDictionary<byte, IEnumerable<bool>> Encodes = new SortedDictionary<byte, IEnumerable<bool>>(); 
        public void OnRead(byte ch)
        {
            if (Statistics.ContainsKey(ch) == false)
            {
                Statistics[ch] = 1;
            }
            else
            {
                Statistics[ch]++;
            }            
        }

        public KeyValuePair<IEnumerable<int>, CharNode> Build( )
        {
            Encodes.Clear();
            var rating = new List<CharNode>();
         
            List<int> metadata = new List<int> { Statistics.Count() };
            foreach (var kv in Statistics){
                metadata.Add(kv.Key);
                metadata.Add(kv.Value);
                rating.Add(new CharNode(new List<byte>() { kv.Key },kv.Value));
            }
            WriteLine(Serialize(metadata));
            while (rating.Count() > 1)
            {
                foreach(var rate in rating)
                {
                    Write($"\t{rate.GetText()}");
                }
                WriteLine();
                WriteLine();
                rating.Sort((x1, x2) => x1.Rating - x2.Rating);
                var chars = new List<byte>();
                int count = 0;
                count += rating[0].Rating;
                count += rating[1].Rating;
                chars.AddRange(rating[0].Characters);
                chars.AddRange(rating[1].Characters);
                CharNode left = rating[0];
                CharNode right = rating[1];
                rating.RemoveAt(1);
                rating.RemoveAt(0);
                rating.Add(new CharNode(chars, count)
                {
                    Left = left,
                    Right = right
                });
                
            }
            WriteLine("Tree");
            rating[0].Trace();
            return new KeyValuePair<IEnumerable<int>, CharNode>(metadata, rating[0]);
        }
        internal void LogInfo()        
            => Console.WriteLine(Serialize(Statistics));
        

        public int ToInt(char ch) => UniCode[ch];
        public byte ToByte(char ch) => ASCII[ch];
        public char ToChar(int code) => UniCode.Where(kv => kv.Value == code).First().Key;
        public char ToChar(byte code) => ASCII.Where(kv => kv.Value == code).First().Key; 
    }
    public class CharDecoder
    {
        private CharNode _root;
        private IEnumerable<bool> _data;

        public CharDecoder(IEnumerable<bool> bits)
        {
            var bitConverter = new BitConverter();

            
            var bitsArray = bits.ToArray();
            var lengthBits = new List<bool>();
            for (int i = 0; i < 4 * 8; i++)
            {
                lengthBits.Add(bitsArray[ i]);
            }
            int length = bitConverter.ToInt(lengthBits);
            WriteLine(length);

            var statistics = new Dictionary<byte, int>();
            
            for(int byteIndex=0; byteIndex<length; byteIndex++)
            {
                int cursor = 4*8 +(4*8*2*byteIndex);
                var charBits = new List<bool>();
                var rateBits = new List<bool>();
                for(int i=0; i<4*8; i++)
                {
                    charBits.Add(bitsArray[cursor + i]);
                }
                for (int i = 4*8; i < 2*4 * 8; i++)
                {
                    rateBits.Add(bitsArray[cursor + i]);
                }
                int character = bitConverter.ToInt(charBits);
                int rate = bitConverter.ToInt(rateBits);
                statistics[(byte)character] = rate;
            }
            var rating = new List<CharNode>();
            foreach (var kv in statistics)
            {
              
                rating.Add(new CharNode(new List<byte>() { kv.Key }, kv.Value));
            }
     
            while (rating.Count() > 1)
            {
                foreach (var rate in rating)
                {
                    Write($"\t{rate.GetText()}");
                }
                WriteLine();
                WriteLine();
                rating.Sort((x1, x2) => x1.Rating - x2.Rating);
                var chars = new List<byte>();
                int count = 0;
                count += rating[0].Rating;
                count += rating[1].Rating;
                chars.AddRange(rating[0].Characters);
                chars.AddRange(rating[1].Characters);
                CharNode left = rating[0];
                CharNode right = rating[1];
                rating.RemoveAt(1);
                rating.RemoveAt(0);
                rating.Add(new CharNode(chars, count)
                {
                    Left = left,
                    Right = right
                });

            }
            WriteLine("Tree");
            _root = rating[0];
            _root.Trace();
            var dataBits = new List<bool>();
            for (int i=(4*8+(2*4*8*length)); i<bits.Count(); i++)
            {
                dataBits.Add(bitsArray[i]);
            }
            _data = dataBits;
            WriteLine(Serialize(statistics));
        }

        private void SetRoot(CharNode root)
        {
            this._root = root;
        }
        public IEnumerable<byte> Decode( )
        {
            CharNode p = _root;

            var result = new List<byte>();
            foreach (bool right in _data)
            {
                if (right)
                {
                    p = p.Right;
                }
                else
                {
                    p = p.Left;
                }
                if(p.Characters.Count() == 1)
                {
                    result.Add(p.Characters[0]);
                    p = _root;
                }
            }
            return result; ;
        }
    }
    public class CharEncoder
    {
        private CharNode _root;
        public CharEncoder(CharNode root)
        {
            this._root = root;
        }

        internal IEnumerable<bool> EncodeData(string text)
        {
            var result = new List<bool>();
            foreach (byte ch in text)
            {
                result.AddRange(EncodeByte(ch));
            }
            return result; ;
        }

        internal List<bool> EncodeMetadata(IEnumerable<int> indexes)
        {
            var result = new List<bool>();
            var bitConverter = new BitConverter();
            foreach(int index in indexes)
            {
                result.AddRange(bitConverter.ToBinary(index));
            }
            return result;
        }

        private IEnumerable<bool> EncodeByte(byte ch)
        {
            var result = new List<bool>();
            CharNode p = _root;

            while (true && p!=null)
            {
                bool next = p.Right!=null? p.Right.Contains(ch): false;
                 result.Add(next);
                if (next)
                    p = p.Right;
                else p = p.Left;
                if (p.Characters.Count() == 1)
                    return result;
            }
            return result;
        }
    }
}
