using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplication
{
    internal class HeadManagerHelperService : IHeadManagerHelperService
    {
        readonly IBranchService _branchService;
        readonly ICommonHelperService _commonHelperService;
        readonly IManagerService _managerService;
        readonly ICurrencyService _currencyService;
        readonly IValidateInputs _validateInputs;
        readonly IBankService _bankService;
        public HeadManagerHelperService(IBranchService branchService, ICommonHelperService commonHelperService,
            IManagerService managerService, ICurrencyService currencyService, IValidateInputs validateInputs, IBankService bankService)
        {
            _branchService = branchService;
            _commonHelperService = commonHelperService;
            _managerService = managerService;
            _currencyService = currencyService;
            _validateInputs = validateInputs;
            _bankService = bankService;
        }
        public void SelectedOption(ushort Option, string headManagerBankId)
        {
            switch (Option)
            {
                case 1: //CreateBranch
                    while (true)
                    {
                        string branchName = _commonHelperService.GetName(Miscellaneous.branch, _validateInputs);
                        string branchPhoneNumber = _commonHelperService.GetPhoneNumber(Miscellaneous.branch, _validateInputs);
                        string branchAddress = _commonHelperService.GetAddress(Miscellaneous.branch, _validateInputs);

                        Message message = _branchService.CreateBranchAsync(headManagerBankId, branchName, branchPhoneNumber, branchAddress).Result;
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
                    break;

                case 2: //OpenManagerAcccount
                    while (true)
                    {
                        Message message = _branchService.IsBranchesExistAsync(headManagerBankId).Result;
                        if (message.Result)
                        {
                            string branchId = _commonHelperService.GetBranchId(Miscellaneous.branchManager, _branchService, _validateInputs);
                            string branchManagerName = _commonHelperService.GetName(Miscellaneous.branchManager, _validateInputs);
                            string branchManagerPassword = _commonHelperService.GetPassword(Miscellaneous.branchManager, _validateInputs);

                            message = _managerService.OpenManagerAccountAsync(branchId, branchManagerName, branchManagerPassword).Result;
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

                case 3: //UpdateManagerAccount 
                    while (true)
                    {
                        Message message;
                        message = _branchService.IsBranchesExistAsync(headManagerBankId).Result;
                        if (message.Result)
                        {
                            string branchId = _commonHelperService.GetBranchId(Miscellaneous.branchManager, _branchService, _validateInputs);
                            message = _managerService.IsManagersExistAsync(branchId).Result;
                            if (message.Result)
                            {
                                string managerAccountId = _commonHelperService.GetAccountId(Miscellaneous.branchManager, _validateInputs);

                                message = _managerService.IsAccountExistAsync(branchId, managerAccountId).Result;
                                if (message.Result)
                                {
                                    Manager manager = _managerService.GetManagerByIdAsync(branchId, managerAccountId).Result;
                                    Console.WriteLine("Manager Details:");
                                    Console.WriteLine(manager.ToString());

                                    string managerName;
                                    while (true)
                                    {
                                        Console.WriteLine("Update Manager Name");
                                        managerName = Console.ReadLine() ?? string.Empty;
                                        if (!string.IsNullOrEmpty(managerName))
                                        {
                                            message = _validateInputs.ValidateNameFormat(managerName);
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

                                    string managerPassword;
                                    while (true)
                                    {
                                        Console.WriteLine("Update Staff Password");
                                        managerPassword = Console.ReadLine() ?? string.Empty;
                                        if (!string.IsNullOrEmpty(managerPassword))
                                        {
                                            message = _validateInputs.ValidatePasswordFormat(managerPassword);
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

                                    message = _managerService.UpdateManagerAccountAsync(branchId, managerAccountId, managerName, managerPassword).Result;

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
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                    }
                    break;

                case 4://DeleteManagerAccount
                    while (true)
                    {
                        Message message;
                        message = _branchService.IsBranchesExistAsync(headManagerBankId).Result;
                        if (message.Result)
                        {
                            string branchId = _commonHelperService.GetBranchId(Miscellaneous.branchManager, _branchService, _validateInputs);
                            message = _managerService.IsManagersExistAsync(branchId).Result;
                            if (message.Result)
                            {
                                string managerAccountId = _commonHelperService.GetAccountId(Miscellaneous.branchManager, _validateInputs);

                                message = _managerService.DeleteManagerAccountAsync(branchId, managerAccountId).Result;
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
                    break;

                case 5: //AddCurrency with exchange Rates
                    while (true)
                    {
                        Message message;
                        Console.WriteLine("Please Enter currency Code");
                        string currencyCode = Console.ReadLine()?.ToUpper() ?? string.Empty;
                        message = _validateInputs.ValidateCurrencyCodeFormat(currencyCode);
                        if (message.Result)
                        {
                            message = _currencyService.ValidateCurrencyAsync(headManagerBankId, currencyCode).Result;
                            if (!message.Result)
                            {
                                while (true)
                                {
                                    Console.WriteLine("Please Enter Exchange Rate as Per INR");
                                    bool isValid = decimal.TryParse(Console.ReadLine(), out decimal exchangeRate);

                                    if (isValid && exchangeRate == 0)
                                    {
                                        Console.WriteLine($"Provided '{exchangeRate}' Should Not be zero or Empty");
                                        continue;
                                    }
                                    else
                                    {
                                        message = _currencyService.AddCurrencyAsync(headManagerBankId, currencyCode, exchangeRate).Result;
                                        if (message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            break;
                                        }
                                    }
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


                case 6: //UpdateCurrency with exchange Rates
                    while (true)
                    {
                        Message message;
                        IEnumerable<Currency> currencies = _bankService.GetExchangeRatesAsync(headManagerBankId).Result;
                        if (currencies is not null)
                        {
                            while (true)
                            {
                                Console.WriteLine("Please Enter currency Code");
                                string currencyCode = Console.ReadLine() ?? string.Empty;
                                message = _validateInputs.ValidateCurrencyCodeFormat(currencyCode);
                                if (message.Result)
                                {
                                    message = _currencyService.ValidateCurrencyAsync(headManagerBankId, currencyCode).Result;
                                    if (message.Result)
                                    {
                                        while (true)
                                        {
                                            Console.WriteLine("Please Enter Exchange Rate as Per INR");
                                            bool isValid = decimal.TryParse(Console.ReadLine(), out decimal exchangeRate);

                                            if (isValid && exchangeRate == 0)
                                            {
                                                Console.WriteLine($"Provided '{exchangeRate}' Should Not be zero or Empty");
                                                continue;
                                            }
                                            else
                                            {
                                                message = _currencyService.UpdateCurrencyAsync(headManagerBankId, currencyCode, exchangeRate).Result;
                                                if (message.Result)
                                                {
                                                    Console.WriteLine(message.ResultMessage);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(message.Result);
                                        continue;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Exchange Rates Available");
                            break;
                        }
                    }
                    break;

                case 7: //DeleteExchangeRates
                    while (true)
                    {
                        Message message;
                        IEnumerable<Currency> currencies = _bankService.GetExchangeRatesAsync(headManagerBankId).Result;
                        if (currencies is not null)
                        {
                            while (true)
                            {
                                Console.WriteLine("Please Enter currency Code");
                                string currencyCode = Console.ReadLine() ?? string.Empty;
                                message = _validateInputs.ValidateCurrencyCodeFormat(currencyCode);
                                if (message.Result)
                                {
                                    message = _currencyService.ValidateCurrencyAsync(headManagerBankId, currencyCode).Result;
                                    if (message.Result)
                                    {
                                        message = _currencyService.DeleteCurrencyAsync(headManagerBankId, currencyCode).Result;
                                        if (message.Result)
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
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Exchange Rates Available");
                            break;
                        }
                    }
                    break;
            }
        }
    }
}
