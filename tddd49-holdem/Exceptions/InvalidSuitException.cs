using System;

namespace tddd49_holdem.Exceptions
{
	public class InvalidSuitException: Exception
	{
		public InvalidSuitException()
		{
		}

		public InvalidSuitException(string message)
			: base(message)
		{
		}

		public InvalidSuitException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}

