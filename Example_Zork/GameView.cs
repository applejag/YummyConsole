using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YummyConsole;
using YummyConsole.Helpers;

namespace Example_Zork
{
	class GameView : Drawable
	{
		public readonly TextField inputField;
		public readonly Text textLog;
		
		public GameView()
		{
			inputField = new TextField();
			textLog = new Text();

			textLog.alignVertical = Text.Vertical.TopBottom;
			inputField.Submitted += InputFieldOnSubmitted;
		}

		private void InputFieldOnSubmitted(TextField textField, string text)
		{
			string response = Program.ZorkResponse(text);
			if (response != null)
				textLog.text += response + '\n';
		}

		protected override void Update()
		{
			inputField.Position = new Point(0, Yummy.BufferHeight - 1);
			inputField.maxWidth = Yummy.BufferWidth;

			textLog.Position = new Point(0, 0);
			textLog.maxWidth = Yummy.BufferWidth - 4;
			textLog.maxHeight = Yummy.BufferHeight - 1;
		}

		protected override void Draw()
		{}
	}
}
