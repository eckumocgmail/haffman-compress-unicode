using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Encoder.EncoderModule
{
    public class CharsetCollections
    {
        public static string LinearCharset = "-+=^%*/";
        public static string LogicCharset = "|&";
        public static string SpaceCharset = "\t\n\r ";
        public static string SingleSepCharset = "'\"";
        public static string DoubleSepCharset = "()[]{}";
        public static string SpecCharset = "#$~\\";
        public static string PunctuationsCharset = ",.?;:!-";
        public static string NumbersCharset = "0123456789";
        public static string EngCharset = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
        public static string RusCharset = "ёйцукенгшщзхъфывапролджэячсмитьбю"+"ЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";
        public static IEnumerable<char> GetAll()
        {
            var chars = new List<char>((RusCharset + EngCharset + NumbersCharset + PunctuationsCharset + SpecCharset + DoubleSepCharset + SingleSepCharset + LogicCharset + LinearCharset).ToCharArray());
            chars.AddRange(SpaceCharset.ToCharArray());
            return chars;
        }

    }
}
