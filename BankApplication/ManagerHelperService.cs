using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;

namespace BankApplication
{
    public class ManagerHelperService : IManagerHelperService
    {
        readonly IBankService _bankService;
        readonly IBranchService _branchService;
        readonly IStaffService _staffService;
        readonly ICustomerService _customerService;
        readonly ICommonHelperService _commonHelperService;
        readonly ITransactionChargeService _transactionChargeService;
        readonly IValidateInputs _validateInputs;
        readonly ITransactionService _transactionService;
        readonly ICurrencyService _currencyService;
        public ManagerHelperService(IBankService bankService, IBranchService branchService,
        IStaffService staffService, ICustomerService customerService, ICommonHelperService commonHelperService,
        ITransactionChargeService transactionChargeService, IValidateInputs validateInputs,
        ITransactionService transactionService, ICurrencyService currencyService)
        {
            _bankService = bankService;
            _branchService = branchService;
            _staffService = staffService;
            _customerService = customerService;
            _commonHelperService = commonHelperService;
            _transactionChargeService = transactionChargeService;
            _validateInputs = validateInputs;
            _transactionService = transactionService;
            _currencyService = currencyService;
        }
        Message message = new();
        public void SelectedOption(ushort Option, string managerBankId, string managerBranchId)
        {
            switch (Option)
            {
                case 1: //Open Staff Account
                    while (true)
                    {
                        string staffName = _commonHelperService.GetName(Miscellaneous.staff, _validateInputs);
                        string staffPassword = _commonHelperService.GetPassword(Miscellaneous.staff, _validateInputs);
                        Console.WriteLine("Choose Staff Roles from Below:");
                        foreach (Roles option in Enum.GetValues(typeof(Roles)))
                        {
                            Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                        }

                        Roles staffRole = 0;
                        while (true)
                        {
                            Console.WriteLine("Enter Staff Role:");

                            bool isValid = Roles.TryParse(Console.ReadLine(), out staffRole);
                            if (!isValid && staffRole == 0)
                            {
                                Console.WriteLine("Please Enter as per the Above Staff Roles");
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }

                        message = _staffService.OpenStaffAccountAsync(managerBranchId, staffName, staffPassword).Result;
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
                    };
                    break;

                case 2: //Add Transaction Charges
                    while (true)
                    {
                        IEnumerable<TransactionCharges> transactionCharges = _branchService.GetTransactionChargesAsync(managerBranchId).Result;
                        if (transactionCharges is not null)
                        {
                            Console.WriteLine("Charges Already Available");
                            Console.WriteLine();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Enter RtgsSameBank Charge in %");
                            ushort rtgsSameBank;
                            while (true)
                            {
                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out rtgsSameBank);

                                if (!isValidValue)
                                {
                                    Console.WriteLine($"Entered Value should not be Empty and Contain Only Numbers");
                                }
                                else if (isValidValue && rtgsSameBank >= 100)
                                {
                                    Console.WriteLine($"Entered {rtgsSameBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            ushort rtgsOtherBank;
                            while (true)
                            {
                                Console.WriteLine("Enter RtgsOtherBank Charge in %");

                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out rtgsOtherBank);
                                if (!isValidValue)
                                {
                                    Console.WriteLine($"Entered Value should not be Empty and Contain Only Numbers");
                                }
                                else if (isValidValue && rtgsOtherBank >= 100)
                                {
                                    Console.WriteLine($"Entered {rtgsOtherBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            ushort impsSameBank;
                            while (true)
                            {
                                Console.WriteLine("Enter ImpsSameBank Charge in %");

                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out impsSameBank);
                                if (!isValidValue)
                                {
                                    Console.WriteLine($"Entered Value should not be Empty and Contain Only Numbers");
                                }
                                else if (isValidValue && impsSameBank >= 100)
                                {
                                    Console.WriteLine($"Entered {impsSameBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            ushort impsOtherBank;
                            while (true)
                            {
                                Console.WriteLine("Enter ImpsOtherBank Charge in %");

                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out impsOtherBank);
                                if (!isValidValue)
                                {
                                    Console.WriteLine($"Entered Value should not be Empty and Contain Only Numbers");
                                }
                                else if (isValidValue && impsOtherBank >= 100)
                                {
                                    Console.WriteLine($"Entered {impsOtherBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            message = _transactionChargeService.AddTransactionChargesAsync(managerBranchId, rtgsSameBank, rtgsOtherBank, impsSameBank, impsOtherBank).Result;
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                                Console.WriteLine();
                                break;
                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                                continue;
                            }
                        }
                    }
                    break;

                case 3: //OpenCustomerAccount
                    _commonHelperService.OpenCustomerAccount(managerBranchId, _customerService, _validateInputs);
                    break;

                case 4: //UpdateCustomerAccount
                    _commonHelperService.UpdateCustomerAccount(managerBranchId, _validateInputs, _customerService);
                    break;

                case 5://DeleteCustomerAccount
                    _commonHelperService.DeleteCustomerAccount(managerBranchId,_customerService,_validateInputs);
                    break;

                case 6://Displaying Customer Transaction History
                    _commonHelperService.GetTransactoinHistory(managerBranchId, _customerService, _validateInputs,
                       _transactionService, Miscellaneous.staff, null);
                    break;

                case 7://Revert Customer Transaction
                    _commonHelperService.RevertCustomerTransaction(managerBankId, managerBranchId, _customerService, _validateInputs,
                        _transactionService, _bankService, _branchService);
                    break;

                case 8://Check Customer Account Balance
                    _commonHelperService.GetCustomerAccountBalance(managerBranchId, _customerService, _validateInputs,
                    Miscellaneous.staff, null);
                    break;

                case 9:// Get ExchangeRates
                    _commonHelperService.GetExchangeRates(managerBankId, _bankService);
                    break;

                case 10:// Get TransactionCharges
                    _commonHelperService.GetTransactionCharges( managerBranchId, _branchService);
                    break;

                case 11://Deposit Amount in Customer Account
                    _commonHelperService.DepositAmountInCustomerAccount(managerBranchId, _customerService,_validateInputs,
                        _currencyService);
                    break;

                case 12:// Transfer Amount 
                    _commonHelperService.TransferAmount(managerBankId, managerBranchId, _branchService, _bankService, _validateInputs,
                       _customerService, Miscellaneous.staff, null);
                    break;

                case 13: //UpdateTransactionCharges
                    while (true)
                    {
                        IEnumerable<TransactionCharges> transactionCharges = _branchService.GetTransactionChargesAsync(managerBranchId).Result;
                        if (transactionCharges is not null)
                        {
                            Console.WriteLine("Enter RtgsSameBank Charge in %");
                            ushort rtgsSameBank;

                            while (true)
                            {
                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out rtgsSameBank);

                                if (isValidValue && rtgsSameBank >= 100)
                                {
                                    Console.WriteLine($"Entered {rtgsSameBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else if (!isValidValue)
                                {
                                    rtgsSameBank = 101;
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            ushort rtgsOtherBank;
                            while (true)
                            {
                                Console.WriteLine("Enter RtgsOtherBank Charge in %");

                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out rtgsOtherBank);
                                if (!isValidValue && rtgsOtherBank >= 100)
                                {
                                    Console.WriteLine($"Entered {rtgsOtherBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else if (!isValidValue)
                                {
                                    rtgsOtherBank = 101;
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            ushort impsSameBank;
                            while (true)
                            {
                                Console.WriteLine("Enter ImpsSameBank Charge in %");

                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out impsSameBank);

                                if (!isValidValue && impsSameBank >= 100)
                                {
                                    Console.WriteLine($"Entered {impsSameBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else if (!isValidValue)
                                {
                                    impsSameBank = 101;
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            ushort impsOtherBank;
                            while (true)
                            {
                                Console.WriteLine("Enter ImpsOtherBank Charge in %");

                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out impsOtherBank);

                                if (!isValidValue && impsOtherBank >= 100)
                                {
                                    Console.WriteLine($"Entered {impsOtherBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else if (!isValidValue)
                                {
                                    impsOtherBank = 101;
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            message = _transactionChargeService.UpdateTransactionChargesAsync(managerBranchId, rtgsSameBank, rtgsOtherBank, impsSameBank, impsOtherBank).Result;
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
                    break;
                case 14: //DeleteTransactionCharges
                    while (true)
                    {
                        IEnumerable<TransactionCharges> transactionCharges = _branchService.GetTransactionChargesAsync(managerBranchId).Result;
                        if (transactionCharges is not null)
                        {
                            message = _transactionChargeService.DeleteTransactionChargesAsync(managerBranchId).Result;
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                                break;
                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                    }
                    break;

                case 15: //UpdateStaffAccount
                    while (true)
                    {
                        message = _staffService.IsStaffExistAsync(managerBranchId).Result;
                        if (message.Result)
                        {
                            string staffAccountId = _commonHelperService.GetAccountId(Miscellaneous.staff, _validateInputs);
                            message = _staffService.IsAccountExistAsync(managerBranchId, staffAccountId).Result;
                            if (message.Result)
                            {
                                Staff staff = _staffService.GetStaffByIdAsync(managerBranchId, staffAccountId).Result;
                                Console.WriteLine("Staff Details:");
                                Console.WriteLine(staff.ToString());

                                string staffName;
                                while (true)
                                {
                                    Console.WriteLine("Update Staff Name");
                                    staffName = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(staffName))
                                    {
                                        message = _validateInputs.ValidateNameFormat(staffName);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                string staffPassword;
                                while (true)
                                {
                                    Console.WriteLine("Update Staff Password");
                                    staffPassword = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(staffPassword))
                                    {
                                        message = _validateInputs.ValidatePasswordFormat(staffPassword);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                ushort staffRole;
                                while (true)
                                {
                                    Console.WriteLine("Choose From Below Menu Options To Update");
                                    foreach (Roles option in Enum.GetValues(typeof(Roles)))
                                    {
                                        Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                    }
                                    bool isValid = ushort.TryParse(Console.ReadLine(), out staffRole);
                                    if (!isValid || staffRole != 0 && staffRole > 5)
                                    {
                                        Console.WriteLine("please Enter a Valid Option");
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                message = _staffService.UpdateStaffAccountAsync(managerBranchId, staffAccountId, staffName, staffPassword, (Roles)staffRole).Result;

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
                            break;
                        }
                    }
                    break;
                case 16: //DeleteStaffAccount
                    while (true)
                    {
                        message = _staffService.IsStaffExistAsync(managerBranchId).Result;
                        if (message.Result)
                        {
                            string staffAccountId = _commonHelperService.GetAccountId(Miscellaneous.staff, _validateInputs);

                            message = _staffService.DeleteStaffAccountAsync(managerBranchId, staffAccountId).Result;
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
                    };
                    break;
            }
        }

    }
}

