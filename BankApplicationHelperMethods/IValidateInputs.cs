using BankApplicationModels;

namespace BankApplicationHelperMethods
{
    public interface IValidateInputs
    {
        /// <summary>
        /// Validates the format of a bank ID.
        /// </summary>
        /// <param name="bankId">The bank ID to be validated.</param>
        /// <returns>A message indicating status of the bank ID Validation.</returns>
        Message ValidateBankIdFormat(string bankId);

        /// <summary>
        /// Validates the format of a branch ID.
        /// </summary>
        /// <param name="branchId">The branch ID to be validated.</param>
        /// <returns>A message indicating status of the branch ID Validation.</returns>
        Message ValidateBranchIdFormat(string branchId);

        /// <summary>
        /// Validates the format of an account ID.
        /// </summary>
        /// <param name="accountId">The account ID to be validated.</param>
        /// <returns>A message indicating status of the account ID Validation.</returns>
        Message ValidateAccountIdFormat(string accountId);

        /// <summary>
        /// Validates the format of a name.
        /// </summary>
        /// <param name="name">The name to be validated.</param>
        /// <returns>A message indicating status of the name Validation.</returns>
        Message ValidateNameFormat(string name);

        /// <summary>
        /// Validates the format of a password.
        /// </summary>
        /// <param name="password">The password to be validated.</param>
        /// <returns>A message indicating status of the password Validation</returns>
        Message ValidatePasswordFormat(string password);

        /// <summary>
        /// Validates the format of a phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number to be validated.</param>
        /// <returns>A message indicating status of the phone number Validation</returns>
        Message ValidatePhoneNumberFormat(string phoneNumber);

        /// <summary>
        /// Validates the format of an email address.
        /// </summary>
        /// <param name="emailId">The email address to be validated.</param>
        /// <returns>A message indicating status of the email address Validation.</returns>
        Message ValidateEmailIdFormat(string emailId);

        /// <summary>
        /// Validates the format of an account type.
        /// </summary>
        /// <param name="accountType">The account type to be validated.</param>
        /// <returns>A message indicating status of the account type Validation.</returns>
        Message ValidateAccountTypeFormat(int accountType);

        /// <summary>
        /// Validates the format of an address.
        /// </summary>
        /// <param name="address">The address to be validated.</param>
        /// <returns>A message indicating status of the address Validation.</returns>
        Message ValidateAddressFormat(string address);

        /// <summary>
        /// Validates the format of a date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to be validated.</param>
        /// <returns>A message indicating status of date of birth Validation.</returns>
        Message ValidateDateOfBirthFormat(string dateOfBirth);

        /// <summary>
        /// Validates the format of the gender value.
        /// </summary>
        /// <param name="gender">The gender value to validate.</param>
        /// <returns>A message indicating status of gender Validation.</returns>
        Message ValidateGenderFormat(int gender);

        /// <summary>
        /// Validates the format of the currency code.
        /// </summary>
        /// <param name="currencyCode">The currency code to validate.</param>
        /// <returns>A message indicating status of currency code Validation.</returns>
        Message ValidateCurrencyCodeFormat(string currencyCode);
    }
}