using RestaurantApp.Core.Lib;
using System.Collections.Generic;

namespace RestaurantApp.Core
{
    public class Constants
    {
        public static readonly List<string> AllowedImageExtensions = new List<string> { "JPG", "JPE", "BMP", "GIF", "PNG" };

        public const string EMAIL_REGEX = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        public const string PASSWORD_REGEX = @"^(?=.*\d)(?=.*[!#$%&@'*+/=?^_()`><{|}~-])(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";
        public const string NAME_REGEX = @"([a-zA-Z]\s*)+";

        public const string PROFILE_PICTURE_LOCATION = @"Images\ProfilePictures\";
        public const string MENU_BANNER_LOCATION = @"Images\MenuCategoryPictures\";
        public const string MENU_ITEM_LOCATION = @"Images\MenuItemPictures\";
        public const string GALLERY_IMAGE_LOCATION = @"Images\GalleryPictures\";
    }

    public static class ResponseCodes
    {
        #region Success codes

        public const string SUCCESSFUL_REGISTRATION = "The Account has been created.";
        public const string SUCCESSFUL_REQUEST = "Reguest has been executed successfully";

        #endregion

        #region Error codes

        public const string REQUIRED_LIST = "There must be at least one item.";
        public const string MUST_BE_POSITIVE = "Must be a positive number";
        public const string DOES_NOT_EXIST = "Item not found.";
        public const string ACCOUNT_DOES_NOT_EXIST = "Account does not exist.";
        public const string RESTAURANT_DOES_NOT_EXIST = "Restaurant does not exist.";
        public const string FORBIDEN_REQUEST = "User does not have permission to execute this request.";
        public const string PASSWORD_NOT_MATCHED = "Invalid old password.";
        public const string INVALID_ACCOUNT_TYPE = "Invalid account type.";
        public const string EMAIL_ALREADY_REGISTERED = "This email has been already registered.";
        public const string PASSWORD_RULES = "The valid password must contain at least one number digit, one uppercase and lowercase letter and one special character.";
        public const string MUST_BE_EQUAL_PASSWORDS = "The confirmation password must be the same as the password.";
        public const string INVALID_DATE_OF_BIRTH = "Invalid date of birth.";
        public const string INVALID_LOGIN = "Invalid email or password.";
        public const string INVALID_FILE_FORMAT = "Uploaded file has invalid format.";

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
