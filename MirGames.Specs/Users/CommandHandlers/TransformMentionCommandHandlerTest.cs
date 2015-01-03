namespace MirGames.Specs.Users.CommandHandlers
{
    using System.Linq;

    using MirGames.Domain.Users.CommandHandlers;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public sealed class TransformMentionCommandHandlerTest
    {
        [Test]
        public void Transform_Of_One_Mention()
        {
            var query = new GetMentionsFromTextQuery
            {
                Text = "Hello, @MeF Dei Corvi. How do you do?"
            };

            var result = this.Execute(query);
            Assert.AreEqual("Hello, <user id=\"10\">MeF Dei Corvi</user>. How do you do?", result);
        }

        [Test]
        public void Transform_Of_Complex_Mention()
        {
            var query = new GetMentionsFromTextQuery
            {
                Text = "Hello, @Ch@$er. How do you do?"
            };

            var result = this.Execute(query);
            Assert.AreEqual("Hello, <user id=\"5\">Ch@$er</user>. How do you do?", result);
        }

        [Test]
        public void Transform_Of_Only_Mention()
        {
            var query = new GetMentionsFromTextQuery
            {
                Text = "@MeF Dei"
            };

            var result = this.Execute(query);
            Assert.AreEqual("<user id=\"2\">MeF Dei</user>", result);
        }

        [Test]
        public void Transform_Of_Several_Mentions()
        {
            var query = new GetMentionsFromTextQuery
            {
                Text = "@MeF DeiHello, @MeF Dei Corvi. How do you do? @MeF?"
            };

            var result = this.Execute(query);
            Assert.AreEqual("<user id=\"1\">MeF</user> DeiHello, <user id=\"10\">MeF Dei Corvi</user>. How do you do? <user id=\"1\">MeF</user>?", result);
        }

        [Test]
        public void Transform_Of_No_Mentions()
        {
            var query = new GetMentionsFromTextQuery
            {
                Text = "Hello, @MeFF"
            };

            var result = this.Execute(query);
            Assert.AreEqual("Hello, @MeFF", result);
        }

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The result.</returns>
        private MentionsInTextViewModel Execute(GetMentionsFromTextQuery query)
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

            var queryProcessorMock = new Mock<IQueryProcessor>();
            queryProcessorMock
                .Setup(m => m.Process<AuthorViewModel>(It.IsAny<ResolveAuthorsQuery>(), null))
                .Returns((ResolveAuthorsQuery q) => q.Authors);

            IQueryHandler handler = new GetMentionsFromTextQueryHandler(readContextFactoryMock.Object, queryProcessorMock.Object);
            return handler.Execute(query, null, null).Cast<MentionsInTextViewModel>().First();
        }
    }
}
