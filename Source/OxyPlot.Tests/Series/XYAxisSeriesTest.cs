// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XYAxisSeriesTest.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="XYAxisSeries" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests.Series
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Provides unit tests for the <see cref="XYAxisSeries" /> class.
    /// </summary>
    [TestFixture]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    // ReSharper disable InconsistentNaming
    class XYAxisSeriesTest
    {
        [Test]
        public void FindWindowStartIndex()
        {
            LineSeries series = new LineSeries();

            List<DataPoint> points = new List<DataPoint>();
            var r = new Random(12);
            var y = r.Next(10, 30);
            for (int x = 0; x <= 100; x += 10)
            {
                points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
            }

            double targetX;
            int expectedIndex;
            int index;

            targetX = -100;
            expectedIndex = -1;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 0;
            expectedIndex = 0;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 50;
            expectedIndex = 5;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 100;
            expectedIndex = 10;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 200;
            expectedIndex = points.Count - 1;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);
        }

        [Test]
        public void FindWindowStartIndex_EmptyPoints()
        {
            LineSeries series = new LineSeries();

            List<DataPoint> points = new List<DataPoint>();
            var r = new Random(12);
            var y = r.Next(10, 30);
            for (int x = 0; x <= 100; x += 10)
            {
                points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
            }

            points[3] = DataPoint.Undefined;
            points[4] = DataPoint.Undefined;
            points[6] = DataPoint.Undefined;
            points[8] = DataPoint.Undefined;

            double targetX;
            int expectedIndex;
            int index;

            targetX = -100;
            expectedIndex = -1;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 0;
            expectedIndex = 0;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 60;
            expectedIndex = 5;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 61;
            expectedIndex = 5;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 70;
            expectedIndex = 7;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 89;
            expectedIndex = 7;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 100;
            expectedIndex = 10;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 200;
            expectedIndex = 10;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);
        }


        [Test]
        public void FindWindowStartIndex_UndefinedStart()
        {
            LineSeries series = new LineSeries();

            List<DataPoint> points = new List<DataPoint>();
            var r = new Random(12);
            var y = r.Next(10, 30);
            for (int x = 0; x <= 100; x += 10)
            {
                points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
            }

            points[0] = DataPoint.Undefined;

            double targetX;
            int expectedIndex;
            int index;

            targetX = -100;
            expectedIndex = -1;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 0;
            expectedIndex = -1;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 10;
            expectedIndex = 1;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);
        }

        [Test]
        public void FindWindowStartIndex_UndefinedEnd()
        {
            LineSeries series = new LineSeries();

            List<DataPoint> points = new List<DataPoint>();
            var r = new Random(12);
            var y = r.Next(10, 30);
            for (int x = 0; x <= 100; x += 10)
            {
                points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
            }

            points[10] = DataPoint.Undefined;

            double targetX;
            int expectedIndex;
            int index;

            targetX = 90;
            expectedIndex = 9;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 100;
            expectedIndex = 9;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 200;
            expectedIndex = 9;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);
        }

        [Test]
        public void FindWindowStartIndex_RandomIntervalSize()
        {
            LineSeries series = new LineSeries();

            List<DataPoint> points = new List<DataPoint>();
            var r = new Random(12);
            var y = r.Next(10, 30);
            var count = 0;
            for (int x = 0; count <= 11; x += r.Next(2, 100))
            {
                points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
                count++;
            }

            var actualTargetX = points[6].X;

            points[3] = DataPoint.Undefined;
            points[4] = DataPoint.Undefined;
            points[6] = DataPoint.Undefined;
            points[8] = DataPoint.Undefined;

            double targetX;
            int expectedIndex;
            int index;

            targetX = actualTargetX;
            expectedIndex = 5;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = actualTargetX + 1;
            expectedIndex = 5;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);
        }

        [Test]
        public void FindWindowEndIndex()
        {
            LineSeries series = new LineSeries();

            List<DataPoint> points = new List<DataPoint>();
            var r = new Random(12);
            var y = r.Next(10, 30);
            for (int x = 0; x <= 100; x += 10)
            {
                points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
            }

            double targetX;
            int expectedIndex;
            int index;

            targetX = -100;
            expectedIndex = 0;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 0;
            expectedIndex = 0;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 50;
            expectedIndex = 5;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 100;
            expectedIndex = 10;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 200;
            expectedIndex = -1;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);
        }

        [Test]
        public void FindWindowEndIndex_EmptyPoints()
        {
            LineSeries series = new LineSeries();

            List<DataPoint> points = new List<DataPoint>();
            var r = new Random(12);
            var y = r.Next(10, 30);
            for (int x = 0; x <= 100; x += 10)
            {
                points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
            }

            points[3] = DataPoint.Undefined;
            points[6] = DataPoint.Undefined;
            points[8] = DataPoint.Undefined;

            double targetX;
            int expectedIndex;
            int index;

            targetX = -100;
            expectedIndex = -1;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 0;
            expectedIndex = 0;
            index = series.FindWindowStartIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 35;
            expectedIndex = 4;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 60;
            expectedIndex = 7;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 61;
            expectedIndex = 7;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 70;
            expectedIndex = 7;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 80;
            expectedIndex = 9;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 90;
            expectedIndex = 9;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 100;
            expectedIndex = 10;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);
        }

        [Test]
        public void FindWindowEndIndex_UndefinedStart()
        {
            LineSeries series = new LineSeries();

            List<DataPoint> points = new List<DataPoint>();
            var r = new Random(12);
            var y = r.Next(10, 30);
            for (int x = 0; x <= 100; x += 10)
            {
                points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
            }

            points[0] = DataPoint.Undefined;

            double targetX;
            int expectedIndex;
            int index;

            targetX = -100;
            expectedIndex = 1;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 0;
            expectedIndex = 1;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 10;
            expectedIndex = 1;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);
        }

        [Test]
        public void FindWindowEndIndex_UndefinedEnd()
        {
            LineSeries series = new LineSeries();

            List<DataPoint> points = new List<DataPoint>();
            var r = new Random(12);
            var y = r.Next(10, 30);
            for (int x = 0; x <= 100; x += 10)
            {
                points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
            }

            points[9] = DataPoint.Undefined;
            points[10] = DataPoint.Undefined;

            double targetX;
            int expectedIndex;
            int index;

            targetX = 80;
            expectedIndex = 8;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 90;
            expectedIndex = -1;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 100;
            expectedIndex = -1;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);

            targetX = 200;
            expectedIndex = -1;
            index = series.FindWindowEndIndex(points, x => x.X, targetX, 0);
            Assert.AreEqual(expectedIndex, index, "TargetX {0} Expected {1} actual {2}", targetX, expectedIndex, index);
        }
    }
}
