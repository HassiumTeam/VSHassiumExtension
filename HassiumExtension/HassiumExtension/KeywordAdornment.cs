//------------------------------------------------------------------------------
// <copyright file="KeywordAdornment.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace HassiumExtension
{
    internal sealed class KeywordAdornment
    {
        private readonly IAdornmentLayer layer;
        private readonly IWpfTextView view;
        private readonly Brush brush;
        private readonly Pen pen;

        private string[] keywords =
        {
            "class",
            "do",
            "enum",
            "func",
            "if",
            "trait",
            "while",
        };

        public KeywordAdornment(IWpfTextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            this.layer = view.GetAdornmentLayer("KeywordAdornment");

            this.view = view;
            this.view.LayoutChanged += this.OnLayoutChanged;

            this.brush = new SolidColorBrush(Color.FromArgb(0x20, 0x00, 0x00, 0xff));
            this.brush.Freeze();

            var penBrush = new SolidColorBrush(Colors.Red);
            penBrush.Freeze();
            this.pen = new Pen(penBrush, 0.5);
            this.pen.Freeze();
        }

        internal void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (ITextViewLine line in e.NewOrReformattedLines)
            {
                var text = VisualCharArray.FromTextView(view, line);
                foreach (var word in keywords)
                    text.DrawAtWord(word, Draw, layer, view, view.TextViewLines, brush, pen);
            }
        }

        public void Draw(IAdornmentLayer layer, IWpfTextView view, IWpfTextViewLineCollection textViewLines, Brush brush, Pen pen, int index)
        {
            SnapshotSpan span = new SnapshotSpan(view.TextSnapshot, Span.FromBounds(index, index + 1));
            Geometry geometry = textViewLines.GetMarkerGeometry(span);
            if (geometry != null)
            {
                var drawing = new GeometryDrawing(brush, pen, geometry);
                drawing.Freeze();

                var drawingImage = new DrawingImage(drawing);
                drawingImage.Freeze();

                var image = new Image
                {
                    Source = drawingImage,
                };
                
                Canvas.SetLeft(image, geometry.Bounds.Left);
                Canvas.SetTop(image, geometry.Bounds.Top);

                layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, image, null);
            }
        }
    }
}
