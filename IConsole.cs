using System;

namespace CodeJam
{
	public interface IConsole
	{
		string ReadLine();
		void WriteLine(string s);
		void WriteLine(int i);
	}

	public class ConsoleW : IConsole
	{
		public string ReadLine() => Console.ReadLine();
		public void WriteLine(string s) => Console.WriteLine(s);
		public void WriteLine(int i) => Console.WriteLine(i);
	}
}
