using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YummyConsole.Helpers;

namespace YummyTesting.Helpers
{
	[TestClass]
	public class StringHelperTests
	{

		[TestMethod]
		public void NewLineTester()
		{
			const string input = "hello\n\rworld\rfoo\r\nbar\nmoo";
			const string expected = "hello\nworld\nfoo\nbar\nmoo";

			string actual = input.FixNewLines();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void WordWrapTester()
		{
			var lines = "hello there mild adventurer".WordWrap(15);

			Assert.AreEqual(2, lines.Length);
			Assert.AreEqual("hello there", lines[0]);
			Assert.AreEqual("mild adventurer", lines[1]);
		}

		[TestMethod]
		public void WordWrapUnicodeTester()
		{
			var lines = "fågelskaftstränare".WordWrap(15);
			string prettyString = lines.ToPrettyString();

			Assert.AreEqual(2, lines.Length, prettyString);
			Assert.AreEqual("fågelskaftsträ-", lines[0], prettyString);
			Assert.AreEqual("nare", lines[1], prettyString);
		}

		[TestMethod]
		public void WordWrapTooLongTester()
		{
			var lines = "12345678901234567890".WordWrap(9);
			string prettyString = lines.ToPrettyString();

			Assert.AreEqual(3, lines.Length, prettyString);
			Assert.AreEqual("12345678-", lines[0], prettyString);
			Assert.AreEqual("90123456-", lines[1], prettyString);
			Assert.AreEqual("7890", lines[2], prettyString);
		}

		[TestMethod]
		public void WordWrapNewLineTester()
		{
			var lines = "abc\ndefghijklmnopq\nrstuvxyz".WordWrap(9);
			string prettyString = lines.ToPrettyString();

			Assert.AreEqual(4, lines.Length, prettyString);
			Assert.AreEqual("abc", lines[0], prettyString);
			Assert.AreEqual("defghijk-", lines[1], prettyString);
			Assert.AreEqual("lmnopq", lines[2], prettyString);
			Assert.AreEqual("rstuvxyz", lines[3], prettyString);
		}

		[TestMethod]
		public void TrimNewLinesStartTester()
		{
			string text = "abc\ndef\nghi\nijk\nlmn".TrimRowsFromStart(3);

			Assert.AreEqual("ghi\nijk\nlmn", text);
		}

		[TestMethod]
		public void TrimNewLinesEndTester()
		{
			string text = "abc\ndef\nghi\nijk\nlmn".TrimRowsFromEnd(3);

			Assert.AreEqual("abc\ndef\nghi", text);
		}
	}
}
