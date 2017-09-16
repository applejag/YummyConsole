using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YummyConsole;

namespace Example_TicTacToe
{
	class Program
	{
		static void Main(string[] args)
		{
			
			var game = new Board();

			Time.RunFrameTimer().Wait();

		}
	}
}
