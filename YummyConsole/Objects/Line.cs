using System;
using YummyConsole.Helpers;

namespace YummyConsole
{
	public class Line : Drawable
	{
		public Vector2 point1;
		public Vector2 point2;

		/// <summary>
		/// With <see cref="Space.Local"/>, the points are relative to this objects position.
		/// With <see cref="Space.Global"/>, the points are global positions and ignores the objects position.
		/// <para>Default: <see cref="Space.Global"/></para>
		/// </summary>
		public Space space = Space.Global;

		/// <summary>
		/// The character to be used as filling. Default: ' ' (space)
		/// </summary>
		public char fillChar = ' ';

		public Color? foregroundColor = null;
		public Color? backgroundColor = Color.GREY;
		
		public Line(Vector2 point1, Vector2 point2, Space space = Space.Global, Drawable parent = null) : base(parent)
		{
			this.point1 = point1;
			this.point2 = point2;
			this.space = space;
		}
		
		/// <summary>
		/// Gets or sets the difference from <see cref="point1"/> to <see cref="point2"/>.
		/// </summary>
		public Vector2 Delta
		{
			get => point2 - point1;
			set => point2 = point1 + value;
		}

		public Vector2 LocalPoint1 {
			get => space == Space.Local ? point1 : (point1 - Position);
			set => point1 = space == Space.Local ? value : (value + Position);
		}

		public Vector2 LocalPoint2 {
			get => space == Space.Local ? point2 : (point2 - Position);
			set => point2 = space == Space.Local ? value : (value + Position);
		}

		public Vector2 GlobalPoint1 {
			get => space == Space.Global ? point1 : (point1 + Position);
			set => point1 = space == Space.Global ? value : (value - Position);
		}

		public Vector2 GlobalPoint2 {
			get => space == Space.Global ? point2 : (point2 + Position);
			set => point2 = space == Space.Global ? value : (value - Position);
		}

		protected override void Update()
		{}

		protected override void Draw()
		{
			Yummy.ForegroundColor = foregroundColor;
			Yummy.BackgroundColor = backgroundColor;
			Yummy.FillLine((Point)GlobalPoint1, (Point)GlobalPoint2, ' ');
		}
	}
}