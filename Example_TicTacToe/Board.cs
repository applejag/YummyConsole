using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YummyConsole;
using YummyConsole.Helpers;

namespace Example_TicTacToe
{
	public class Board : Drawable
	{
		protected const int gridWidth = 3 * (Square.WIDTH + 1) - 1;
		protected const int gridHeight = 3 * (Square.HEIGHT + 1) - 1;

		protected Square[,] grid = new Square[3, 3];
		public Point selected = new Point(1, 1);
		public Player turn = Player.X;
		public GameState state = GameState.Playing;

		protected Text infoText;

		public Board() : base(null)
		{
			// Create text header
			infoText = new Text("Turn: " + turn, this)
			{
				alignHorizontal = Text.Horizontal.Center,
				LocalPosition = new Vector2(gridWidth*0.5f, -2),
			};

			// Fill grid
			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					grid[x, y] = new Square(x, y, parent: this);
				}
			}
		}

		protected override void Update()
		{
			print(state);
			print($"grid[{selected.x}, {selected.y}].Player = {grid[selected.x, selected.y].Player}");

			Position = new Vector2(Yummy.BufferWidth - gridWidth, Yummy.BufferHeight - gridHeight) * 0.5f + Vector2.Down;

			if (state != GameState.Playing)
			{
				if (Input.GetKeyDown(ConsoleKey.R)) ResetGame();
				return;
			}
			
			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					grid[x, y].backgroundColor = (selected.x == x && selected.y == y) ? Color.YELLOW : Color.BLACK;
				}
			}
			
			if (Input.GetKeyDown(ConsoleKey.UpArrow)) MoveSelection(Point.Up);
			if (Input.GetKeyDown(ConsoleKey.DownArrow)) MoveSelection(Point.Down);
			if (Input.GetKeyDown(ConsoleKey.LeftArrow)) MoveSelection(Point.Left);
			if (Input.GetKeyDown(ConsoleKey.RightArrow)) MoveSelection(Point.Right);

			if (grid[selected.x, selected.y].Player == Player.None)
			{
				if (Input.GetKeyDown(ConsoleKey.Enter) || Input.GetKeyDown(ConsoleKey.Spacebar))
				{
					grid[selected.x, selected.y].Player = turn;
					turn = turn == Player.O ? Player.X : Player.O;

					infoText.text = "Turn: " + turn;

					CheckGameState();
				}
			}
		}

		protected void MoveSelection(Point delta)
		{
			selected += delta;
			selected.x = MathHelper.Clamp(selected.x, 0, 2);
			selected.y = MathHelper.Clamp(selected.y, 0, 2);
		}
		
		protected override void Draw()
		{
			Yummy.BackgroundColor = Color.GREY;
			Yummy.ForegroundColor = Color.BLACK;
			DrawGridLines();
		}

		protected void DrawGridLines()
		{
			const int paddingColInterval = Square.WIDTH + 1;
			const int paddingRowInterval = Square.HEIGHT + 1;
			Point offset = ApproxPosition;
			
			// Vertical lines
			for (int col = 1; col < 3; col++)
			{
				Point topmost = offset + Point.Right * (paddingColInterval * col - 1);
				Yummy.FillRay(topmost, Point.Down * gridHeight, '│');
			}

			// Horizontal lines
			for (int row = 1; row < 3; row++)
			{
				Point leftmost = offset + Point.Down * (paddingRowInterval * row - 1);
				Yummy.FillRay(leftmost, Point.Right * gridWidth, '─');

				// Crosses
				for (int col = 1; col < 3; col++)
				{
					leftmost.x = offset.x + paddingColInterval * col - 1;
					Yummy.FillPoint(leftmost, '┼');
				}
			}
		}

		protected void CheckGameState()
		{
			
			Player winner = GetWinner();

			if (winner != Player.None)
			{
				state = GameState.Win;
				infoText.text = turn + " won! Press [R] to reset";
			}
			else if (IsBoardFull())
			{
				state = GameState.Draw;
				infoText.text = "Draw! Press [R] to reset";
				foreach (Square square in grid)
					square.backgroundColor = Color.BLUE;
			}
		}

		protected bool IsBoardFull()
		{
			foreach (Square square in grid)
				if (square.Player == Player.None) return false;

			return true;
		}

		protected Player GetWinner()
		{
			if (IsPlayerWinner(Player.X)) return Player.X;
			if (IsPlayerWinner(Player.O)) return Player.O;
			return Player.None;
		}

		protected bool IsPlayerWinner(Player player)
		{
			// This forces it to check every option, in case we won in multiple ways
			bool won = PlayerWonAnyColumn(player);
			if (PlayerWonAnyRow(player)) won = true;
			if (PlayerWonAnyDiagonal(player)) won = true;

			return won;
		}

		protected bool PlayerWonAnyColumn(Player player)
		{
			for (int column = 0; column < 3; column++)
				if (PlayerWonColumn(player, column)) return true;

			return false;
		}

		protected bool PlayerWonColumn(Player player, int column)
		{
			for (int y = 0; y < 3; y++)
				if (grid[column, y].Player != player) return false;

			// Player won
			for (int y = 0; y < 3; y++)
				grid[column, y].backgroundColor = Color.LIGHT_GREEN;

			return true;
		}

		protected bool PlayerWonAnyRow(Player player)
		{
			for (int row = 0; row < 3; row++)
				if (PlayerWonRow(player, row)) return true;

			return false;
		}

		protected bool PlayerWonRow(Player player, int row)
		{
			for (int x = 0; x < 3; x++)
				if (grid[x, row].Player != player) return false;

			// Player won
			for (int x = 0; x < 3; x++)
				grid[x, row].backgroundColor = Color.LIGHT_GREEN;

			return true;
		}

		protected bool PlayerWonAnyDiagonal(Player player)
		{
			return PlayerWonDiagonal(player, 0, 1)
			       || PlayerWonDiagonal(player, 2, -1);
		}

		protected bool PlayerWonDiagonal(Player player, int rowStart, int dy)
		{
			for (int x = 0, y = rowStart; x < 3; x++, y += dy)
				if (grid[x, y].Player != player) return false;

			// Player won
			for (int x = 0, y = rowStart; x < 3; x++, y += dy)
				grid[x, y].backgroundColor = Color.LIGHT_GREEN;

			return true;
		}

		protected void ResetGame()
		{
			foreach (Square square in grid)
				square.Player = Player.None;

			turn = Player.X;
			selected = Point.One;
			infoText.text = "Turn: " + turn;
			state = GameState.Playing;
		}

		public enum GameState
		{
			Playing,
			Win,
			Draw
		}
	}
}
