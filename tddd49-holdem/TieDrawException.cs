using System;

namespace tddd49_holdem
{
	public class TieDrawException: Exception
	{
		public TieDrawException()
		{
		}

		public TieDrawException(string message)
			: base(message)
		{
		}

		public TieDrawException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}

