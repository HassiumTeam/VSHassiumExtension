using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace HassiumExtension
{
    public class VisualCharArray
    {
        public static VisualCharArray FromTextView(IWpfTextView view, ITextViewLine line)
        {
            VisualCharArray arr = new VisualCharArray();

            for (int i = line.Start; i < line.End; i++)
                arr.Add(new VisualChar(view.TextSnapshot[i], i));

            return arr;
        }

        public List<VisualChar> Chars { get; private set; }

        public VisualCharArray()
        {
            Chars = new List<VisualChar>();
        }
        public VisualCharArray(List<VisualChar> chars)
        {
            Chars = chars;
        }

        public void Add(VisualChar ch)
        {
            Chars.Add(ch);
        }

        public void DrawAtWord(string word, DrawDelegate drawDelegate, IAdornmentLayer layer, IWpfTextView view, IWpfTextViewLineCollection textViewLines, Brush brush, Pen pen)
        {
            string str = ToString();

            int startIndex = 0;
            while (true)
            {
                if (startIndex >= str.Length)
                    break;
                var tmp = str.Substring(startIndex);
                int index = tmp.IndexOf(word);
                if (index == -1)
                    break;
                
                for (int i = startIndex + index; i < startIndex + index + word.Length; i++)
                {
                    Chars[i].DrawDelegate = drawDelegate;
                    Chars[i].Draw(layer, view, textViewLines, brush, pen, Chars[i].IndexInLine);
                }

                startIndex += index + word.Length;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var c in Chars)
                sb.Append(c.Char);
            return sb.ToString();
        }
    }
}
