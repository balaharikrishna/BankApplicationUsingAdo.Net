using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;

namespace BankApplication
{
    public class CommonHelperService : ICommonHelperService
    {
        private string bankId = string.Empty;
        private string branchId = string.Empty;

        //It takes the menu options and Process It To and Fro.
        public ushort GetOption(string position)
        {
            while (true)
            {
                Console.WriteLine("Please Enter the Option");
                bool isValidOption = ushort.TryParse(Console.ReadLine(), out ushort Option);
                if (!isValidOption)
                {
                    Console.WriteLine("Option should contain only Positive Numbers.");
                    continue;
                }
                else
                {
                    string errorMessage = $"Entered Option:{Option} is Invalid.Please Select as per Above Options";
                    switch (position)
                    {
                        case "Main Page":
                            while (true)
                            {
                                if (Option == 0 || Option > 5)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;

                        case "Customer":
                            while (true)
                            {
                                if (Option > 7)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;

                        case "Staff":
                            while (true)
                            {
                                if (Option > 10)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;

                        case "Branch Manager":
                            while (true)
                            {
                                if (Option > 16)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;

                        case "Head Manager":
                            while (true)
                            {
                                if (Option > 7)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;

                        case "Reserve Bank Manager":
                            while (true)
                            {
                                if (Option > 4)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;
                    }
                }
                return Option;
            }
        }

        //Takes BankId Input and Validates It.
        public string GetBankId(string position, IBankService _bankService, IValidateInputs _validateInputs)
        {
            Message message;
            while (true)
            {
                Console.WriteLine($"Please Enter {position} BankId:");
                bankId = Console.ReadLine()?.ToUpper() ?? string.Empty;
                Message isValidbankId = _validateInputs.ValidateBankIdFormat(bankId);
                if (isValidbankId.Result)
                {
                    message = _bankService.AuthenticateBankIdAsync(bankId).Result;
                    if (message.Result)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine(isValidbankId.ResultMessage);
                    continue;
                }
            }
            return bankId;
        }

        //Takes BranchId Input and Validates It.
        public string GetBranchId(string position, IBranchService _branchService, IValidateInputs _validateInputs)
        {
            Message message;
            while (true)
            {
                Console.WriteLine($"Please Enter {position} BranchId:");
                branchId = Console.ReadLine()?.ToUpper() ?? string.Empty;
                Message isValidBranchId = _validateInputs.ValidateBranchIdFormat(branchId);
                if (isValidBranchId.Result)
                {
                    message = _branchService.AuthenticateBranchIdAsync(branchId).Result;
                    if (message.Result)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine(isValidBranchId.ResultMessage);
                    continue;
                }

            }
            return branchId;
        }

        //Validate  AccountId
        public string GetAccountId(string position, IValidateInputs _validateInputs)
        {
            string accountId;
            while (true)
            {
                Console.WriteLine($"Enter {position} AccountId:");
                accountId = Console.ReadLine()?.ToUpper() ?? string.Empty;
                Message isValidAccount = _validateInputs.ValidateAccountIdFormat(accountId);
                if (isValidAccount.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidAccount.ResultMessage);
                    continue;
                }
            }
            return accountId;
        }

        //Validate  Name
        public string GetName(string position, IValidateInputs _validateInputs)
        {
            string name;
            while (true)
            {
                Console.WriteLine($"Enter {position} Name:");
                name = Console.ReadLine()?.ToUpper().Replace(" ", "").ToUpper() ?? string.Empty;
                Message isValidName = _validateInputs.ValidateNameFormat(name);
                if (isValidName.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidName.ResultMessage);
                    continue;
                }

            }
            return name;
        }

        //Validate  Password
        public string GetPassword(string position, IValidateInputs _validateInputs)
        {
            string password;
            while (true)
            {
                Console.WriteLine($"Enter {position} Password:");
                password = Console.ReadLine() ?? string.Empty;
                Message isValidPassword = _validateInputs.ValidatePasswordFormat(password);
                if (isValidPassword.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidPassword.ResultMessage);
                    continue;
                }

            }
            return password;
        }

        //Validate  Phone Number
        public string GetPhoneNumber(string position, IValidateInputs _validateInputs)
        {
            string phoneNumber;
            while (true)
            {
                Console.WriteLine($"Enter {position} Phone Number:");
                phoneNumber = Console.ReadLine() ?? string.Empty;
                Message isValidPhoneNumber = _validateInputs.ValidatePhoneNumberFormat(phoneNumber);
                if (isValidPhoneNumber.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidPhoneNumber.ResultMessage);
                    continue;
                }

            }
            return phoneNumber;
        }

        // Validate  email
        public string GetEmailId(string position, IValidateInputs _validateInputs)
        {
            string emailId;
            while (true)
            {
                Console.WriteLine($"Enter {position} Email Id:");
                emailId = Console.ReadLine()?.ToUpper() ?? string.Empty;
                Message isValidEmail = _validateInputs.ValidateEmailIdFormat(emailId);
                if (isValidEmail.Result)
                {
                    break;
                }
                else
                {

                    Console.WriteLine(isValidEmail.ResultMessage);
                    continue;
                }
            }
            return emailId;
        }

        // Validate  account type
        public int GetAccountType(string position, IValidateInputs _validateInputs)
        {
            int accountType;
            while (true)
            {
                Console.WriteLine($"Enter {position} Account Type:");
                foreach (AccountType type in Enum.GetValues(typeof(AccountType)))
                {
                    Console.WriteLine("Enter {0} For {1}", (int)type, type.ToString());
                }

                accountType = int.Parse(Console.ReadLine() ?? string.Empty);
                Message isValidAccountType = _validateInputs.ValidateAccountTypeFormat(accountType);
                if (isValidAccountType.Result)
                {
                    break;

                }
                else
                {
                    Console.WriteLine(isValidAccountType.ResultMessage);
                    continue;
                }
            }
            return accountType;
        }

        // Validate address
        public string GetAddress(string position, IValidateInputs _validateInputs)
        {
            string address;
            while (true)
            {
                Console.WriteLine($"Enter {position} Address:");
                address = Console.ReadLine()?.ToUpper() ?? string.Empty;
                Message isValidAddress = _validateInputs.ValidateAddressFormat(address);
                if (isValidAddress.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidAddress.ResultMessage);
                    continue;
                }
            }
            return address;
        }

        // Validates  date of birth 
        public string GetDateOfBirth(string position, IValidateInputs _validateInputs)
        {
            string dateOfBirth;
            while (true)
            {
                Console.WriteLine($"Enter {position} Date Of Birth Ex:27/06/97(DD/MM/YY):");
                dateOfBirth = Console.ReadLine() ?? string.Empty;
                Message isValidDOB = _validateInputs.ValidateDateOfBirthFormat(dateOfBirth);
                if (isValidDOB.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidDOB.ResultMessage);
                    continue;
                }
            }
            return dateOfBirth;
        }

