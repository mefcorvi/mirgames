namespace MirGames.Specs.Users.CommandHandlers
{
    using System.Linq;

    using MirGames.Domain.Users.CommandHandlers;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public sealed class TransformMentionCommandHandlerTest
    {
        [Test]
        public void Transform_Of_One_Mention()
        {
            var command = new TransformMentionsCommand
            {
                Text = "Hello, @MeF Dei Corvi. How do you do?"
            };

            var result = this.Execute(command);
            Assert.AreEqual("Hello, <user id=\"10\">MeF Dei Corvi</user>. How do you do?", result);
        }

        [Test]
        public void Transform_Of_Complex_Mention()
        {
            var command = new TransformMentionsCommand
            {
                Text = "Hello, @Ch@$er. How do you do?"
            };

            var result = this.Execute(command);
            Assert.AreEqual("Hello, <user id=\"5\">Ch@$er</user>. How do you do?", result);
        }

        [Test]
        public void Transform_Of_Only_Mention()
        {
            var command = new TransformMentionsCommand
            {
                Text = "@MeF Dei"
            };

            var result = this.Execute(command);
            Assert.AreEqual("<user id=\"2\">MeF Dei</user>", result);
        }

        [Test]
        public void Transform_Of_Several_Mentions()
        {
            var command = new TransformMentionsCommand
            {
                Text = "@MeF DeiHello, @MeF Dei Corvi. How do you do? @MeF?"
            };

            var result = this.Execute(command);
            Assert.AreEqual("<user id=\"1\">MeF</user> DeiHello, <user id=\"10\">MeF Dei Corvi</user>. How do you do? <user id=\"1\">MeF</user>?", result);
        }

        [Test]
        public void Transform_Of_No_Mentions()
        {
            var command = new TransformMentionsCommand
            {
                Text = "Hello, @MeFF"
            };

            var result = this.Execute(command);
            Assert.AreEqual("Hello, @MeFF", result);
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The result.</returns>
        private string Execute(TransformMentionsCommand command)
        {
            var users = new[]
            {
                new User
                {
                    Login = "MeF",
                    Id = 1
                },
                new User
                {
                    Login = "MeF Dei Corvi",
                    Id = 10
                },
                new User
                {
                    Login = "MeF Dei",
                    Id = 2
                },
                new User
                {
                    Login = "User",
                    Id = 3
                },
                new User
                {
                    Login = "Dei",
                    Id = 4
                },
                new User
                {
                    Login = "Ch@$er",
                    Id = 5
                }
            };

            var readContextMock = new Mock<IReadContext>();
            readContextMock.Setup(m => m.Query<User>()).Returns(users.AsQueryable());

            var readContextFactoryMock = new Mock<IReadContextFactory>();
            readContextFactoryMock.Setup(m => m.Create()).Returns(readContextMock.Object);

            var handler = new TransformMentionsCommandHandler(readContextFactoryMock.Object);

            var result = handler.Execute(command, null, null);
            return result;
        }
    }
}
