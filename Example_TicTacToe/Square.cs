using System;
using YummyConsole;

namespace Example_TicTacToe
{
	public class Square : Text
	{
		public const int WIDTH = 7;
		public const int HEIGHT = 5;
		private const string spriteX =
			"#     #" +
			" #   # " +
			"   #   " +
			" #   # " +
			"#     #";

		private const string spriteO =
			"  ###  " +
			"##   ##" +
			"#     #" +
			"##   ##" +
			"  ###  ";

		private const string spriteNone =
			"       " +
			"       " +
			"       " +
			"       " +
			"       ";

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