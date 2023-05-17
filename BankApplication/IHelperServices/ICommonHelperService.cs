using BankApplicationHelperMethods;
using BankApplicationServices.IServices;

namespace BankApplication.IHelperServices
{
    public interface ICommonHelperService
    {
        /// <summary>
        /// Retrieves the account ID.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User</param>
        /// <param name="_validateInputs">The input validator.</param>
        /// <returns>The account ID.</returns>
        string GetAccountId(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the account type.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User</param>
        /// <param name="_validateInputs">The input validator.</param>
        /// <returns>The account type.</returns>
        int GetAccountType(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the address.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User</param>
        /// <param name="_validateInputs">The input validator.</param>
        /// <returns>The address.</returns>
        string GetAddress(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the bank ID, using a bank service and after validating the inputs.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User</param>
        /// <param name="bankService">The bank service to use to retrieve the bank ID</param>
        /// <param name="_validateInputs">The input validator.</param>
        /// <returns>The bank ID.</returns>
        string GetBankId(string position, IBankService bankService, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the branch ID, using a branch service and after validating the inputs.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User</param>
        /// <param name="branchService">The branch service to use to retrieve the branch ID.</param>
        /// <param name="_validateInputs">The input validator.</param>
        /// <returns>The branch ID.</returns>
        string GetBranchId(string position, IBranchService branchService, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the date of birth of a Account Holder.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">The validation service to be used to validate inputs.</param>
        /// <returns>The date of birth of the Account Holder as a string.</returns>
        string GetDateOfBirth(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the email address of a Account Holder.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">The validation service to be used to validate inputs.</param>
        /// <returns>The email address of the Account Holder as a string.</returns>
        string GetEmailId(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the gender of a Account Holder. 
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">The validation service to be used to validate inputs.</param>
        /// <returns>The gender of the Account Holder as an integer.</returns>
        int GetGender(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the name of a Account Holder.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">The validation service to be used to validate inputs.</param>
        /// <returns>The name of the Account Holder as a string.</returns>
        string GetName(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves an option based on selection of Choices.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <returns>The option as an unsigned short integer.</returns>
        ushort GetOption(string position);

        /// <summary>
        /// Retreives an Option based on User Choice Selection.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">An object used to validate the inputs.</param>
        /// <returns>The generated password as a string.</returns>
        string GetPassword(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Gets a phone number.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">An object used to validate the inputs.</param>
        /// <returns>The generated phone number as a string.</returns>
        string GetPhoneNumber(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Validates the amount entered by the user.
        /// </summary>
        /// <returns>The validated amount as a decimal.</returns>
        decimal ValidateAmount();

        /// <summary>
        /// Validates the currency entered by the user based on the bank ID and a currency service.
        /// </summary>
        /// <param name="bankId">The ID of the bank for which the currency is being validated.</param>
        /// <param name="currencyService">The currency service used to validate the currency.</param>
        /// <param name="_validateInputs">An object used to validate the inputs.</param>
        /// <returns>The validated currency as a string.</returns>
        string ValidateCurrency(string bankId, ICurrencyService currencyService, IValidateInputs _validateInputs);

        /// <summary>
        /// Validates the format of the transaction ID entered by the user.
        /// </summary>
        /// <returns>The validated transaction ID format as a string.</returns>
        string ValidateTransactionIdFormat();

        /// <summary>
        /// Validates the transfer method entered by the user.
        /// </summary>
        /// <returns>The validated transfer method as an integer.</returns>
        int ValidateTransferMethod();

        /// <summary>
        /// Logs in an account holder at a specified level, given the necessary services and input validation.
        /// </summary>
        /// <param name="level">The access level of the account holder.</param>
        /// <param name="bankService">The service used to access bank-related data.</param>
        /// <param name="branchService">The service used to access branch-related data.</param>
        /// <param name="validateInputs">The Service used to validate the inputs.</param>
        /// <param name="customerHelperService">The service used to perform operations related to customers.</param>
        /// <param name="staffHelperService">The service used to perform operations related to staff.</param>
        /// <param name="managerHelperService">The service used to perform operations related to managers.</param>
        /// <param name="headManagerHelperService">The service used to perform operations related to head managers.</param>
        /// <param name="reserveBankManagerHelperService">The service used to perform operations related to reserve bank managers.</param>
        /// <param name="customerService">The service used to access customer-related data.</param>
        /// <param name="staffService">The service used to access staff-related data.</param>
        /// <param name="managerService">The service used to access manager-related data.</param>
        /// <param name="headManagerService">The service used to access head manager-related data.</param>
        /// <param name="reserveBankManagerService">The service used to access reserve bank manager-related data.</param>
         void LoginAccountHolder(string level, IBankService bankService, IBranchService branchService, IValidateInputs validateInputs,
            ICustomerHelperService? customerHelperService = null, IStaffHelperService? staffHelperService = null, IManagerHelperService? managerHelperService = null,
            IHeadManagerHelperService? headManagerHelperService = null, IReserveBankManagerHelperService? reserveBankManagerHelperService = null, ICustomerService? customerService = null,
            IStaffService? staffService = null, IManagerService? managerService = null, IHeadManagerService? headManagerService = null, IReserveBankManagerService? reserveBankManagerService = null);

        /// <summary>
        /// Retrieves the account balance for a given customer and displays it to the user.
        /// </summary>
        /// <param name="bankId">The ID of the bank that the customer's account belongs to.</param>
        /// <param name="branchId">The ID of the branch where the customer's account is located.</param>
        /// <param name="_customerService">The customer service used to retrieve the account balance.</param>
        /// <param name="_validateInputs">The Service used to validate the inputs.</param>
        /// <param name="level">The access level of the account holder.</param>
        /// <param name="userAccountId">Optional. The ID of the user's account. If null, additional verfications to be done.</param>
        void GetCustomerAccountBalance(string branchId, ICustomerService _customerService, IValidateInputs _validateInputs, string level, string? userAccountId = null);

        /// <summary>
        /// Retrieves the transaction history for a given customer and displays it to the user.
        /// </summary>
        /// <param name="bankId">The ID of the bank that the customer's account belongs to.</param>
        /// <param name="branchId">The ID of the branch where the customer's account is located.</param>
        /// <param name="_customerService">The customer service used to get Customer Detials.</param>
        /// <param name="_validateInputs">The Service used to validate the inputs.</param>
        /// <param name="_transactionService">The transaction service used to retrieve the transaction history.</param>
        /// <param name="level">The access level of the account holder.</param>
        /// <param name="userAccountId">Optional. The ID of the user's account.If null, additional verfications to be done.</param>
        void GetTransactoinHistory(string branchId, ICustomerService _customerService, IValidateInputs _validateInputs, ITransactionService _transactionService, string level, string? userAccountId = null);

        /// <summary>
        /// Retrieves the current exchange rates for a given bank and displays them to the user.
        /// </summary>
        /// <param name="bankId">The ID of the bank to retrieve the exchange rates for.</param>
        /// <param name="_bankService">The bank service used to retrieve the exchange rates.</param>
        void GetExchangeRates(string bankId, IBankService _bankService);

        /// <summary>
        /// Retrieves the transaction charges.
        /// </summary>
        /// <param name="bankId">The ID of the bank from which the transaction is to be made.</param>
        /// <param name="branchId">The ID of the branch from which the transaction is to be made.</param>
        /// <param name="_branchService">The branch service used to retrieve branch information.</param>
        void GetTransactionCharges(string branchId, IBranchService _branchService);

        /// <summary>
        /// Transfers the specified amount from the user's account to another account.
        /// </summary>
        /// <param name="userBankId">The ID of the bank where the user's account is located.</param>
        /// <param name="userBranchId">The ID of the branch where the user's account is located.</param>
        /// <param name="_branchService">The branch service used to retrieve branch information.</param>
        /// <param name="_bankService">The bank service used to retrieve bank information.</param>
        /// <param name="_validateInputs">The service used to validate user inputs.</param>
        /// <param name="_customerService">The customer service used to retrieve customer information.</param>
        /// <param name="level">The access level of the user making the transaction.</param>
        /// <param name="userAccountId">Optional. The ID of the user's account. If null, additional verifications are to be done.</param>
        void TransferAmount(string userBankId, string userBranchId, IBranchService _branchService, IBankService _bankService, IValidateInputs _validateInputs, ICustomerService _customerService, string level, string? userAccountId = null);

        /// <summary>
        /// Opens a new customer account at the specified bank and branch.
        /// </summary>
        /// <param name="bankId">The ID of the bank where the new account is to be opened.</param>
        /// <param name="branchId">The ID of the branch where the new account is to be opened.</param>
        /// <param name="_customerService">The customer service used to manage customer accounts.</param>
        /// <param name="_validateInputs">The input validation service used to validate the user's inputs.</param>
        void OpenCustomerAccount(string branchId, ICustomerService _customerService, IValidateInputs _validateInputs);

        /// <summary>
        /// Updates an existing customer account at the specified bank and branch.
        /// </summary>
        /// <param name="bankId">The ID of the bank where the account is located.</param>
        /// <param name="branchId">The ID of the branch where the account is located.</param>
        /// <param name="_validateInputs">The input validation service used to validate the user's inputs.</param>
        /// <param name="_customerService">The customer service used to manage customer accounts.</param>
        void UpdateCustomerAccount(string branchId, IValidateInputs _validateInputs, ICustomerService _customerService);

        /// <summary>
        /// Deletes an existing customer account at the specified bank and branch.
        /// </summary>
        /// <param name="bankId">The ID of the bank where the account is located.</param>
        /// <param name="branchId">The ID of the branch where the account is located.</param>
        /// <param name="_validateInputs">The input validation service used to validate the user's inputs.</param>
        /// <param name="_customerService">The customer service used to manage customer accounts.</param>
        void DeleteCustomerAccount(string branchId, ICustomerService _customerService, IValidateInputs _validateInputs);

        /// <summary>
        /// Reverts a customer transaction at the specified bank and branch.
        /// </summary>
        /// <param name="bankId">The ID of the bank where the account is located.</param>
        /// <param name="branchId">The ID of the branch where the account is located.</param>
        /// <param name="_validateInputs">The input validation service used to validate the user's inputs.</param>
        /// <param name="_customerService">The customer service used to manage customer accounts.</param>
        /// <param name="_transactionService">The transaction service used to manage customer transactions.</param>
        /// <param name="_bankService">The bank service used to manage banks.</param>
        /// <param name="_branchService">The branch service used to manage branches.</param>
        void RevertCustomerTransaction(string bankId, string branchId, ICustomerService _customerService,
            IValidateInputs _validateInputs, ITransactionService _transactionService, IBankService _bankService,
            IBranchService _branchService);

        /// <summary>
        /// Reverts a customer transaction at the specified bank and branch.
        /// </summary>
        /// <param name="bankId">The ID of the bank where the account is located.</param>
        /// <param name="branchId">The ID of the branch where the account is located.</param>
        /// <param name="_customerService">The customer service used to manage customer accounts.</param>
        /// <param name="_validateInputs">The input validation service used to validate the user's inputs.</param>
        /// /// <param name="_currencyService">The currency service used to validate the currency.</param>
        public void DepositAmountInCustomerAccount(string branchId, ICustomerService _customerService,
            IValidateInputs _validateInputs, ICurrencyService _currencyService);
    }

}