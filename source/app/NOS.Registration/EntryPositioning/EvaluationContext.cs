namespace NOS.Registration.EntryPositioning
{
	public class EvaluationContext
	{
		public int NumberOfAttendees
		{
			get;
			set;
		}

		public int ListEnd
		{
			get;
			set;
		}

		public int WaitingListEnd
		{
			get;
			set;
		}

		public IPluginConfiguration Configuration
		{
			get;
			set;
		}

		public User User
		{
			get;
			set;
		}

		public ILogger Logger
		{
			get;
			set;
		}
	}
}