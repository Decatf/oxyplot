
namespace OxyPlot.WindowsForms.Tracker
{
    using System;

    public class TrackerModel
    {
        /// <summary>
        /// Represents a tracker.
        /// </summary>
        public TrackerModel()
        {
            this.BackgroundColor = OxyColor.FromAColor(192, OxyColors.LightBlue);
            this.Color = OxyColors.Black;
            this.TextColor = OxyColors.Black;
            this.TextOffset = 5;
            this.TextPadding = 5;
            this.BorderThickness = 1.0;

            this.Font = "Arial";
            this.FontSize = 12.0;
            this.FontWeight = 500.0;

            this.HorizontalLineStyle = LineStyle.Dot;
            this.VerticalLineStyle = LineStyle.Dot;
            this.HorizontalLineVisible = true;
            this.VerticalLineVisible = true;

            this.TrackerLocation = ScreenPoint.Undefined;
        }

        /// <summary>
        /// Gets or sets the tracker background color.
        /// </summary>
        public OxyColor BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the border thickness.
        /// </summary>
        public double BorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the tracker color.
        /// </summary>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        public string Font { get; set; }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        public double FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public OxyColor TextColor { get; set; }

        /// <summary>
        /// Gets or sets the text offset from the tracker position.
        /// </summary>
        public double TextOffset { get; set; }

        /// <summary>
        /// Gets or sets the text padding around the text box.
        /// </summary>
        public double TextPadding { get; set; }

