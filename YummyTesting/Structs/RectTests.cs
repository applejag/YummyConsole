using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YummyConsole;

namespace YummyTesting.Structs
{
	[TestClass]
	public class RectTests
	{
		[TestMethod]
		public void CreateFromXYWH()
		{
			var rect = new Rect(-3,-3,20,20);

			Assert.AreEqual(-3, rect.Left);
			Assert.AreEqual(-3, rect.Top);
			Assert.AreEqual(17, rect.Right);
			Assert.AreEqual(17, rect.Bottom);
		}

		[TestMethod]
		public void CreateFromPoints()
		{
			var point1 = new Point(-3,17);
			var point2 = new Point(17,-3);

			var rect = new Rect(point1, point2);

			Assert.AreEqual(-3, rect.x);
			Assert.AreEqual(-3, rect.y);
			Assert.AreEqual(20, rect.width);
			Assert.AreEqual(20, rect.height);
		}

		[TestMethod]
		public void CreateFromRectsParams()
		{
			var rect1 = new Rect(-3, 5, 2, 3);
			var rect2 = new Rect(4, -3, 13, 4);
			var rect3 = new Rect(new Point(0, 13), new Point(17, 17));

			var rect = new Rect(rects: new[] { rect1, rect2, rect3 });

			Assert.AreEqual(-3, rect.x);
			Assert.AreEqual(-3, rect.y);
			Assert.AreEqual(20, rect.width);
			Assert.AreEqual(20, rect.height);
		}

		[TestMethod]
		public void CreateFromRectsTwo()
		{
			var rect1 = new Rect(-3, 5, 2, 3);
			var rect2 = new Rect(new Point(0, -3), new Point(17, 17));

			var rect = new Rect(rect1: rect1, rect2: rect2);

			Assert.AreEqual(-3, rect.x);
			Assert.AreEqual(-3, rect.y);
			Assert.AreEqual(20, rect.width);
			Assert.AreEqual(20, rect.height);
		}
	}
}
