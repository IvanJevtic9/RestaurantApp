using RestaurantApp.Core.Lib;

namespace RestaurantApp.Infrastructure
{
    public class Constants
    {
        public const string EMAIL_REGEX = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        public const string PASSWORD_REGEX = @"^(?=.*\d)(?=.*[!#$%&@'*+/=?^_()`><{|}~-])(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";
        public const string NAME_REGEX = @"([a-zA-Z]\s*)+";
    }

    public static class ResponseCodes
    {
        #region Success codes

        public const string SUCCESSFUL_REGISTRATION = "The Account has been created.";

        #endregion

        #region Error codes

        public static string INVALID_ACCOUNT_TYPE = "Invalid account type.";
        public const string EMAIL_ALREADY_REGISTERED = "This email has been already registered.";
        public const string PASSWORD_RULES = "The valid password must contain at least one number digit, one uppercase and lowercase letter and one special character.";
        public const string MUST_BE_EQUAL_PASSWORDS = "The confirmation password must be the same as the password.";
        public const string INVALID_DATE_OF_BIRTH = "Invalid date of birth.";
        public const string INVALID_LOGIN = "Invalid email or password.";

        #endregion

        public static string RequiredField(string propertyName)
        {
            return $"The {propertyName.ToCamelCase()} is required property.";
        }
        public static string InvalidValue(string propertyName)
        {
            return $"The {propertyName.ToCamelCase()} has invalid value.";
        }
        public static string LengthError(string propertyName, bool isMinCase, int length)
        {
            if (isMinCase)
            {
                return $"The length of {propertyName.ToCamelCase() } must be at least {length} characters.";
            }
            else
            {
                return $"The length of { propertyName.ToCamelCase() } must be {length} characters or fewer.";
            }
        }
    }
}
