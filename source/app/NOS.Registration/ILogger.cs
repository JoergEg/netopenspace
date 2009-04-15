namespace NOS.Registration
{
	public interface ILogger
	{
		void Error(string message, string username);
		void Info(string message, string username);
		void Warning(string message, string username);
	}
}