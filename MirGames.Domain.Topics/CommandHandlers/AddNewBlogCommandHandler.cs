namespace MirGames.Domain.Topics.CommandHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Exceptions;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    public sealed class AddNewBlogCommandHandler : CommandHandler<AddNewBlogCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewBlogCommandHandler"/> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public AddNewBlogCommandHandler(IWriteContextFactory writeContextFactory)
        {
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override int Execute(AddNewBlogCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            using (var writeContext = this.writeContextFactory.Create())
            {
                if (writeContext.Set<Blog>().Any(b => b.Alias == command.Alias))
                {
                    throw new BlogAlreadyRegisteredException(command.Alias);
                }

                var blog = new Blog
                {
                    Alias = command.Alias,
                    Description = command.Description,
                    Title = command.Title
                };

                writeContext.Set<Blog>().Add(blog);
                writeContext.SaveChanges();

                return blog.Id;
            }
        }
    }
}