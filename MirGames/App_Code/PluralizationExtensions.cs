namespace System.Web.Mvc
{
    /// <summary>
    /// Pluralizes the specified number.
    /// </summary>
    public static class PluralizationExtensions
    {
        /// <summary>
        /// Pluralizes the specified number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="singleItem">The single item.</param>
        /// <param name="fourItems">The four items.</param>
        /// <param name="fiveItems">The five items.</param>
        /// <param name="format">The format.</param>
        /// <returns>The pluralized result.</returns>
        public static string Pluralize(this int number, string singleItem, string fourItems, string fiveItems, string format = "{1}")
        {
            int originalNumber = number;
            string pluralization;
            number = number % 100;

            if (number >= 11 && number <= 19)
            {
                pluralization = fiveItems;
            }
            else
            {
                number = number % 10;
                switch (number)
                {
                    case 1:
                        pluralization = singleItem;
                        break;
                    case 2:
                    case 3:
                    case 4:
                        pluralization = fourItems;
                        break;
                    default:
                        pluralization = fiveItems;
                        break;
                }
            }

            return string.Format(format, originalNumber, pluralization);
        }
    }
}