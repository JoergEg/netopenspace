using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Specifications;

using NOS.Registration.Abstractions;
using NOS.Registration.Model;
using NOS.Registration.Queries;

using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace NOS.Registration.Tests
{
	public class RepositorySpecs
	{
		protected static IFileReader Reader;
		protected static IRegistrationRepository Repository;
		protected static IEnumerable<User> Users;
		protected static IFileWriter Writer;

		Establish context = () =>
			{
				ISynchronizer synchronizer = MockRepository.GenerateStub<ISynchronizer>();
				synchronizer
					.Stub(x => x.Lock(Arg<Action>.Is.Anything))
					.WhenCalled(invocation => ((Action) invocation.Arguments.First()).Invoke());

				Reader = MockRepository.GenerateStub<IFileReader>();
				Writer = MockRepository.GenerateStub<IFileWriter>();

				Repository = new RegistrationRepository("file",
				                                        synchronizer,
				                                        Reader,
				                                        Writer);
			};
	}

	[Subject(typeof(RegistrationRepository))]
	public class When_all_users_are_loaded_and_no_users_exist : RepositorySpecs
	{
		Establish context = () => Reader
		                          	.Stub(x => x.Read("file"))
		                          	.Return(String.Empty);

		Because of = () => { Users = Repository.Query(new AllUsers()); };

		It should_return_an_empty_list =
			() => Users.ShouldBeEmpty();
	}

	[Subject(typeof(RegistrationRepository))]
	public class When_all_users_are_loaded_and_the_user_file_does_not_exists : RepositorySpecs
	{
		Establish context = () => Reader
		                          	.Stub(x => x.Read("file"))
		                          	.Return(null);

		Because of = () => { Users = Repository.Query(new AllUsers()); };

		It should_return_an_empty_list =
			() => Users.ShouldBeEmpty();
	}

	[Subject(typeof(RegistrationRepository))]
	public class When_all_users_are_loaded_and_users_exist : RepositorySpecs
	{
		Establish context = () => Reader
		                          	.Stub(x => x.Read("file"))
		                          	.Return(
		                          		"[ { UserName: \"torsten\", Data: { Xing: \"foo\", Twitter: \"bar\" } }, { UserName: \"alex\", Data: { Xing: \"baz\" } } ]");

		Because of = () => { Users = Repository.Query(new AllUsers()); };

		It should_load_the_user_list =
			() => Users.Count().ShouldEqual(2);

		It should_load_user_data =
			() => Users.First().Data.Xing.ShouldEqual("foo");

		It should_assign_null_to_nonexistent_values =
			() => Users.Skip(1).First().Data.Twitter.ShouldBeNull();
	}

	[Subject(typeof(RegistrationRepository))]
	public class When_a_user_is_saved : RepositorySpecs
	{
		Establish context = () => Reader
		                          	.Stub(x => x.Read("file"))
		                          	.Return("[ { UserName: \"torsten\", Data: { Xing: \"foo\", Twitter: \"bar\" } } ]");

		Because of = () => Repository.Save(new User("alex")
		                                   {
		                                   	Data =
		                                   		{
		                                   			Xing = "baz"
		                                   		}
		                                   });

		It should_add_the_user_to_the_list =
			() => Writer.AssertWasCalled(x => x.Write(null, null),
			                             o => o.Constraints(Is.Equal("file"), Text.Contains("alex")));

		It should_retain_the_original_collection =
			() => Writer.AssertWasCalled(x => x.Write(null, null),
			                             o => o.Constraints(Is.Equal("file"), Text.Contains("torsten")));
	}

	[Subject(typeof(RegistrationRepository))]
	public class When_an_existing_user_is_deleted : RepositorySpecs
	{
		Establish context = () => Reader
		                          	.Stub(x => x.Read("file"))
		                          	.Return(
		                          		"[ { UserName: \"torsten\", Data: { Xing: \"foo\", Twitter: \"bar\" } }, { UserName: \"alex\", Data: { Xing: \"baz\" } } ]");

		Because of = () => Repository.Delete("torsten");

		It should_remove_the_user_from_the_list =
			() => Writer.AssertWasCalled(x => x.Write(null, null),
			                             o => o.Constraints(Is.Equal("file"), Is.Matching<string>(x => !x.Contains("torsten"))));

		It should_retain_all_other_users =
			() => Writer.AssertWasCalled(x => x.Write(null, null),
			                             o => o.Constraints(Is.Equal("file"), Text.Contains("alex")));
	}

	[Subject(typeof(RegistrationRepository))]
	public class When_a_user_is_deleted_and_no_users_exist : RepositorySpecs
	{
		Establish context = () => Reader
		                          	.Stub(x => x.Read("file"))
		                          	.Return(null);

		Because of = () => Repository.Delete("torsten");

		It should_succeed =
			() => true.ShouldBeTrue();
	}
}