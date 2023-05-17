using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IBranchService _branchService;
        private readonly ITransactionService _transactionService;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrencyService _currencyService;
        private readonly ITransactionChargeService _transactionChargeService;
        public CustomerService(IEncryptionService encryptionService, ICustomerRepository customerRepository,
            IBranchService branchService, ITransactionService transactionService, ICurrencyService currencyService,
            ITransactionChargeService transactionChargeService)
        {
            _encryptionService = encryptionService;
            _branchService = branchService;
            _transactionService = transactionService;
            _customerRepository = customerRepository;
            _currencyService = currencyService;
            _transactionChargeService = transactionChargeService;
        }

        public async Task<IEnumerable<Customer?>> GetAllCustomersAsync(string branchId)
        {
            return await _customerRepository.GetAllCustomers(branchId);
        }

        public async Task<Customer> GetCustomerByIdAsync(string branchId, string customerAccountId)
        {
            Customer? customer = await _customerRepository.GetCustomerById(customerAccountId, branchId);
            return customer!;
        }

        public async Task<Customer> GetCustomerByNameAsync(string branchId, string customerName)
        {
            Customer? customer = await _customerRepository.GetCustomerByName(customerName, branchId);
            return customer!;
        }

        public async Task<Message> IsCustomersExistAsync(string branchId)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                IEnumerable<Customer?> customers = await _customerRepository.GetAllCustomers(branchId);

                if (customers.Any())
                {
                    message.Result = true;
                    message.ResultMessage = $"Customers Exist in The Branch:{branchId}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"No Customers Available In The Branch:{branchId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "branchId Authentication Failed";
            }
            return message;
        }
        public async Task<Message> AuthenticateCustomerAccountAsync(string branchId, string customerAccountId, string customerPassword)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                IEnumerable<Customer?> customers = await _customerRepository.GetAllCustomers(branchId);
                if (customers.Any())
                {
                    byte[] salt = new byte[32];
                    Customer? customer = await _customerRepository.GetCustomerById(customerAccountId, branchId);
                    if (customer is not null)
                    {
                        salt = customer.Salt;
                    }

                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(customerPassword, salt);
                    bool isValidPassword = Convert.ToBase64String(customer!.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck));
                    if (isValidPassword)
                    {
                        message.Result = true;
                        message.ResultMessage = "Customer Validation Successful.";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Customer Validation Failed.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"No Customers Available In The Branch Id:{branchId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> IsAccountExistAsync(string branchId, string customerAccountId)
        {
            Message message;
            message = await IsCustomersExistAsync(branchId);
            if (message.Result)
            {
                bool isCustomerExist = await _customerRepository.IsCustomerExist(customerAccountId, branchId);
                if (isCustomerExist)
                {
                    message.Result = true;
                    message.ResultMessage = "Customer Exist.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Customer Doesn't Exist";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"No Customers Available In The Branch:{branchId}";
            }
            return message;
        }

        public async Task<Message> OpenCustomerAccountAsync(string branchId, string customerName, string customerPassword,
          string customerPhoneNumber, string customerEmailId, AccountType customerAccountType, string customerAddress,
          string customerDateOfBirth, Gender customerGender)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                Customer customer = await _customerRepository.GetCustomerByName(customerName, branchId);
                if (customer is null)
                {
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string UserFirstThreeCharecters = customerName.Substring(0, 3);
                    string customerAccountId = string.Concat(UserFirstThreeCharecters, date);

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(customerPassword, salt);
                    decimal OpeningBalance = 500; //INR
                    Customer customerObject = new()
                    {
                        Name = customerName,
                        PhoneNumber = customerPhoneNumber,
                        EmailId = customerEmailId,
                        AccountType = customerAccountType,
                        Address = customerAddress,
                        DateOfBirth = customerDateOfBirth,
                        Gender = customerGender,
                        Salt = salt,
                        PassbookIssueDate = date,
                        Balance = OpeningBalance,
                        HashedPassword = hashedPassword,
                        AccountId = customerAccountId,
                        IsActive = true,
                        Role = Roles.Customer
                    };

                    bool isCustomerAdded = await _customerRepository.AddCustomerAccount(customerObject, branchId);
                    if (isCustomerAdded)
                    {
                        message.Result = true;
                        message.ResultMessage = $"Account Created for {customerName} with Account Id:{customerAccountId}";
                        message.Data = customerAccountId;
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"Failed to Create Customer Account for {customerName}";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Customer Name:{customerName} Already Exist.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> AuthenticateToCustomerAccountAsync(string branchId, string customerAccountId)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                IEnumerable<Customer?> customers = await _customerRepository.GetAllCustomers(branchId);
                if (customers.Any())
                {
                    bool isToCustomerExist = await _customerRepository.IsCustomerExist(customerAccountId, branchId);
                    if (isToCustomerExist)
                    {
                        message.Result = true;
                        message.ResultMessage = "Customer Validation Successful.";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Customer Validation Failed.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"No Customers Available In The Branch Id:{branchId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }
        public async Task<Message> UpdateCustomerAccountAsync(string branchId, string customerAccountId, string customerName, string customerPassword,
          string customerPhoneNumber, string customerEmailId, AccountType customerAccountType, string customerAddress, string customerDateOfBirth, Gender customerGender)
        {
            Message message;
            message = await IsAccountExistAsync(branchId, customerAccountId);
            if (message.Result)
            {
                Customer? customer = await _customerRepository.GetCustomerById(customerAccountId, branchId);
                byte[] salt = null;
                byte[] hashedPassword = null;
                bool canContinue = true;
                if (customerPassword is not null && customer is not null)
                {
                    salt = customer!.Salt;
                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(customerPassword, salt);
                    if (Convert.ToBase64String(customer.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)))
                    {
                        message.Result = false;
                        message.ResultMessage = "New password Matches with the Old Password.,Provide a New Password";
                        canContinue = false;
                    }

                    salt = _encryptionService.GenerateSalt();
                    hashedPassword = _encryptionService.HashPassword(customerPassword, salt);
                }

                if (canContinue && customer is not null)
                {
                    Customer customerObject = new()
                    {
                        AccountId = customerAccountId,
                        Name = customerName,
                        PhoneNumber = customerPhoneNumber,
                        EmailId = customerEmailId,
                        AccountType = customerAccountType,
                        HashedPassword = hashedPassword,
                        Address = customerAddress,
                        DateOfBirth = customerDateOfBirth,
                        Gender = customerGender,
                        Salt = salt,
                        IsActive = true
                    };
                    bool isDetailsUpdated = await _customerRepository.UpdateCustomerAccount(customerObject, branchId);
                    if (isDetailsUpdated)
                    {
                        message.Result = true;
                        message.ResultMessage = "Updated Details Sucessfully";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Failed to Update Details";
                    }
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Validation Failed.";
            }

            return message;
        }

        public async Task<Message> DeleteCustomerAccountAsync(string branchId, string customerAccountId)
        {
            Message message;
            message = await IsAccountExistAsync(branchId, customerAccountId);
            if (message.Result)
            {
                bool isDeleted = await _customerRepository.DeleteCustomerAccount(customerAccountId, branchId);
                if (isDeleted)
                {
                    message.Result = true;
                    message.ResultMessage = $"Deleted AccountId:{customerAccountId} Successfully.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Delete Customer Account Id:{customerAccountId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Customer with Account Id:{customerAccountId} doesn't Exist.";
            }
            return message;
        }

        public async Task<Message> DepositAmountAsync(string bankId, string branchId, string customerAccountId, decimal depositAmount, string currencyCode)
        {
            Message message;
            message = await IsAccountExistAsync(branchId, customerAccountId);
            if (message.Result)
            {
                if (depositAmount > 0)
                {
                    Currency currency = new();
                    decimal exchangedAmount = 0;
                    if (currency.DefaultCurrencyCode.Equals(currencyCode))
                    {
                        exchangedAmount = depositAmount * currency.ExchangeRate;
                    }
                    else
                    {
                        message = await _currencyService.ValidateCurrencyAsync(bankId, currencyCode);

                        if (message.Result)
                        {
                            Currency? currencyObject = await _currencyService.GetCurrencyByCode(currencyCode, bankId);
                            exchangedAmount = depositAmount * currencyObject!.ExchangeRate;
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Entered Currency is not Acceptable by Bank.Please Kindly Contact Branch Manager";
                        }
                    }

                    if (exchangedAmount > 0)
                    {
                        Customer? customer = await _customerRepository.GetCustomerById(customerAccountId, branchId);
                        Customer customerObject = new()
                        {
                            Balance = customer!.Balance + exchangedAmount,
                            AccountId = customerAccountId
                        };
                        bool isUpdated = await _customerRepository.UpdateCustomerAccount(customerObject, branchId);
                        if (isUpdated)
                        {
                            message = await _transactionService.TransactionHistoryAsync(bankId, branchId, customerAccountId, 0, exchangedAmount, customerObject.Balance, TransactionType.Deposit);
                            if (message.Result)
                            {
                                message.Result = true;
                                message.ResultMessage = $"Amount:'{exchangedAmount}'INR Added Succesfully., Avl.Bal:{customerObject.Balance}INR";
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = $"Failed to Deposit Amount";
                            }
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Amount less than 0";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Amount less than 0";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Not Existed";
            }
            return message;
        }

        public async Task<Message> CheckAccountBalanceAsync(string branchId, string customerAccountId)
        {
            Message message;
            message = await IsAccountExistAsync(branchId, customerAccountId);
            if (message.Result)
            {
                Customer? customer = await _customerRepository.GetCustomerById(customerAccountId, branchId);
                message.ResultMessage = $"Available Balance :{customer!.Balance} INR";
                message.Data = $"{customer.Balance}";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Doest Exist";
            }
            return message;
        }

        public async Task<Message> CheckToCustomerAccountBalanceAsync(string branchId, string customerAccountId)
        {
            Message message;
            message = await IsAccountExistAsync(branchId, customerAccountId);
            if (message.Result)
            {
                Customer? customer = await _customerRepository.GetCustomerById(customerAccountId, branchId);
                message.ResultMessage = $"Available Balance :{customer!.Balance}";
                message.Data = $"{customer.Balance}";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Doest Exist";
            }
            return message;
        }

        public async Task<Message> WithdrawAmountAsync(string bankId, string branchId, string customerAccountId, decimal withDrawAmount)
        {
            Message message;
            message = await IsAccountExistAsync(branchId, customerAccountId);
            if (message.Result)
            {
                Customer? customer = await _customerRepository.GetCustomerById(customerAccountId, branchId);
                if (customer!.Balance == 0)
                {
                    message.Result = false;
                    message.ResultMessage = "Failed ! Account Balance: 0 Rupees";
                }
                else if (customer.Balance < withDrawAmount)
                {
                    message.Result = false;
                    message.ResultMessage = $"Insufficient funds !! Aval.Bal is {customer.Balance} Rupees";
                }
                else
                {
                    Customer customerObject = new()
                    {
                        Balance = customer.Balance - withDrawAmount,
                        AccountId = customerAccountId
                    };
                    bool isUpdated = await _customerRepository.UpdateCustomerAccount(customerObject, branchId);
                    if (isUpdated)
                    {
                        message = await _transactionService.TransactionHistoryAsync(bankId, branchId, customerAccountId, withDrawAmount, 0, customerObject.Balance, TransactionType.Withdraw);
                        if (message.Result)
                        {
                            message.Result = true;
                            message.ResultMessage = $"Withdrawn Amount:{withDrawAmount}INR Successful!! Aval.Bal is {customerObject.Balance}INR";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"Failed to Withdraw Amount:{withDrawAmount}";
                        }
                    }
                }
            }
            return message;
        }

        public async Task<Message> TransferAmountAsync(string bankId, string branchId, string customerAccountId, string toBankId,
            string toBranchId, string toCustomerAccountId, decimal transferAmount, TransferMethod transferMethod)
        {
            Message message = new();
            Message fromCustomer = await IsAccountExistAsync(branchId, customerAccountId);
            Message toCustomer = await IsAccountExistAsync(toBranchId, toCustomerAccountId);
            ushort bankInterestRate = 0;
            if (fromCustomer.Result && toCustomer.Result)
            {
                TransactionCharges transactionCharges = await _transactionChargeService.GetTransactionCharges(branchId);
                if (transactionCharges is not null)
                {
                    if (bankId.Substring(0, 3).Equals(toBankId.Substring(0, 3)))
                    {
                        if (transferMethod.Equals(TransferMethod.RtgsSameBank))
                        {
                            bankInterestRate = transactionCharges.RtgsSameBank;
                        }
                        else if (transferMethod.Equals(TransferMethod.ImpsSameBank))
                        {
                            bankInterestRate = transactionCharges.ImpsSameBank;
                        }
                    }
                    else
                    {
                        if (transferMethod.Equals(TransferMethod.RtgsOtherBank))
                        {
                            bankInterestRate = transactionCharges.RtgsOtherBank;
                        }
                        else if (transferMethod.Equals(TransferMethod.ImpsOtherBank))
                        {
                            bankInterestRate = transactionCharges.ImpsOtherBank;
                        }
                    }
                    decimal transferAmountInterest = transferAmount * (bankInterestRate / 100.0m);
                    decimal transferAmountWithInterest = transferAmount + transferAmountInterest;

                    message = await CheckAccountBalanceAsync(branchId, customerAccountId);
                    decimal fromCustomerBalanace = decimal.Parse(message.Data);
                    if (fromCustomerBalanace >= transferAmountInterest + transferAmount)
                    {
                        Customer? fromCustomerData = await _customerRepository.GetCustomerById(customerAccountId, branchId);
                        Customer? toCustomerData = await _customerRepository.GetCustomerById(toCustomerAccountId, toBranchId);
                        Customer fromCustomerObject = new()
                        {
                            Balance = fromCustomerData!.Balance - transferAmountWithInterest,
                            AccountId = customerAccountId
                        };

                        Customer toCustomerObject = new()
                        {
                            Balance = toCustomerData!.Balance + transferAmount,
                            AccountId = toCustomerAccountId
                        };

                        bool isfromCustomerUpdated = await _customerRepository.UpdateCustomerAccount(fromCustomerObject, branchId);
                        fromCustomerBalanace = fromCustomerObject.Balance;

                        bool isToCustomerUpdated = await _customerRepository.UpdateCustomerAccount(toCustomerObject, toBranchId);
                        decimal toCustomerBalance = toCustomerObject.Balance;

                        message = await _transactionService.TransactionHistoryFromAndToAsync(bankId, branchId, customerAccountId, toBankId, toBranchId, toCustomerAccountId, transferAmount, 0, fromCustomerBalanace, toCustomerBalance, TransactionType.Transfer);
                        if (message.Result && isfromCustomerUpdated && isToCustomerUpdated)
                        {
                            message.Result = true;
                            message.ResultMessage = $"Transfer of {transferAmount} Rupees Sucessfull.,Deducted Amout :{transferAmount + transferAmountInterest}, Avl.Bal: {fromCustomerBalanace}";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Failed to Transfer the Amount.";
                        }
                    }
                    else
                    {
                        decimal requiredAmount = Math.Abs(fromCustomerBalanace - transferAmountInterest + transferAmount);
                        message.Result = false;
                        message.ResultMessage = $"Insufficient Balance.,Avl.Bal:{fromCustomerBalanace},Add {requiredAmount} or Reduce the Transfer Amount Next Time.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Charges Not Available";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Account Doesn't Exist";
            }
            return message;
        }
    }
}
