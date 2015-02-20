// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerView.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a control that displays a plot.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace OxyPlot.WindowsForms
{
    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Windows.Forms;

    /// <summary>
    /// A control that displays a <see cref="TrackerHitResult"/>.
    /// </summary>
    public class TrackerView : Control
    {
        /// <summary>
        /// The tracker location.
        /// </summary>
        private ScreenPoint trackerLocation = ScreenPoint.Undefined;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerView"/> class.
        /// </summary>
        public TrackerView()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Gets or sets the tracker position.
        /// </summary>
        public ScreenPoint TrackerLocation
        {
            get { return trackerLocation; }
            set { trackerLocation = value; }
        }

        /// <summary>
        /// Gets or sets the tracker area.
        /// </summary>
        public OxyRect TrackerBounds { get; set; }

        /// <summary>
        /// Gets or sets the tracker text.
        /// </summary>
        public string TrackerText { get; set; }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.Paint event.
        /// </summary>
        /// <param name="e">A System.Windows.Forms.PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!ScreenPoint.IsUndefined(this.trackerLocation))
            {
                this.PaintTracker(e);
            }
        }

        /// <summary>
        /// Paints the tracker.
        /// </summary>
        /// <param name="e"></param>
        private void PaintTracker(PaintEventArgs e)
        {
            if (ScreenPoint.IsUndefined(this.trackerLocation))
            {
                return;
            }

            TextRenderingHint origHint = e.Graphics.TextRenderingHint;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

            string text = this.TrackerText;
            Font trackerFont = new System.Drawing.Font("Arial", 10);
            SizeF textSize = e.Graphics.MeasureString(text, trackerFont);

            OxyRect trackerBounds = this.TrackerBounds;
            Point trackerPosition = this.trackerLocation.ToPoint(true);
            OxyRect textboxBounds = this.TrackerBounds;
            OxyRect textBox = new OxyRect(trackerPosition.X, trackerPosition.Y, textSize.Width, textSize.Height);
            int textboxOffset = 5;

            // default text box position is top and centered horizontally
            textBox.Left = trackerPosition.X - (textSize.Width / 2) - 1;
            textBox.Top = trackerPosition.Y - textSize.Height - textboxOffset - 1;

            // determine textbox orientation
            int orientation = 0;
            if (textboxBounds.Width > 0)
            {
                if (textBox.Left + textSize.Width > textboxBounds.Right)
                {
                    textBox.Left = trackerPosition.X - textSize.Width - textboxOffset;
                    textBox.Top = trackerPosition.Y - (textSize.Height / 2);
                    orientation = 1; // left
                }

                if (textBox.Left < textboxBounds.Left)
                {
                    textBox.Left = trackerPosition.X + textboxOffset;
                    textBox.Top = trackerPosition.Y - (textSize.Height / 2);
                    orientation = 2; // right
                }
            }

            if (textboxBounds.Height > 0)
            {
                if (textBox.Top < textboxBounds.Top)
                {
                    textBox.Left = trackerPosition.X - (textSize.Width / 2);
                    textBox.Top = trackerPosition.Y + textboxOffset;
                    orientation = 3; // bottom
                }
            }

            PointF[] borderPoints = new PointF[0];
            PointF[] polyPoints = new PointF[0];

            // textbox border line
            if (orientation == 0)
            {
                polyPoints = new PointF[]
                {
                    new PointF((float) (textBox.Left + (textSize.Width/2.0f) - textboxOffset), (float) textBox.Bottom),
                    trackerPosition.ToScreenPoint().ToPoint(true),
                    new PointF((float) (textBox.Left + (textSize.Width/2.0f) + textboxOffset), (float) textBox.Bottom),
                };

                borderPoints = new PointF[]
                {
                    polyPoints[0],
                    new PointF((float) textBox.Left, (float) textBox.Bottom),
                    new PointF((float) textBox.Left, (float) textBox.Top),
                    new PointF((float) textBox.Right, (float) textBox.Top),
                    new PointF((float) textBox.Right, (float) textBox.Bottom),
                    polyPoints[2],
                };
            }
            else if (orientation == 1)
            {
                polyPoints = new PointF[]
                {
                    new PointF((float) textBox.Right, (float) textBox.Top + (textSize.Height/2.0f) - textboxOffset),
                    trackerPosition.ToScreenPoint().ToPoint(true),
                    new PointF((float) textBox.Right, (float) textBox.Top + (textSize.Height/2.0f) + textboxOffset),
                };

                borderPoints = new PointF[]
                {
                    polyPoints[0],
                    new PointF((float) textBox.Right, (float) textBox.Top),
                    new PointF((float) textBox.Left, (float) textBox.Top),
                    new PointF((float) textBox.Left, (float) textBox.Bottom),
                    new PointF((float) textBox.Right, (float) textBox.Bottom),
                    polyPoints[2],
                };
            }
            else if (orientation == 2)
            {
                polyPoints = new PointF[]
                {
                    new PointF((float) textBox.Left, (float) textBox.Top + (textSize.Height/2.0f) - textboxOffset),
                    trackerPosition.ToScreenPoint().ToPoint(true),
                    new PointF((float) textBox.Left, (float) textBox.Top + (textSize.Height/2.0f) + textboxOffset),
                };

                borderPoints = new PointF[]
                {
                    polyPoints[0],
                    new PointF((float) textBox.Left, (float) textBox.Top),
                    new PointF((float) textBox.Right, (float) textBox.Top),
                    new PointF((float) textBox.Right, (float) textBox.Bottom),
                    new PointF((float) textBox.Left, (float) textBox.Bottom),
                    polyPoints[2],
                };
            }
            else if (orientation == 3)
            {
                polyPoints = new PointF[]
                {
                    new PointF((float) (textBox.Left + (textSize.Width/2.0f) - textboxOffset), (float) textBox.Top),
                    trackerPosition.ToScreenPoint().ToPoint(true),
                    new PointF((float) (textBox.Left + (textSize.Width/2.0f) + textboxOffset), (float) textBox.Top),
                };

                borderPoints = new PointF[]
                {
                    polyPoints[0],
                    new PointF((float) textBox.Left, (float) textBox.Top),
                    new PointF((float) textBox.Left, (float) textBox.Bottom),
                    new PointF((float) textBox.Right, (float) textBox.Bottom),
                    new PointF((float) textBox.Right, (float) textBox.Top),
                    polyPoints[2],
                };
            }

            // draw horizontal and vertical lines
            e.Graphics.DrawLine(
                Pens.Gray,
                new PointF((float)trackerPosition.X, (float)trackerBounds.Top),
                new PointF((float)trackerPosition.X, (float)trackerBounds.Bottom));

            e.Graphics.DrawLine(
                Pens.Gray,
                new PointF((float)trackerBounds.Left, trackerPosition.Y),
                new PointF((float)trackerBounds.Right, trackerPosition.Y));

            // draw the textbox
            Brush textboxBrush = new SolidBrush(Color.FromArgb(128, Color.LightSkyBlue));
            Pen textboxPen = new Pen(Color.Black);
            e.Graphics.FillRectangle(textboxBrush, textBox.ToRect(true));

            if (polyPoints.Length > 0)
            {
                e.Graphics.FillPolygon(textboxBrush, polyPoints);
                e.Graphics.DrawLines(textboxPen, polyPoints);
            }

            if (borderPoints.Length > 0)
            {
                e.Graphics.DrawLines(textboxPen, borderPoints);
            }

            // draw the text
            e.Graphics.DrawString(text, trackerFont, Brushes.Black, (float)textBox.Left, (float)textBox.Top);

            textboxBrush.Dispose();
            textboxPen.Dispose();
            trackerFont.Dispose();

            e.Graphics.TextRenderingHint = origHint;
        }

        /// <summary>
        /// Processes Windows messages.
        /// <remarks>
        /// Pass mouse events through to parent control.
        /// https://stackoverflow.com/questions/547172/pass-through-mouse-events-to-parent-control
        /// </remarks>
        /// </summary>
        /// <param name="m">The Windows System.Windows.Forms.Message to process.</param>
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);

            if (m.Msg == WM_NCHITTEST)
            {
                m.Result = (IntPtr)HTTRANSPARENT;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
