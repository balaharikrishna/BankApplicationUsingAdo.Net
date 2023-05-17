using BankApplicationModels;
using BankApplicationModels.Enums;

namespace BankApplicationServices.IServices
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer?>> GetAllCustomersAsync(string branchId);

        /// <summary>
        /// Retrieves the Customer Passbook.
        /// </summary>
        /// <param name="branchId">The unique identifier of the branch where the customer account is located.</param>
        /// <param name="customerAccountId">The unique identifier of the customer account whose passbook will be retrieved.</param>
        /// <returns>A message about status of Retreving the passbook.</returns>
        Task<Customer> GetCustomerByIdAsync(string branchId, string customerAccountId);

        Task<Customer> GetCustomerByNameAsync(string branchId, string customerName);
        
        /// <summary>
        /// Checks if a customer account exists in a given bank and branch.
        /// </summary>
        /// <param name="branchId">The BranchId of the branch in which the customer account is held.</param>
        /// <returns>A message about status of the customers Existance in Branch of a given Bank.</returns>
        Task<Message> IsCustomersExistAsync(string branchId);

        /// <summary>
        /// Authenticates a customer Account by verifying the customer's password.
        /// </summary>
        /// <param name="branchId">The BranchId of the branch in which the customer account is held.</param>
        /// <param name="customerAccountId">The CustomerAccountId of the customer account to authenticate.</param>
        /// <param name="customerPassword">The password associated with the customer account.</param>
        /// <returns>A message about status of the customer account Authentication.</returns>
        Task<Message> AuthenticateCustomerAccountAsync(string branchId, string customerAccountId, string customerPassword);

        /// <summary>
        /// Checks if a customer account exists in a given bank and branch.
        /// </summary>
        /// <param name="bankId">The BankId of the bank in which the customer account is held.</param>
        /// <param name="branchId">The BranchId of the branch in which the customer account is held.</param>
        /// <param name="customerAccountId">The CustomerAccountId of the customer account to check.</param>
        /// <returns>A message about status of the customer account Existence.</returns>
        Task<Message> IsAccountExistAsync(string branchId, string customerAccountId);

        /// <summary>
        /// Authenticates the Other customer Account.
        /// </summary>
        /// <param name="bankId">The BankId of the bank in which the customer account is held.</param>
        /// <param name="branchId">The BranchId of the branch in which the customer account is held.</param>
        /// <param name="customerAccountId">The CustomerAccountId of the customer account to authenticate to.</param>
        /// <returns>A message about status of the Other customer Authentication in a given Bank.</returns>
        Task<Message> AuthenticateToCustomerAccountAsync(string branchId, string customerAccountId);

        /// <summary>
        /// Checks the Balance of a customer Account.
        /// </summary>
        /// <param name="bankId">The BankId of the bank in which the customer account is held.</param>
        /// <param name="branchId">The BranchId of the branch in which the customer account is held.</param>
        /// <param name="customerAccountId">The CustomerAccountId of the customer account to check the balance of.</param>
        /// <returns>A message about status of the customer's Balance.</returns>
        Task<Message> CheckAccountBalanceAsync(string branchId, string customerAccountId);

        /// <summary>
        /// Checks the balance Other customer Account.
        /// </summary>
        /// <param name="bankId">The BankId of the bank in which the customer account is held.</param>
        /// <param name="branchId">The BranchId of the branch in which the customer account is held.</param>
        /// <param name="customerAccountId">The CustomerAccountId of the customer account to check the balance of.</param>
        /// <returns>A message about status of the Other customer Balance in a given Bank branch.</returns>
        Task<Message> CheckToCustomerAccountBalanceAsync(string branchId, string customerAccountId);

        /// <summary>
        /// Deletes an existing customer account from a specific bank branch.
        /// </summary>
        /// <param name="bankId">The unique identifier of the bank where the customer account is located.</param>
        /// <param name="branchId">The unique identifier of the branch where the customer account is located.</param>
        /// <param name="customerAccountId">The unique identifier of the customer account to be deleted.</param>
        /// <returns>A message about status of the customer Account Deletion.</returns>
        Task<Message> DeleteCustomerAccountAsync(string branchId, string customerAccountId);

        /// <summary>
        /// Deposits Amount into a customer Account.
        /// </summary>
        /// <param name="bankId">The unique identifier of the bank where the customer account is located.</param>
        /// <param name="branchId">The unique identifier of the branch where the customer account is located.</param>
        /// <param name="customerAccountId">The unique identifier of the customer account where the deposit will be made.</param>
        /// <param name="depositAmount">The amount of money to be deposited into the customer account.</param>
        /// <param name="currencyCode">The currency code of the deposit amount.</param>
        /// <returns>A message about status of the Deposition.</returns>
        Task<Message> DepositAmountAsync(string bankId, string branchId, string customerAccountId, decimal depositAmount, string currencyCode);

        /// <summary>
        /// Opens a new customer Account in a Bank Branch.
        /// </summary>
        /// <param name="bankId">The unique identifier of the bank where the customer account will be created.</param>
        /// <param name="branchId">The unique identifier of the branch where the customer account will be created.</param>
        /// <param name="customerName">The name of the customer who will own the new customer account.</param>
        /// <param name="customerPassword">The password of the customer who will own the new customer account.</param>
        /// <param name="customerPhoneNumber">The phone number of the customer who will own the new customer account.</param>
        /// <param name="customerEmailId">The email address of the customer who will own the new customer account.</param>
        /// <param name="customerAccountType">The type of the new customer account.</param>
        /// <param name="customerAddress">The address of the customer who will own the new customer account.</param>
        /// <param name="customerDateOfBirth">The date of birth of the customer who will own the new customer account.</param>
        /// <param name="customerGender">The gender of the customer who will own the new customer account.</param>
        /// <returns>A message about status of the new customer account Creation.</returns>
        Task<Message> OpenCustomerAccountAsync(string branchId, string customerName, string customerPassword, string customerPhoneNumber,
            string customerEmailId, AccountType customerAccountType, string customerAddress, string customerDateOfBirth, Gender customerGender);

        /// <summary>
        /// Transfers a specified amount from one customer account to another.
        /// </summary>
        /// <param name="bankId">The bank identifier where the customer account is located.</param>
        /// <param name="branchId">The branch identifier where the customer account is located.</param>
        /// <param name="customerAccountId">The identifier of the customer account from which the transfer is made.</param>
        /// <param name="toBankId">The bank identifier where the recipient customer account is located.</param>
        /// <param name="toBranchId">The branch identifier where the recipient customer account is located.</param>
        /// <param name="toCustomerAccountId">The identifier of the recipient customer account to which the transfer is made.</param>
        /// <param name="transferAmount">The amount to be transferred.</param>
        /// <param name="transferMethod">The method used to transfer the amount.</param>
        /// <returns>A message indicating the status of the transfer.</returns>
        Task<Message> TransferAmountAsync(string bankId, string branchId, string customerAccountId, string toBankId,
            string toBranchId, string toCustomerAccountId, decimal transferAmount, TransferMethod transferMethod);

        /// <summary>
        /// Updates an existing customer account with new Information.
        /// </summary>
        /// <param name="bankId">The bank identifier where the customer account is located.</param>
        /// <param name="branchId">The branch identifier where the customer account is located.</param>
        /// <param name="customerAccountId">The identifier of the customer account to be updated.</param>
        /// <param name="customerName">The new name of the customer.</param>
        /// <param name="customerPassword">The new password for the customer account.</param>
        /// <param name="customerPhoneNumber">The new phone number for the customer.</param>
        /// <param name="customerEmailId">The new email address for the customer.</param>
        /// <param name="customerAccountType">The new account type for the customer.</param>
        /// <param name="customerAddress">The new address of the customer.</param>
        /// <param name="customerDateOfBirth">The new date of birth of the customer.</param>
        /// <param name="customerGender">The new gender of the customer.</param>
        /// <returns>A message indicating the status of the Account Updation.</returns>
        Task<Message> UpdateCustomerAccountAsync(string branchId, string customerAccountId, string customerName, string customerPassword,
            string customerPhoneNumber, string customerEmailId, AccountType customerAccountType, string customerAddress, string customerDateOfBirth, Gender customerGender);

        /// <summary>
        /// Withdraws a specified amount from a customer Account.
        /// </summary>
        /// <param name="bankId">The bank identifier where the customer account is located.</param>
        /// <param name="branchId">The branch identifier where the customer account is located.</param>
        /// <param name="customerAccountId">The identifier of the customer account from which the withdrawal is made.</param>
        /// <param name="withDrawAmount">The amount to be withdrawn.</param>
        /// <returns>A message indicating the status of the withdrawal.</returns>
        Task<Message> WithdrawAmountAsync(string bankId, string branchId, string customerAccountId, decimal withDrawAmount);
    }
}