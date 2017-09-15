using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YummyConsole.Helpers;

namespace YummyTesting.Helpers
{
	[TestClass]
	public class MathHelperTests
	{
		[TestMethod]
		public void WrapTester()
		{
			Assert.AreEqual(2, MathHelper.Wrap(2, max: 10));
			Assert.AreEqual(0, MathHelper.Wrap(11, max: 10));
			Assert.AreEqual(10, MathHelper.Wrap(10, min: 1, max: 10));
			Assert.AreEqual(10, MathHelper.Wrap(0, min: 1, max: 10));
			Assert.AreEqual(-1, MathHelper.Wrap(2, min: -1, max: 1));
			Assert.AreEqual(0, MathHelper.Wrap(-3, min: -1, max: 1));

			// char implicitly casted to int
			Assert.AreEqual('Y', MathHelper.Wrap('A' - 2, min: 'A', max: 'Z'));
		}
	}
}
