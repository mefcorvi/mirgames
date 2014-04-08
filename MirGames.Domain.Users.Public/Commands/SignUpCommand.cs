namespace MirGames.Domain.Users.Commands
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// The sign up command.
    /// </summary>
    public class SignUpCommand : Command
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [DisplayName("Логин")]
        [Required(ErrorMessage = "Пожалуйста, введите логин")]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [DisplayName("E-mail")]
        [Required(ErrorMessage = "Пожалуйста, введите e-mail")]
        [EmailAddress(ErrorMessage = "Некорректный e-mail адрес")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [DisplayName("Пароль")]
        [Required(ErrorMessage = "Пожалуйста, введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
