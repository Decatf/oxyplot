// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerControl.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
    using System.Windows.Forms;

    /// <summary>
    /// Windows Forms tracker control
    /// </summary>
    public class TrackerControl : Control
    {
        /// <summary>
        /// Redraw the parent control bitmap.
        /// </summary>
        private bool refreshBackground = true;

        /// <summary>
        /// A bitmap of the parent control
        /// </summary>
        private Bitmap backgroundBitmap = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerControl"/> class.
        /// </summary>
        public TrackerControl()
        {
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Gets or sets the tracker hit result data.
        /// </summary>
        public TrackerHitResult TrackerHitResult { get; set; }

        /// <summary>
        /// Invalidates the entire surface of the control and causes the control to redrawn.
        /// </summary>
        /// <param name="refreshBackground">true to redraw the parent control bitmap.</param>
        public void InvalidateControl(bool refreshBackground)
        {
            if (refreshBackground == true)
            {
                this.refreshBackground = true;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
        /// </summary>
        /// <param name="refreshBackground">true to redraw the parent control bitmap.</param>
        public void RefreshControl(bool refreshBackground)
        {
            if (refreshBackground == true)
            {
                this.refreshBackground = true;
            }

            this.Refresh();
        }

        /// <summary>
        /// Paints the background of the control.
        /// A bitmap of the parent control is used as the background.
        /// </summary>
        /// <param name="e">A System.Windows.Forms.PaintEventArgs that contains information about the control to paint.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.Parent != null && this.refreshBackground)
            {
                if (this.backgroundBitmap != null)
                {
                    this.backgroundBitmap.Dispose();
                    this.backgroundBitmap = null;
                }

                Bitmap behind = new Bitmap(Parent.Width, Parent.Height);
                foreach (Control c in Parent.Controls)
                {
                    if (c is IPlotControl && c.Bounds.IntersectsWith(this.Bounds) && c != this)
                    {
                        c.DrawToBitmap(behind, c.Bounds);
                    }
                }

                this.backgroundBitmap = behind;
                this.refreshBackground = false;
            }

            if (this.backgroundBitmap != null)
            {
                e.Graphics.DrawImage(this.backgroundBitmap, -this.Left, -this.Top);
            }
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.Paint event.
        /// </summary>
        /// <param name="e">A System.Windows.Forms.PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

            string text = TrackerHitResult.ToString();
            Font font = new System.Drawing.Font("Arial", 10);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            OxyRect trackerBounds;

            if (TrackerHitResult.XAxis != null && TrackerHitResult.YAxis != null)
            {
                trackerBounds = new OxyRect(
                    TrackerHitResult.XAxis.ScreenMin.X,
                    TrackerHitResult.YAxis.ScreenMin.Y,
                    TrackerHitResult.XAxis.ScreenMax.X - TrackerHitResult.XAxis.ScreenMin.X,
                    TrackerHitResult.YAxis.ScreenMax.Y - TrackerHitResult.YAxis.ScreenMin.Y);
            }
            else
            {
                trackerBounds = new OxyRect();
            }

            Point trackerPosition = TrackerHitResult.Position.ToPoint(true);
            OxyRect textboxBounds = TrackerHitResult.PlotModel.PlotAndAxisArea;
            OxyRect textBox = new OxyRect(trackerPosition.X, trackerPosition.Y, textSize.Width, textSize.Height);
            int textboxOffset = 5;

            // default text box position is top and centered horizontally
            textBox.Left = trackerPosition.X - (textSize.Width / 2);
            textBox.Top = trackerPosition.Y - textSize.Height - textboxOffset;

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
                    new PointF((float)(textBox.Left + (textSize.Width / 2) - textboxOffset), (float)textBox.Bottom),
                    trackerPosition.ToScreenPoint().ToPoint(true),
                    new PointF((float)(textBox.Left + (textSize.Width / 2) + textboxOffset), (float)textBox.Bottom),
                };

                borderPoints = new PointF[]
                {
                    polyPoints[0],
                    new PointF((float)textBox.Left, (float)textBox.Bottom),
                    new PointF((float)textBox.Left, (float)textBox.Top),
                    new PointF((float)textBox.Right, (float)textBox.Top),
                    new PointF((float)textBox.Right, (float)textBox.Bottom),
                    polyPoints[2],
                };
            }
            else if (orientation == 1)
            {
                polyPoints = new PointF[] 
                {
                    new PointF((float)textBox.Right, (float)textBox.Top + (textSize.Height / 2) - textboxOffset),
                    trackerPosition.ToScreenPoint().ToPoint(true),
                    new PointF((float)textBox.Right, (float)textBox.Top + (textSize.Height / 2) + textboxOffset),
                };

                borderPoints = new PointF[]
                {
                    polyPoints[0],
                    new PointF((float)textBox.Right, (float)textBox.Top),
                    new PointF((float)textBox.Left, (float)textBox.Top),
                    new PointF((float)textBox.Left, (float)textBox.Bottom),
                    new PointF((float)textBox.Right, (float)textBox.Bottom),
                    polyPoints[2],
                };
            }
            else if (orientation == 2)
            {
                polyPoints = new PointF[] 
                {
                    new PointF((float)textBox.Left, (float)textBox.Top + (textSize.Height / 2) - textboxOffset),
                    trackerPosition.ToScreenPoint().ToPoint(true),
                    new PointF((float)textBox.Left, (float)textBox.Top + (textSize.Height / 2) + textboxOffset),
                };

                borderPoints = new PointF[]
                {
                    polyPoints[0],
                    new PointF((float)textBox.Left, (float)textBox.Top),
                    new PointF((float)textBox.Right, (float)textBox.Top),
                    new PointF((float)textBox.Right, (float)textBox.Bottom),
                    new PointF((float)textBox.Left, (float)textBox.Bottom),
                    polyPoints[2],
                };
            }
            else if (orientation == 3)
            {
                polyPoints = new PointF[] 
                {
                    new PointF((float)(textBox.Left + (textSize.Width / 2) - textboxOffset), (float)textBox.Top),
                    trackerPosition.ToScreenPoint().ToPoint(true),
                    new PointF((float)(textBox.Left + (textSize.Width / 2) + textboxOffset), (float)textBox.Top),
                };

                borderPoints = new PointF[]
                {
                    polyPoints[0],
                    new PointF((float)textBox.Left, (float)textBox.Top),
                    new PointF((float)textBox.Left, (float)textBox.Bottom),
                    new PointF((float)textBox.Right, (float)textBox.Bottom),
                    new PointF((float)textBox.Right, (float)textBox.Top),
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
            Brush textboxBrush = new SolidBrush(Color.FromArgb(128, Color.Yellow));
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
            e.Graphics.DrawString(text, font, Brushes.Black, (float)textBox.Left, (float)textBox.Top);

            textboxBrush.Dispose();
            textboxPen.Dispose();
            font.Dispose();
            base.OnPaint(e);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the System.Windows.Forms.Control
        /// and its child controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; 
        /// false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                if (this.backgroundBitmap != null)
                {
                    this.backgroundBitmap.Dispose();
                    this.backgroundBitmap = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
