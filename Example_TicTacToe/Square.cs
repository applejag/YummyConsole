using System;
using YummyConsole;

namespace Example_TicTacToe
{
	public class Square : Text
	{
		public const int WIDTH = 7;
		public const int HEIGHT = 5;
		private const string spriteX =
			"#     #\n" +
			" #   # \n" +
			"   #   \n" +
			" #   # \n" +
			"#     #\n";

		private const string spriteO =
			"  ###  \n" +
			"##   ##\n" +
			"#     #\n" +
			"##   ##\n" +
			"  ###  \n";

		private const string spriteNone =
			"       \n" +
			"       \n" +
			"       \n" +
			"       \n" +
			"       \n";

		private Player _player;
		public Player Player
		{
			get => _player;
			set => text = GetSprite(_player = value);
		}

		public Square(int x, int y, Drawable parent = null) : base(parent)
		{
			LocalPosition = new Point(x * (WIDTH + 1), y * (HEIGHT + 1));
			maxWidth = WIDTH;
			text = GetSprite(_player);
			ZDepth = 1;
			wordWrap = false;
		}

		protected override void Draw()
		{
			if (Player == Player.O)
				foregroundColor = Color.LIGHT_MAGENTA;
			else if (Player == Player.X)
				foregroundColor = Color.LIGHT_YELLOW;
			base.Draw();
		}

		private static string GetSprite(Player player)
		{
			switch (player)
			{
				case Player.O:
					return spriteO;
				case Player.X:
					return spriteX;
				default:
					return spriteNone;
			}
		}
	}

	public enum Player
	{
		None,
		X,
		O,
	}
}