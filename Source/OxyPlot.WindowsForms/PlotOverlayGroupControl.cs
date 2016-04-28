
namespace OxyPlot.WindowsForms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// A control that renders <see cref="PlotOverlayControl"/> objects over the <see cref="OxyPlot.WindowsForms.PlotView"/>.
    /// </summary>
    [Serializable]
    public class PlotOverlayGroupControl : Control
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotOverlayGroupControl"/> class.
        /// </summary>
        public PlotOverlayGroupControl()
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            BackColor = Color.Transparent;
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.Paint event.
        /// </summary>
        /// <param name="e">A System.Windows.Forms.PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            try
            {
                foreach (PlotOverlayControl overlay in this.Controls)
                {
                    if (!overlay.Visible)
                    {
                        continue;
                    }
                    overlay.Render(e);
                }
            }
            catch (Exception paintException)
            {
                var trace = new StackTrace(paintException);
                Debug.WriteLine(paintException);
                Debug.WriteLine(trace);
                using (var font = new Font("Arial", 10))
                {
                    e.Graphics.ResetTransform();
                    e.Graphics.DrawString(
                        "PlotOverlayGroupControl paint exception: " + paintException.Message, font, Brushes.Red,
                        Width * 0.5f,
                        Height * 0.5f,
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            foreach (Control item in this.Controls)
            {
                item.Size = this.Size;
            }
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
            // ReSharper disable once InconsistentNaming
            const int WM_NCHITTEST = 0x0084;
            // ReSharper disable once InconsistentNaming
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
