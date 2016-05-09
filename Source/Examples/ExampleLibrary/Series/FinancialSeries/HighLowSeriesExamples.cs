// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighLowSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("HighLowSeries"), Tags("Series")]
    public static class HighLowSeriesExamples
    {
        [Example("HighLowSeries")]
        public static PlotModel HighLowSeries()
        {
            var model = new PlotModel { Title = "HighLowSeries", LegendSymbolLength = 24 };
            var s1 = new HighLowSeries { Title = "HighLowSeries 1", Color = OxyColors.Black, };
            var r = new Random(314);
            var price = 100.0;
            for (int x = 0; x < 24; x++)
            {
                price = price + r.NextDouble() + 0.1;
                var high = price + 10 + r.NextDouble() * 10;
                var low = price - (10 + r.NextDouble() * 10);
                var open = low + r.NextDouble() * (high - low);
                var close = low + r.NextDouble() * (high - low);
                s1.Items.Add(new HighLowItem(x, high, low, open, close));
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MaximumPadding = 0.3, MinimumPadding = 0.3 });

            return model;
        }

        [Example("HighLowSeries (DateTime axis)")]
        public static PlotModel HighLowSeriesDateTimeAxis()
        {
            var m = new PlotModel();
            var t0 = new DateTime(2013, 05, 04);
            var t1 = t0.AddDays(1);
            var a = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = DateTimeAxis.ToDouble(t0.AddDays(-0.9)),
                Maximum = DateTimeAxis.ToDouble(t0.AddDays(1.9)),
                IntervalType = DateTimeIntervalType.Days,
                StringFormat = "yyyy-MM-dd"
            };
            m.Axes.Add(a);
            var s = new HighLowSeries
            {
                TrackerFormatString =
                    "X: {1:yyyy-MM-dd}\nHigh: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}"
            };

            var x0 = DateTimeAxis.ToDouble(t0);
            var x1 = DateTimeAxis.ToDouble(t1);
            s.Items.Add(new HighLowItem(x0, 14, 10, 13, 12.4));
            s.Items.Add(new HighLowItem(x1, 17, 8, 12.4, 16.3));
            m.Series.Add(s);

            return m;
        }
    }
}