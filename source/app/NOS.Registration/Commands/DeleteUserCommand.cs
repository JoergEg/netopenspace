using System;

using NOS.Registration.DataAccess;
using NOS.Registration.Queries;

namespace NOS.Registration.Commands
{
	public class DeleteUserCommand : Command<DeleteUserMessage>
	{
		readonly IRegistrationRepository _registrationRepository;
		readonly ISynchronizer _synchronizer;

		public DeleteUserCommand(IRegistrationRepository registrationRepository,
		                         ISynchronizer synchronizer)
		{
			_registrationRepository = registrationRepository;
			_synchronizer = synchronizer;
		}

		protected override ReturnValue Execute(DeleteUserMessage message)
		{
			return _synchronizer.Lock(() =>
				{
					var user = _registrationRepository.Query(new UserByUserName(message.UserName));
					if (user == null)
					{
						return ReturnValue.Success();
					}

					try
					{
						_registrationRepository.Delete(user.UserName);
					}
					catch (Exception ex)
					{
						return ReturnValue.Fail(ex.Message);
					}

					return ReturnValue.Success();
				});
		}
	}

	public class DeleteUserMessage
	{
		public DeleteUserMessage(string userName)
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