        //Checks whether the given GenderOption is valid or not.
        public int GetGender(string position, IValidateInputs _validateInputs)
        {
            int genderType;
            while (true)
            {
                Console.WriteLine($"Enter {position} Gender:");
                foreach (Gender gender in Enum.GetValues(typeof(Gender)))
                {
                    Console.WriteLine("Enter {0} For {1}", (int)gender, gender.ToString());
                }

                genderType = int.Parse(Console.ReadLine() ?? string.Empty);
                Message isValidGender = _validateInputs.ValidateGenderFormat(genderType);
                if (isValidGender.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidGender.ResultMessage);
                    continue;
                }
            }
            return genderType;
        }
        //Checks whether the given transferOptions are valid or not.
        public int ValidateTransferMethod()
        {
            int result;
            while (true)
            {
                Console.WriteLine("Enter the Transfer Method");
                foreach (TransferMethod method in Enum.GetValues(typeof(TransferMethod)))
                {
                    Console.WriteLine("Enter {0} For {1}", (int)method, method.ToString());
                }

                int choice = int.Parse(Console.ReadLine() ?? string.Empty);

                if (choice != 1 && choice != 2)
                {
                    Console.WriteLine("Enter as per Above Choices.");
                    continue;
                }
                else
                {
                    result = choice;
                    break;
                }
            }
            return result;
        }

