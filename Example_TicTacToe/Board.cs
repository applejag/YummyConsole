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
		protected const int gridColumns = 3;
		protected const int gridRows = 3;
		protected const int gridPadding = 1;
		protected const int gridWidth = gridColumns * (Square.width + gridPadding) - 1;
		protected const int gridHeight = gridRows * (Square.height + gridPadding) - 1;

		protected Square[,] grid;
		public Point selected;
		public Player turn = Player.X;
		public GameState state = GameState.Playing;

		protected Text turnText;

		public Board() : base(null)
		{
			turnText = new Text("Turn: " + turn, this)
			{
				alignment = Text.Alignment.Center,
				LocalPosition = new Vector2(gridWidth*0.5f, -2),
			};
			grid = new Square[gridColumns,gridRows];

			for (int x = 0; x < gridColumns; x++)
			{
				for (int y = 0; y < gridRows; y++)
				{
					grid[x,y] = new Square(parent: this)
					{
						LocalPosition = new Point(x * (Square.width + gridPadding), y * (Square.height + gridPadding)),
					};
				}
			}
		}

		protected override void Update()
		{
			Position = new Vector2(Yummy.BufferWidth - gridWidth, Yummy.BufferHeight - gridHeight) * 0.5f + Vector2.Down;

			if (state != GameState.Playing)
			{
				if (Input.GetKeyDown(ConsoleKey.R)) ResetGame();
				return;
			}


			for (int x = 0; x < gridColumns; x++)
			{
				for (int y = 0; y < gridRows; y++)
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

					turnText.text = "Turn: " + turn;

					CheckGameState();
				}
			}
		}

		protected void MoveSelection(Point delta)
		{
			selected += delta;
			selected.x = MathHelper.Clamp(selected.x, 0, gridColumns - 1);
			selected.y = MathHelper.Clamp(selected.y, 0, gridRows - 1);
		}
		
		protected override void Draw()
		{
			Yummy.BackgroundColor = Color.CYAN;
			DrawGridLines();
		}

		protected void DrawGridLines()
		{
			const int paddingColInterval = Square.width + gridPadding;
			const int paddingRowInterval = Square.height + gridPadding;
			Point offset = ApproxPosition;

			// Vertical lines
			for (int col = 1; col < gridColumns; col++)
			{
				Point topmost = offset + Point.Right * (paddingColInterval * col - 1);
				Yummy.FillRay(topmost, Point.Down * gridHeight, '│');
			}

			// Horizontal lines
			for (int row = 1; row < gridRows; row++)
			{
				Point leftmost = offset + Point.Down * (paddingRowInterval * row - 1);
				Yummy.FillRay(leftmost, Point.Right * gridWidth, '─');

				// Crosses
				for (int col = 1; col < gridColumns; col++)
				{
					leftmost.x = offset.x + paddingColInterval * col - 1;
					Yummy.FillPoint(leftmost, '┼');
				}
			}
		}

		protected void CheckGameState()
		{
			Player winner = Player.None;
			List<Square> list = null;

			// Horizontal
			for (int col = 0; col < gridColumns; col++)
				if ((winner = GetWinner(list = SelectColumn(col))) != Player.None)
					goto ApplyWinningColor;
			
			// Vertical
			for (int row = 0; row < gridRows; row++)
				if ((winner = GetWinner(list = SelectRow(row))) != Player.None)
					goto ApplyWinningColor;

			// Diagonal
			if ((winner = GetWinner(list = SelectDiagonal(0, 1))) != Player.None)
				goto ApplyWinningColor;
			if ((winner = GetWinner(list = SelectDiagonal(gridRows - 1, -1))) != Player.None)
				goto ApplyWinningColor;

			// Game over?
			if ((list = SelectAll()).TrueForAll(s => s.Player != Player.None))
			{
				state = GameState.Draw;
				turnText.text = "Draw! Press [R] to reset";
				foreach (Square square in list)
					square.backgroundColor = Color.BLUE;
			}

			return;

		ApplyWinningColor:
			if (winner != Player.None && list != null)
			{
				foreach (Square square in list)
					square.backgroundColor = Color.LIGHT_GREEN;

				state = GameState.Win;
				turnText.text = turn + " won! Press [R] to reset";
			}
		}

		protected Player GetWinner(List<Square> list)
		{
			return list.Select(s => s.Player).Distinct().Count() == 1 ? list[0].Player : Player.None;
		}

		protected List<Square> SelectColumn(int column)
		{
			var list = new List<Square>(gridRows);
			for (int y = 0; y < gridRows; y++)
				list.Add(grid[column, y]);
			return list;
		}

		protected List<Square> SelectRow(int row)
		{
			var list = new List<Square>(gridColumns);
			for (int x = 0; x < gridColumns; x++)
				list.Add(grid[x, row]);
			return list;
		}

		protected List<Square> SelectDiagonal(int rowStart, int dy)
		{
			var list = new List<Square>(gridColumns);
			for (int x = 0; x < gridColumns; x++, rowStart += dy)
				list.Add(grid[x, rowStart]);
			return list;
		}

		protected List<Square> SelectAll()
		{
			var list = new List<Square>(gridColumns * gridRows);
			for (int x = 0; x < gridColumns; x++)
			{
				for (int y = 0; y < gridRows; y++)
					list.Add(grid[x, y]);
			}

			return list;
		}

		protected void ResetGame()
		{
			for (int x = 0; x < gridColumns; x++)
			{
				for (int y = 0; y < gridRows; y++)
				{
					grid[x, y].Player = Player.None;
				}
			}
			turn = Player.X;
			selected = Point.Zero;
			turnText.text = "Turn: " + turn;
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
