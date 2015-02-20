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
//   Represents a control that displays a tracker over a plot.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WindowsForms
{
    using System.Drawing.Text;
    using System.Windows.Forms;
    using Tracker;

    /// <summary>
    /// A control that displays a <see cref="TrackerHitResult"/>.
    /// </summary>
    public class TrackerView : PlotOverlayControl
    {
        /// <summary>
        /// The render context.
        /// </summary>
        private readonly GraphicsRenderContext renderContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerView"/> class.
        /// </summary>
        public TrackerView()
        {
            this.renderContext = new GraphicsRenderContext();
            this.TrackerModel = new TrackerModel();
        }

        public TrackerModel TrackerModel { get; set; }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.Paint event.
        /// </summary>
        /// <param name="e">A System.Windows.Forms.PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            TextRenderingHint origHint = e.Graphics.TextRenderingHint;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

            if (this.TrackerModel != null && this.renderContext != null)
            {
                this.renderContext.SetGraphicsTarget(e.Graphics);
                this.TrackerModel.Render(this.renderContext);
            }

            e.Graphics.TextRenderingHint = origHint;
        }
    }
}
