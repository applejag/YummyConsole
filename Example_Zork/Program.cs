using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YummyConsole;
using YummyConsole.Helpers;

namespace Example_Zork
{
	class Program
	{
		static void Main(string[] args)
		{

			var game = new GameView();
			game.textLog.text =
				"You walk into a strange room. The door seals behind you. You observe the size of the room, and the big white looking walls.\n- \"it looks like pillows\", you grunt to yourself while seeing it's not just the walls; the ceiling and floor has the identical texture.\n\nYou look behind you and see that the door is gone.\n- *gasp*, you gasp.\n\nYou start to wonder where it has gone. You haven't event moved a muscle since you walked in.\nAs your mind searches for explinations, your eyes spot some sprinkles on the floow. You take a closer look.\n\n- \"it's... it's candy..\", you whisper to yourself.\nAs you look around you see that there's a lot of candy laying around.\n\n";
			game.inputField.text = "pickup candy";

			Time.RunFrameTimer().Wait();

		}

		public static string ZorkResponse(string input)
		{
			string[] words = input.Trim().Split(' ');

			// Ensure not typed empty message
			if (words.Length == 0 || string.IsNullOrEmpty(words[0]))
				return null;

			if (words[0] == "pickup" || words[0] == "take")
			{
				if (words.Length < 2)
					return "Pickup what?";

				string obj = string.Join(" ", words.SubArray(1));
				if (obj.ToLower() == "candy")
					return RandomHelper.Float > 0.2f ? "Picked up candy!" : "*nom* oops I ate it";
				else
					return $"What do you mean \"{obj}\"? I cant see any around here...";

			}

			return "Sorry I dont understand.";
		}
	}
}
