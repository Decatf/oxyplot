
namespace OxyPlot.WindowsForms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Base class for creating plot overlays.
    /// </summary>
    [Serializable]
    public abstract class PlotOverlayControl : Control
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotOverlayControl"/> class.
        /// </summary>
        protected PlotOverlayControl()
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            this.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Render the overlay.
        /// </summary>
        /// <param name="e"></param>
        public void Render(PaintEventArgs e)
        {
            this.OnPaint(e);
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
