using NOS.Registration.DataAccess;
using NOS.Registration.Queries;

namespace NOS.Registration.Commands
{
	public class DeactivateUserCommand : Command<DeactivateUserMessage>
	{
		readonly IRegistrationRepository _registrationRepository;
		readonly ISynchronizer _synchronizer;

		public DeactivateUserCommand(IRegistrationRepository registrationRepository,
		                         ISynchronizer synchronizer)
		{
			_registrationRepository = registrationRepository;
			_synchronizer = synchronizer;
		}

		protected override ReturnValue Execute(DeactivateUserMessage message)
		{
			_synchronizer.Lock(() =>
				{
					var user = _registrationRepository.Query(new UserByUserName(message.UserName));
					if (user == null)
					{
						return;
					}

					if (!user.Active)
					{
						return;
					}

					user.Active = false;

					_registrationRepository.Save(user);
				});

			return ReturnValue.Success();
		}
	}

	public class DeactivateUserMessage
	{
		public DeactivateUserMessage(string userName)
		{
			UserName = userName;
		}

		public string UserName
		{
			get;
			private set;
		}
	}
}