        //Checks whether the given amount is valid or not.
        public decimal ValidateAmount()
        {
            decimal result;
            while (true)
            {
                Console.WriteLine("Enter the Amount");

                decimal amount = decimal.Parse(Console.ReadLine() ?? string.Empty);

                if (amount < 1)
                {
                    Console.WriteLine($"Entered Amount:{amount} is Invalid.,Please Enter a Valid Amout Ex:1");
                    continue;
                }
                else
                {
                    result = amount;
                    break;
                }
            }
            return result;
        }

        //checks the format and currency already exist or not 
        public string ValidateCurrency(string bankId, ICurrencyService _currencyService, IValidateInputs _validateInputs)
        {
            Message message;
            string result;
            while (true)
            {
                Console.WriteLine("Enter the Currency Name Ex:USD,KWD.");
                string currencyCode = Console.ReadLine()?.ToUpper() ?? string.Empty;
                message = _validateInputs.ValidateCurrencyCodeFormat(currencyCode);

                if (message.Result)
                {
                    message = _currencyService.ValidateCurrencyAsync(bankId, currencyCode).Result;
                    if (message.Result)
                    {
                        result = currencyCode;
                        break;
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    continue;
                }
            }
            return result;
        }

        //ToValidate TransactionId Format.
        public string ValidateTransactionIdFormat()
        {
            string result;
            while (true)
            {
                Console.WriteLine("Enter the Transaction Id");
                string transactionId = Console.ReadLine()?.ToUpper() ?? string.Empty;

                if (transactionId.Length == 23 && transactionId.Contains("TXN"))
                {
                    result = transactionId;
                    break;
                }
                else
                {
                    Console.WriteLine($"TransactionId:{transactionId} Is Invalid");
                    continue;
                }
            }
            return result;
        }


        //customer,staff,manager Login
        public void LoginAccountHolder(string level, IBankService bankService, IBranchService branchService, IValidateInputs validateInputs,
            ICustomerHelperService? customerHelperService = null, IStaffHelperService? staffHelperService = null, IManagerHelperService? managerHelperService = null,
            IHeadManagerHelperService? headManagerHelperService = null, IReserveBankManagerHelperService? reserveBankManagerHelperService = null, ICustomerService? customerService = null, IStaffService? staffService = null, IManagerService? managerService = null,
            IHeadManagerService? headManagerService = null, IReserveBankManagerService? reserveBankManagerService = null)
        {
            Message message = new();
            while (true)
            {
                string bankId = string.Empty;
                if (!level.Equals("Reserve Bank Manager"))
                {
                    bankId = GetBankId(level, bankService, validateInputs);
                    message = bankService.AuthenticateBankIdAsync(bankId).Result;
                }

                if (level.Equals("Reserve Bank Manager") || message.Result)
                {
                    if (!level.Equals("Head Manager") && !level.Equals("Reserve Bank Manager"))
                    {
                        message = branchService.IsBranchesExistAsync(bankId).Result;
                    }

                    if (level.Equals("Reserve Bank Manager") || level.Equals("Head Manager") || message.Result)
                    {
                        while (true)
                        {
                            string branchId = string.Empty;
                            if (!level.Equals("Head Manager") && !level.Equals("Reserve Bank Manager"))
                            {
                                branchId = GetBranchId(level, branchService, validateInputs);
                                message = branchService.AuthenticateBranchIdAsync(branchId).Result;
                            }

                            if (message.Result || level.Equals("Head Manager") || level.Equals("Reserve Bank Manager"))
                            {

                                if (level.Equals("Customer") && customerService is not null)
                                {
                                    message = customerService.IsCustomersExistAsync(branchId).Result;
                                }
                                else if (level.Equals("Staff") && staffService is not null)
                                {
                                    message = staffService.IsStaffExistAsync(branchId).Result;
                                }
                                else if (level.Equals("Branch Manager") && managerService is not null)
                                {
                                    message = managerService.IsManagersExistAsync(branchId).Result;
                                }
                                else if (level.Equals("Head Manager") && headManagerService is not null)
                                {
                                    message = headManagerService.IsHeadManagersExistAsync(bankId).Result;
                                }

                                if (level.Equals("Reserve Bank Manager") || message.Result)
                                {
                                    while (true)
                                    {
                                        string accountId = string.Empty;
                                        string ReserveBankManagerName = string.Empty;
                                        if (!level.Equals("Reserve Bank Manager"))
                                        {
                                            accountId = GetAccountId(level, validateInputs);
                                        }

                                        if (level.Equals("Customer") && customerService is not null)
                                        {
                                            message = customerService.IsAccountExistAsync(branchId, accountId).Result;
                                        }
                                        else if (level.Equals("Staff") && staffService is not null)
                                        {
                                            message = staffService.IsAccountExistAsync(branchId, accountId).Result;
                                        }
                                        else if (level.Equals("Branch Manager") && managerService is not null)
                                        {
                                            message = managerService.IsAccountExistAsync(branchId, accountId).Result;
                                        }
                                        else if (level.Equals("Head Manager") && headManagerService is not null)
                                        {
                                            message = headManagerService.IsHeadManagerExistAsync(bankId, accountId).Result;
                                        }
                                        else if (level.Equals("Reserve Bank Manager"))
                                        {
                                            ReserveBankManagerName = GetName(level, validateInputs);
                                            if (ReserveBankManagerName is not null)
                                            {
                                                message.Result = true;
                                            }
                                            else
                                            {
                                                message.Result = false;
                                            }
                                        }

                                        if (message.Result)
                                        {
                                            string password = GetPassword(level, validateInputs);

                                            if (level.Equals("Customer") && customerService is not null)
                                            {
                                                message = customerService.AuthenticateCustomerAccountAsync(branchId, accountId, password).Result;
                                            }
                                            else if (level.Equals("Staff") && staffService is not null)
                                            {
                                                message = staffService.AuthenticateStaffAccountAsync(branchId, accountId, password).Result;
                                            }
                                            else if (level.Equals("Branch Manager") && managerService is not null)
                                            {
                                                message = managerService.AuthenticateManagerAccountAsync(branchId, accountId, password).Result;
                                            }
                                            else if (level.Equals("Head Manager") && headManagerService is not null)
                                            {
                                                message = headManagerService.AuthenticateHeadManagerAsync(bankId, accountId, password).Result;
                                            }
                                            else if (level.Equals("Reserve Bank Manager") && reserveBankManagerService is not null)
                                            {
                                                message = reserveBankManagerService.AuthenticateManagerAccountAsync(accountId, password).Result;
                                            }

                                            if (message.Result)
                                            {
                                                while (true)
                                                {
                                                    Console.WriteLine("Choose From Below Menu Options");
                                                    if (level.Equals("Customer"))
                                                    {
                                                        foreach (CustomerOptions option in Enum.GetValues(typeof(CustomerOptions)))
                                                        {
                                                            Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                                        }
                                                    }
                                                    else if (level.Equals("Staff"))
                                                    {
                                                        foreach (StaffOptions option in Enum.GetValues(typeof(StaffOptions)))
                                                        {
                                                            Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                                        }
                                                    }
                                                    else if (level.Equals("Branch Manager"))
                                                    {
                                                        foreach (ManagerOptions option in Enum.GetValues(typeof(StaffOptions)))
                                                        {
                                                            Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                                        }
                                                    }
                                                    else if (level.Equals("Head Manager"))
                                                    {
                                                        foreach (HeadManagerOptions option in Enum.GetValues(typeof(HeadManagerOptions)))
                                                        {
                                                            Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                                        }
                                                    }
                                                    else if (level.Equals("Reserve Bank Manager"))
                                                    {
                                                        foreach (ReserveBankManagerOptions option in Enum.GetValues(typeof(ReserveBankManagerOptions)))
                                                        {
                                                            Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                                        }
                                                    }

                                                    Console.WriteLine("Enter 0 For Main Menu");
                                                    ushort selectedOption = GetOption(level);
                                                    if (selectedOption == 0)
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        if (level.Equals("Customer"))
                                                        {
                                                            customerHelperService?.SelectedOption(selectedOption, bankId, branchId, accountId);
                                                            continue;
                                                        }
                                                        else if (level.Equals("Staff"))
                                                        {
                                                            staffHelperService?.SelectedOption(selectedOption, bankId, branchId);
                                                            continue;
                                                        }
                                                        else if (level.Equals("Branch Manager"))
                                                        {
                                                            managerHelperService?.SelectedOption(selectedOption, bankId, branchId);
                                                            continue;
                                                        }
                                                        else if (level.Equals("Head Manager"))
                                                        {
                                                            headManagerHelperService?.SelectedOption(selectedOption, bankId);
                                                            continue;
                                                        }
                                                        else if (level.Equals("Reserve Bank Manager"))
                                                        {
                                                            reserveBankManagerHelperService?.SelectedOption(selectedOption);
                                                            continue;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine(message.ResultMessage);
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            continue;
                                        }
                                    }
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                                continue;
                            }
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        break;
                    }
                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    continue;
                }
            }
        }

        //Get AccountBalance
        public void GetCustomerAccountBalance(string branchId, ICustomerService _customerService, IValidateInputs _validateInputs, string level, string? userAccountId = null)
        {
            Message message;
            while (true)
            {
                message = _customerService.IsCustomersExistAsync(branchId).Result;
                if (message.Result || level.Equals("Customer"))
                {
                    string customerAccountId;
                    if (!level.Equals("Customer"))
                    {
                        customerAccountId = GetAccountId(Miscellaneous.customer, _validateInputs);
                        message = _customerService.IsAccountExistAsync(branchId, customerAccountId).Result;
                    }
                    else
                    {
                        customerAccountId = userAccountId!;
                    }

                    if (message.Result || level.Equals("Customer"))
                    {
                        message = _customerService.CheckAccountBalanceAsync(branchId, customerAccountId).Result;
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        break;
                    }
                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    break;
                }
            }
        }
        //Get Transaction History
        public void GetTransactoinHistory(string branchId, ICustomerService _customerService, IValidateInputs _validateInputs, ITransactionService _transactionService, string level, string? userAccountId = null)
        {
            Message message = new();
            while (true)
            {
                if (!level.Equals("Customer"))
                {
                    message = _customerService.IsCustomersExistAsync(branchId).Result;
                }

                if (message.Result || level.Equals("Customer"))
                {
                    string customerAccountId;
                    if (!level.Equals("Customer"))
                    {
                        customerAccountId = GetAccountId(Miscellaneous.customer, _validateInputs);
                        message = _customerService.IsAccountExistAsync(branchId, customerAccountId).Result;
                    }
                    else
                    {
                        customerAccountId = userAccountId!;
                    }
                    if (message.Result || level.Equals("Customer"))
                    {
                        message = _transactionService.IsTransactionsAvailableAsync(customerAccountId).Result;
                        if (message.Result)
                        {
                            IEnumerable<Transaction> transactions = _transactionService.GetAllTransactionHistory(customerAccountId).Result;
                            foreach (Transaction transaction in transactions)
                            {
                                Console.WriteLine();
                                Console.WriteLine(transaction);
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        break;
                    }
                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    break;
                }
            }
        }

        //Get Exchange Rates
        public void GetExchangeRates(string bankId, IBankService _bankService)
        {
            while (true)
            {
                IEnumerable<Currency> currencies = _bankService.GetExchangeRatesAsync(bankId).Result;
                if (currencies is not null)
                {
                    Console.WriteLine("Available Exchange Rates:");
                    foreach (Currency currency in currencies)
                    {
                        Console.WriteLine($"{currency.CurrencyCode} : {currency.ExchangeRate} Rupees");
                    }
                    Console.WriteLine();
                    break;
                }
                else
                {
                    Console.WriteLine("No Exchange Rates Available");
                    continue;
                }
            }
        }

        //Get Transaction Charges
        public void GetTransactionCharges(string branchId, IBranchService _branchService)
        {
            while (true)
            {
                IEnumerable<TransactionCharges> transactionCharges = _branchService.GetTransactionChargesAsync(branchId).Result;
                if (transactionCharges is not null)
                {
                    foreach (TransactionCharges charges in transactionCharges)
                    {
                        Console.WriteLine($"{charges.RtgsSameBank},{charges.RtgsOtherBank},{charges.ImpsSameBank},{charges.ImpsOtherBank}");
                    }
                    Console.WriteLine();
                    break;
                }
                else
                {
                    Console.WriteLine("Transaction Charges Not Available");
                    Console.WriteLine();
                    break;
                }
            }
        }

        //Transfer Amount
        public void TransferAmount(string userBankId, string userBranchId, IBranchService _branchService, IBankService _bankService, IValidateInputs _validateInputs, ICustomerService _customerService, string level, string? userAccountId = null)
        {
            Message message = new();
            while (true)
            {
                if (!level.Equals("Customer"))
                {
                    message = _customerService.IsCustomersExistAsync(branchId).Result;
                }
                if (message.Result || level.Equals("Customer"))
                {
                    string fromCustomerAccountId;
                    if (!level.Equals("Customer"))
                    {
                        fromCustomerAccountId = GetAccountId(Miscellaneous.customer, _validateInputs);
                        message = _customerService.IsAccountExistAsync(userBranchId, fromCustomerAccountId).Result;
                    }
                    else
                    {
                        fromCustomerAccountId = userAccountId!;
                    }

                    if (message.Result || level.Equals("Customer"))
                    {
                        int transferMethod = ValidateTransferMethod();
                        decimal amount = ValidateAmount();
                        message = _customerService.IsAccountExistAsync(userBranchId, fromCustomerAccountId).Result;

                        if (message.Result)
                        {
                            while (true)
                            {
                                string toCustomerBankId = GetBankId(Miscellaneous.toCustomer, _bankService, _validateInputs);
                                message = _bankService.AuthenticateBankIdAsync(toCustomerBankId).Result;
                                if (message.Result)
                                {
                                    string toCustomerBranchId = GetBranchId(Miscellaneous.toCustomer, _branchService, _validateInputs);
                                    message = _branchService.AuthenticateBranchIdAsync(toCustomerBranchId).Result;
                                    if (message.Result)
                                    {
                                        string toCustomerAccountId = GetAccountId(Miscellaneous.toCustomer, _validateInputs);
                                        message = _customerService.IsAccountExistAsync(toCustomerBranchId, toCustomerAccountId).Result;
                                        if (message.Result)
                                        {
                                            message = _customerService.TransferAmountAsync(userBankId, userBranchId, fromCustomerAccountId,
                                                toCustomerBankId, toCustomerBranchId, toCustomerAccountId, amount, (TransferMethod)transferMethod).Result;
                                            if (message.Result)
                                            {
                                                Console.WriteLine(message.ResultMessage);
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine(message.ResultMessage);
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(message.ResultMessage);
                                        continue;
                                    }

                                }
                                else
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    continue;
                                }
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        break;
                    }
                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    break;
                }
            }
        }

        public void OpenCustomerAccount(string branchId, ICustomerService _customerService, IValidateInputs _validateInputs)
        {
            Message message;
            while (true)
            {
                string customerName = GetName(Miscellaneous.customer, _validateInputs);
                string customerPassword = GetPassword(Miscellaneous.customer, _validateInputs);
                string customerPhoneNumber = GetPhoneNumber(Miscellaneous.customer, _validateInputs);
                string customerEmailId = GetEmailId(Miscellaneous.customer, _validateInputs);
                int customerAccountType = GetAccountType(Miscellaneous.customer, _validateInputs);
                string customerAddress = GetAddress(Miscellaneous.customer, _validateInputs);
                string customerDOB = GetDateOfBirth(Miscellaneous.customer, _validateInputs);
                int customerGender = GetGender(Miscellaneous.customer, _validateInputs);

                message = _customerService.OpenCustomerAccountAsync(branchId, customerName, customerPassword,
                customerPhoneNumber, customerEmailId, (AccountType)customerAccountType, customerAddress, customerDOB,
                (Gender)customerGender).Result;
                if (message.Result)
                {
                    Console.WriteLine(message.ResultMessage);
                    Console.WriteLine();
                    break;
                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    Console.WriteLine();
                    continue;
                }
            };
        }

        public void UpdateCustomerAccount(string branchId, IValidateInputs _validateInputs, ICustomerService _customerService)
        {
            Message message;
            while (true)
            {
                message = _customerService.IsCustomersExistAsync(branchId).Result;
                if (message.Result)
                {
                    string customerAccountId = GetAccountId(Miscellaneous.customer, _validateInputs);
                    message = _customerService.IsAccountExistAsync(branchId, customerAccountId).Result;
                    if (message.Result)
                    {
                        Customer customer = _customerService.GetCustomerByIdAsync(branchId, customerAccountId).Result;
                        Console.WriteLine($"Passbook Details:");
                        Console.WriteLine(customer.ToString());

                        string customerName;
                        while (true)
                        {
                            Console.WriteLine("Update Customer Name");
                            customerName = Console.ReadLine() ?? string.Empty;
                            if (!string.IsNullOrEmpty(customerName))
                            {
                                message = _validateInputs.ValidateNameFormat(customerName);
                                if (!message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    Console.WriteLine();
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        string customerPassword;
                        while (true)
                        {
                            Console.WriteLine("Update Customer Password");
                            customerPassword = Console.ReadLine() ?? string.Empty;
                            if (!string.IsNullOrEmpty(customerPassword))
                            {
                                message = _validateInputs.ValidatePasswordFormat(customerPassword);
                                if (!message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    Console.WriteLine();
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        string customerPhoneNumber;
                        while (true)
                        {
                            Console.WriteLine("Update Customer Phone Number");
                            customerPhoneNumber = Console.ReadLine() ?? string.Empty;
                            if (!string.IsNullOrEmpty(customerPhoneNumber))
                            {
                                message = _validateInputs.ValidatePhoneNumberFormat(customerPhoneNumber);
                                if (!message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    Console.WriteLine();
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        string customerEmailId;
                        while (true)
                        {
                            Console.WriteLine("Update Customer Email Id");
                            customerEmailId = Console.ReadLine() ?? string.Empty;
                            if (!string.IsNullOrEmpty(customerEmailId))
                            {
                                message = _validateInputs.ValidateEmailIdFormat(customerEmailId);
                                if (!message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    Console.WriteLine();
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        int customerAccountType;
                        while (true)
                        {
                            Console.WriteLine("Choose From Below Menu Options To Update");
                            foreach (AccountType option in Enum.GetValues(typeof(AccountType)))
                            {
                                Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                            }
                            bool isValid = int.TryParse(Console.ReadLine(), out customerAccountType);
                            if (isValid && customerAccountType != 0)
                            {
                                message = _validateInputs.ValidateAccountTypeFormat(customerAccountType);
                                if (!message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    Console.WriteLine();
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        string customerAddress;
                        while (true)
                        {
                            Console.WriteLine("Update Customer Address");
                            customerAddress = Console.ReadLine() ?? string.Empty;
                            if (!string.IsNullOrEmpty(customerAddress))
                            {
                                message = _validateInputs.ValidateAddressFormat(customerAddress);
                                if (!message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    Console.WriteLine();
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        string customerDOB;
                        while (true)
                        {
                            Console.WriteLine("Update Customer Date Of Birth");
                            customerDOB = Console.ReadLine() ?? string.Empty;
                            if (!string.IsNullOrEmpty(customerDOB))
                            {
                                message = _validateInputs.ValidateDateOfBirthFormat(customerDOB);
                                if (message.Result == false)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    Console.WriteLine();
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        int customerGender;
                        while (true)
                        {
                            Console.WriteLine("Choose From Below Menu Options To Update");
                            foreach (Gender option in Enum.GetValues(typeof(Gender)))
                            {
                                Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                            }
                            bool isValid = int.TryParse(Console.ReadLine(), out customerGender);
                            if (isValid && customerGender != 0)
                            {
                                message = _validateInputs.ValidateGenderFormat(customerGender);
                                if (message.Result == false)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    Console.WriteLine();
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        message = _customerService.UpdateCustomerAccountAsync(branchId, customerAccountId, customerName, customerPassword, customerPhoneNumber,
                            customerEmailId, (AccountType)customerAccountType, customerAddress, customerDOB, (Gender)customerGender).Result;
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine();
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine();
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        Console.WriteLine();
                        continue;
                    }

                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    Console.WriteLine();
                    break;
                }
            }
        }

        public void DeleteCustomerAccount(string branchId, ICustomerService _customerService, IValidateInputs _validateInputs)
        {
            Message message;
            while (true)
            {
                message = _customerService.IsCustomersExistAsync(branchId).Result;
                if (message.Result)
                {
                    string customerAccountId = GetAccountId(Miscellaneous.customer, _validateInputs);
                    message = _customerService.DeleteCustomerAccountAsync(branchId, customerAccountId).Result;
                    if (message.Result)
                    {
                        Console.WriteLine(message.ResultMessage);
                        Console.WriteLine();
                        break;
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        Console.WriteLine();
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    Console.WriteLine();
                    break;
                }

            }
        }

        public void RevertCustomerTransaction(string bankId, string branchId, ICustomerService _customerService,
            IValidateInputs _validateInputs, ITransactionService _transactionService, IBankService _bankService,
            IBranchService _branchService)
        {
            Message message;
            while (true)
            {
                message = _customerService.IsCustomersExistAsync(branchId).Result;
                if (message.Result)
                {
                    string fromCustomerAccountId = GetAccountId(Miscellaneous.customer, _validateInputs);
                    message = _customerService.IsAccountExistAsync(branchId, fromCustomerAccountId).Result;

                    if (message.Result)
                    {
                        message = _transactionService.IsTransactionsAvailableAsync(fromCustomerAccountId).Result;
                        if (message.Result)
                        {
                            string toCustomerBankId = GetBankId(Miscellaneous.toCustomer, _bankService, _validateInputs);
                            message = _bankService.AuthenticateBankIdAsync(toCustomerBankId).Result;
                            if (message.Result)
                            {
                                string toCustomerBranchId = GetBranchId(Miscellaneous.toCustomer, _branchService, _validateInputs);
                                message = _branchService.AuthenticateBranchIdAsync(toCustomerBranchId).Result;
                                if (message.Result)
                                {
                                    string toCustomerAccountId = GetAccountId(Miscellaneous.toCustomer, _validateInputs);

                                    message = _customerService.AuthenticateToCustomerAccountAsync(toCustomerBranchId, toCustomerAccountId).Result;
                                    if (message.Result)
                                    {
                                        string transactionId = ValidateTransactionIdFormat();
                                        message = _transactionService.RevertTransactionAsync(transactionId, bankId, branchId, fromCustomerAccountId, toCustomerBankId, toCustomerBranchId, toCustomerAccountId).Result;
                                        Console.WriteLine(message.ResultMessage);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine(message.ResultMessage);
                                        continue;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    continue;
                                }
                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    break;
                }
            }
        }

        public void DepositAmountInCustomerAccount(string branchId, ICustomerService _customerService,
            IValidateInputs _validateInputs, ICurrencyService _currencyService)
        {
            Message message;
            while (true)
            {
                message = _customerService.IsCustomersExistAsync(branchId).Result;
                if (message.Result)
                {
                    string customerAccountId = GetAccountId(Miscellaneous.customer, _validateInputs);
                    message = _customerService.IsAccountExistAsync(branchId, customerAccountId).Result;
                    if (message.Result)
                    {
                        decimal depositAmount = ValidateAmount();
                        string currencyCode = ValidateCurrency(bankId, _currencyService, _validateInputs);
                        message = _customerService.DepositAmountAsync(bankId, branchId, customerAccountId, depositAmount, currencyCode).Result;
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        break;
                    }
                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    break;
                }
            }
        }
    }
}
