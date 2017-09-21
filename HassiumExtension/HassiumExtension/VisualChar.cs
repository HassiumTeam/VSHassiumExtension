using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace HassiumExtension
{
    public delegate void DrawDelegate(IAdornmentLayer layer, IWpfTextView view, IWpfTextViewLineCollection textViewLines, Brush brush, Pen pen, int index);

    public class VisualChar
    {
        public char Char { get; set; }
        public int IndexInLine { get; private set; }
        public DrawDelegate DrawDelegate { get; set; }

        public VisualChar(char c, int index)
        {
            Char = c;
            IndexInLine = index;
        }

        public void Draw(IAdornmentLayer layer, IWpfTextView view, IWpfTextViewLineCollection textViewLines, Brush brush, Pen pen, int index)
        {
            DrawDelegate(layer, view, textViewLines, brush, pen, index);
        }
    }
}