        /// <summary>
        /// Gets or sets the horizontal line style.
        /// </summary>
        public LineStyle HorizontalLineStyle { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the horizontal tracker line.
        /// </summary>
        public bool HorizontalLineVisible { get; set; }

        /// <summary>
        /// Gets or sets the vertical line style.
        /// </summary>
        public LineStyle VerticalLineStyle { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the vertical tracker line.
        /// </summary>
        public bool VerticalLineVisible { get; set; }

        /// <summary>
        /// Gets or sets the tracker position.
        /// </summary>
        public ScreenPoint TrackerLocation { get; set; }

        /// <summary>
        /// Gets or sets the tracker area.
        /// </summary>
        public OxyRect TrackerBounds { get; set; }

        /// <summary>
        /// Gets or sets the tracker text.
        /// </summary>
        public string TrackerText { get; set; }

        /// <summary>
        /// Calculate the text box screen points.
        /// </summary>
        /// <param name="orientation">Text box orientation.</param>
        /// <param name="textBoxSize">Text box size.</param>
        /// <param name="textSize">Text size.</param>
        /// <param name="borderPoints">Border points.</param>
        /// <param name="polyPoints">Text box connector points.</param>
        private void CalculatePoints(int orientation, OxyRect textBoxSize, OxySize textSize, out ScreenPoint[] borderPoints, out ScreenPoint[] polyPoints)
        {
            var textboxOffset = this.TextOffset;
            var trackerPosition = this.TrackerLocation;

            // round the values
            var r = Math.Round(textBoxSize.Right);
            var l = Math.Round(textBoxSize.Left);
            var t = Math.Round(textBoxSize.Top);
            var b = Math.Round(textBoxSize.Bottom);
            textBoxSize = new OxyRect(l, t, r - l, b - t);

            if (orientation == 0)
            {
                polyPoints = new ScreenPoint[]
                {
                    new ScreenPoint((float) (textBoxSize.Left + (textSize.Width/2.0f) - textboxOffset), (float) textBoxSize.Bottom),
                    trackerPosition,
                    new ScreenPoint((float) (textBoxSize.Left + (textSize.Width/2.0f) + textboxOffset), (float) textBoxSize.Bottom),
                };

                borderPoints = new ScreenPoint[]
                {
                    trackerPosition,
                    new ScreenPoint((float) (textBoxSize.Left + (textSize.Width/2.0f) - textboxOffset), (float) textBoxSize.Bottom),
                    new ScreenPoint((float) textBoxSize.Left, (float) textBoxSize.Bottom),
                    new ScreenPoint((float) textBoxSize.Left, (float) textBoxSize.Top),
                    new ScreenPoint((float) textBoxSize.Right, (float) textBoxSize.Top),
                    new ScreenPoint((float) textBoxSize.Right, (float) textBoxSize.Bottom),
                    new ScreenPoint((float) (textBoxSize.Left + (textSize.Width/2.0f) + textboxOffset), (float) textBoxSize.Bottom),
                    trackerPosition,
                };
            }
            else if (orientation == 1)
            {
                polyPoints = new ScreenPoint[]
                {

                    new ScreenPoint(
                        (float) textBoxSize.Right,
                        (float) (textBoxSize.Top + (textSize.Height/2.0f) - textboxOffset)),
                    trackerPosition,
                    new ScreenPoint(
                        (float) textBoxSize.Right,
                        (float) (textBoxSize.Top + (textSize.Height/2.0f) + textboxOffset)),
                };

                borderPoints = new ScreenPoint[]
                {
                    trackerPosition,
                    new ScreenPoint((float) textBoxSize.Right, (float) (textBoxSize.Top + (textSize.Height/2.0f) - textboxOffset)),
                    new ScreenPoint((float) textBoxSize.Right, (float) textBoxSize.Top),
                    new ScreenPoint((float) textBoxSize.Left, (float) textBoxSize.Top),
                    new ScreenPoint((float) textBoxSize.Left, (float) textBoxSize.Bottom),
                    new ScreenPoint((float) textBoxSize.Right, (float) textBoxSize.Bottom),
                    new ScreenPoint((float) textBoxSize.Right, (float) (textBoxSize.Top + (textSize.Height/2.0f) + textboxOffset)),
                    trackerPosition,
                };
            }
            else if (orientation == 2)
            {
                polyPoints = new ScreenPoint[]
                {
                    new ScreenPoint((float) textBoxSize.Left, (float) (textBoxSize.Top + (textSize.Height/2.0f) - textboxOffset)),
                    trackerPosition,
                    new ScreenPoint((float) textBoxSize.Left, (float) (textBoxSize.Top + (textSize.Height/2.0f) + textboxOffset)),
                };

                borderPoints = new ScreenPoint[]
                {
                    trackerPosition,
                    new ScreenPoint((float) l, (float) (textBoxSize.Top + (textSize.Height/2.0f) - textboxOffset)),
                    new ScreenPoint((float) l, (float) textBoxSize.Top),
                    new ScreenPoint((float) r, (float) textBoxSize.Top),
                    new ScreenPoint((float) r, (float) textBoxSize.Bottom),
                    new ScreenPoint((float) l, (float) textBoxSize.Bottom),
                    new ScreenPoint((float) l, (float) (textBoxSize.Top + (textSize.Height/2.0f) + textboxOffset)),
                     trackerPosition,
                };
            }
            else if (orientation == 3)
            {
                polyPoints = new ScreenPoint[]
                {
                    new ScreenPoint((float) (textBoxSize.Left + (textSize.Width/2.0f) - textboxOffset), (float) textBoxSize.Top),
                    trackerPosition,
                    new ScreenPoint((float) (textBoxSize.Left + (textSize.Width/2.0f) + textboxOffset), (float) textBoxSize.Top),
                };

                borderPoints = new ScreenPoint[]
                {
                    trackerPosition,
                    new ScreenPoint((float) (textBoxSize.Left + (textSize.Width/2.0f) - textboxOffset), (float) textBoxSize.Top),
                    new ScreenPoint((float) textBoxSize.Left, (float) textBoxSize.Top),
                    new ScreenPoint((float) textBoxSize.Left, (float) textBoxSize.Bottom),
                    new ScreenPoint((float) textBoxSize.Right, (float) textBoxSize.Bottom),
                    new ScreenPoint((float) textBoxSize.Right, (float) textBoxSize.Top),
                    new ScreenPoint((float) (textBoxSize.Left + (textSize.Width/2.0f) + textboxOffset), (float) textBoxSize.Top),
                    trackerPosition,
                };
            }
            else
            {
                borderPoints = new ScreenPoint[0];
                polyPoints = new ScreenPoint[0];
            }
        }

        /// <summary>
        /// Renders the plot with the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public void Render(IRenderContext rc)
        {
            if (ScreenPoint.IsUndefined(this.TrackerLocation))
            {
                return;
            }

            string text = this.TrackerText;
            OxySize textSize = rc.MeasureText(text, this.Font, this.FontSize, this.FontWeight);

            double textboxOffset = this.TextOffset < 0 ? 0 : this.TextOffset;
            double textPadding = this.TextPadding < 0 ? 0 : this.TextPadding;

            OxySize textArea = new OxySize(
                textSize.Width + (float)(2.0 * textPadding),
                textSize.Height + (float)(2.0 * textPadding));
            textSize = textArea;

            OxyRect trackerBounds = this.TrackerBounds;
            ScreenPoint trackerPosition = this.TrackerLocation;
            OxyRect textboxBounds = this.TrackerBounds;

            // default text box position is top and centered horizontally
            double left = Math.Round(trackerPosition.X - (textSize.Width / 2));
            double top = Math.Round(trackerPosition.Y - textSize.Height - textboxOffset);

            // determine textbox orientation
            int orientation = 0; // top
            if (textboxBounds.Width > 0)
            {
                if (left + textSize.Width > textboxBounds.Right)
                {
                    orientation = 1; // left
                    left = trackerPosition.X - textSize.Width - textboxOffset;
                    top = trackerPosition.Y - (textSize.Height / 2);
                }

                if (left < textboxBounds.Left)
                {
                    orientation = 2; // right
                    left = trackerPosition.X + textboxOffset;
                    top = trackerPosition.Y - (textSize.Height / 2);
                }
            }

            if (textboxBounds.Height > 0)
            {
                if (top < textboxBounds.Top)
                {
                    left = trackerPosition.X - (textSize.Width / 2);
                    top = trackerPosition.Y + textboxOffset;
                    orientation = 3; // bottom
                }
            }

            OxyRect textBox = new OxyRect(left, top,
                Math.Round(textSize.Width),
                Math.Round(textSize.Height));

            // get the text box points
            ScreenPoint[] borderPoints;
            ScreenPoint[] polyPoints;
            this.CalculatePoints(orientation, textBox, textSize, out borderPoints, out polyPoints);

            // draw horizontal and vertical lines
            if (this.HorizontalLineVisible)
            {
                var pen = new OxyPen(this.Color, this.BorderThickness, this.HorizontalLineStyle);
                rc.DrawLine(
                    (float)trackerPosition.X,
                    (float)trackerBounds.Top,
                    (float)trackerPosition.X,
                    (float)trackerBounds.Bottom,
                    pen);
            }

            if (this.VerticalLineVisible)
            {
                var pen = new OxyPen(this.Color, this.BorderThickness, this.HorizontalLineStyle);
                rc.DrawLine(
                    (float)trackerBounds.Left,
                    trackerPosition.Y,
                    (float)trackerBounds.Right,
                    trackerPosition.Y,
                    pen);
            }

            // draw the textbox
            rc.DrawRectangle(textBox, this.BackgroundColor, this.Color, 0);
            rc.DrawLine(borderPoints, this.Color, this.BorderThickness, aliased: true);
            //rc.DrawLineSegments(borderPoints, this.Color, this.BorderThickness, aliased:true);
            //rc.DrawClippedRectangle(textBoxSize, textBoxSize, this.BackgroundColor, this.Color, this.BorderThickness);

            if (polyPoints.Length > 0)
            {
                //e.Graphics.FillPolygon(textboxBrush, polyPoints);
                //e.Graphics.DrawLines(textboxPen, polyPoints);
                //rc.DrawLineSegments(polyPoints.Take(2).ToArray(), this.Color, this.BorderThickness);
                //rc.DrawLineSegments(polyPoints.Skip(1).ToArray(), this.Color, this.BorderThickness);
                rc.DrawPolygon(polyPoints, this.BackgroundColor, this.BackgroundColor, 0);
            }

            //if (borderPoints.Length > 0)
            //{
            //    e.Graphics.DrawLines(textboxPen, borderPoints);
            //}

            // draw the text
            rc.DrawText(
                new ScreenPoint((float)(textBox.Left + textPadding), (float)(textBox.Top + textPadding)),
                text,
                this.TextColor,
                this.Font,
                this.FontSize,
                this.FontWeight);
        }
    }
}
