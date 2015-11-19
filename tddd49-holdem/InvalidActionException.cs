using System;

namespace tddd49_holdem
{
	public class InvalidActionException: Exception
	{
		public InvalidActionException()
		{
		}

		public InvalidActionException(string message)
			: base(message)
		{
		}

		public InvalidActionException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}

