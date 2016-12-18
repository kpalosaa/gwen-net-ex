using System;

namespace Gwen.UnitTest.MonoGame.DesktopGL
{
	public static class Program
	{
		static void Main(string[] args)
		{
			using (var game = new UnitTestGame())
				game.Run();
		}
	}
}
