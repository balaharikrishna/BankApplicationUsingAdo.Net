using BankApplicationModels;
using System.Text.RegularExpressions;

namespace BankApplicationHelperMethods
{
    public class ValidateInputs : IValidateInputs
    {
        private readonly Regex passwordRegex = new("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        private readonly Regex regex = new("^[a-zA-Z]+$");
        private readonly Regex phoneNumberRegex = new("^\\d{10}$");
        private readonly Regex emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        //private readonly Regex dateRegex = new(@"^(0[1-9]|[1-2][0-9]|3[0-1])/(0[1-9]|1[0-2])/(\d{4})$");// Enter a date(DD/MM/YYYY)
        private readonly Regex dateRegex = new(@"^(\d{4})/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])$");// Enter a date(YYYY/MM/DD)

        public Message ValidateBankIdFormat(string bankId)
        {
            Message message = new();
            if (bankId.Length != 12)
            {
                message.Result = false;
                message.ResultMessage = $"Bank Id:'{bankId}' Format Is Invalid.";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Bank Id Format:'{bankId}' Is Valid";
            }
            return message;
        }


        public Message ValidateBranchIdFormat(string branchId)
        {
            Message message = new();
            if (branchId.Length != 17)
            {
                message.Result = false;
                message.ResultMessage = $"Branch Id:'{branchId}' Format Is Invalid.";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Branch Id '{branchId}' Format Is Valid.";
            }
            return message;
        }


        public Message ValidateAccountIdFormat(string accountId)
        {
            Message message = new();
            if (accountId.Length != 17)
            {
                message.Result = false;
                message.ResultMessage = $"Account Id '{accountId}' cannot not be Empty and must be 17 Numbers";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Account Id '{accountId}' Validation Successful";
            }
            return message;
        }


        public Message ValidateNameFormat(string name)
        {
            Message message = new();
            bool isValidName = regex.IsMatch(name);
            if (!isValidName)
            {
                message.Result = false;
                message.ResultMessage = $"Name '{name}' Should contain Only Charecters";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Name '{name}' Validation Successful";
            }
            return message;
        }


        public Message ValidatePasswordFormat(string password)
        {
            Message message = new();
            bool isValidPassword = passwordRegex.IsMatch(password);
            if (!isValidPassword)
            {
                message.Result = false;
                message.ResultMessage = $"Password '{password}' Should contain One Capital,small,numbers,& Special Charecters";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Password '{password}' Validation Successful";
            }
            return message;
        }

        public Message ValidatePhoneNumberFormat(string phoneNumber)
        {
            Message message = new();
            bool isValidPhoneNumber = phoneNumberRegex.IsMatch(phoneNumber);
            if (!isValidPhoneNumber)
            {
                message.Result = false;
                message.ResultMessage = $"Customer Phone Number '{phoneNumber}' Should contain only numbers";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Phone Number '{phoneNumber}' Validation Successful";
            }
            return message;
        }


        public Message ValidateEmailIdFormat(string emailId)
        {
            Message message = new();
            bool isValidemail = emailRegex.IsMatch(emailId);
            if (!isValidemail)
            {
                message.Result = false;
                message.ResultMessage = $"Email Id '{emailId}' Should contain Valid Email Format Ex.example@example.com";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Email Id '{emailId}' Validation Successful";
            }
            return message;
        }


        public Message ValidateAccountTypeFormat(int accountType)
        {
            Message message = new();
            if (accountType != 1 && accountType != 2)
            {
                message.Result = false;
                message.ResultMessage = $"Entered Account Type '{accountType}'is Invalid., Please Enter Either 1 or 2";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Account Type '{accountType}' Validation Successful";
            }
            return message;
        }


        public Message ValidateAddressFormat(string address)
        {
            Message message = new();
            if (string.IsNullOrEmpty(address) || address.Length < 10)
            {
                message.Result = false;
                message.ResultMessage = $"Entered Address '{address}'., Address Should not be Empty or Less Than 10 Charecters.";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Address:'{address}' Validation Successful.";
            }
            return message;
        }


        public Message ValidateDateOfBirthFormat(string dateOfBirth)
        {
            Message message = new();
            bool isValidDob = dateRegex.IsMatch(dateOfBirth);
            if (!isValidDob)
            {
                message.Result = false;
                message.ResultMessage = $"Entered Date Of Birth '{dateOfBirth}' is Invalid.,Please Enter in DD/MM/YY Format.";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Date Of Birth '{dateOfBirth}' Validation Successful.";
            }
            return message;
        }


        public Message ValidateGenderFormat(int gender)
        {
            Message message = new();
            if (gender < 1 || gender > 3)
            {
                message.Result = false;
                message.ResultMessage = $"Entered '{gender}' is Invalid, Please Select Either 1 , 2 or 3";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Gender '{gender}' Validation Successful.";
            }
            return message;
        }

        public Message ValidateCurrencyCodeFormat(string currencyCode)
        {
            Message message = new();
            if (currencyCode is null || currencyCode.Length != 3 && currencyCode.Length != 4)
            {
                message.Result = false;
                message.ResultMessage = $"Entered CurrencyCode '{currencyCode}' Should Not be NUll & Should be 3 or 4 Charecters only";
            }
            else if (!regex.IsMatch(currencyCode))
            {
                message.Result = false;
                message.ResultMessage = $"Entered '{currencyCode}' Cannot Contain Special Charecters & Numbers";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Currency Code:'{currencyCode}' Format is Valid";
            }
            return message;
        }
    }
}
