using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationServices.IServices;

namespace BankApplication
{
    public class StaffHelperService : IStaffHelperService
    {
        readonly ICustomerService _customerService;
        readonly IBankService _bankService;
        readonly IBranchService _branchService;
        readonly ICommonHelperService _commonHelperService;
        readonly IValidateInputs _validateInputs;
        readonly ITransactionService _transactionService;
        readonly ICurrencyService _currencyService;
        public StaffHelperService(IBankService bankService, IBranchService branchService, ICustomerService customerService,
        ICommonHelperService commonHelperService, IValidateInputs validateInputs, ITransactionService transactionService,
        ICurrencyService currencyService)
        {
            _bankService = bankService;
            _branchService = branchService;
            _commonHelperService = commonHelperService;
            _transactionService = transactionService;
            _currencyService = currencyService;
            _customerService = customerService;
            _validateInputs = validateInputs;
        }

        public void SelectedOption(ushort Option, string staffBankId, string staffBranchId)
        {
            switch (Option)
            {
                case 1: //OpenCustomerAccount
                    _commonHelperService.OpenCustomerAccount(staffBranchId, _customerService, _validateInputs);
                    break;

                case 2: //UpdateCustomerAccount
                    _commonHelperService.UpdateCustomerAccount(staffBranchId, _validateInputs, _customerService);
                    break;

                case 3://DeleteCustomerAccount
                    _commonHelperService.DeleteCustomerAccount(staffBranchId, _customerService, _validateInputs);
                    break;

                case 4://Displaying Customer Transaction History
                    _commonHelperService.GetTransactoinHistory(staffBranchId, _customerService, _validateInputs,
                        _transactionService, Miscellaneous.staff, null);
                    break;

                case 5://Revert Customer Transaction
                    _commonHelperService.RevertCustomerTransaction(staffBankId, staffBranchId, _customerService, _validateInputs,
                       _transactionService, _bankService, _branchService);
                    break;

                case 6://Check Customer Account Balance
                    _commonHelperService.GetCustomerAccountBalance(staffBranchId, _customerService, _validateInputs,
                        Miscellaneous.staff, null);
                    break;

                case 7:// Get ExchangeRates
                    _commonHelperService.GetExchangeRates(staffBankId, _bankService);
                    break;

                case 8:// Get TransactionCharges
                    _commonHelperService.GetTransactionCharges(staffBranchId, _branchService);
                    break;

                case 9://Deposit Amount in Customer Account
                    _commonHelperService.DepositAmountInCustomerAccount(staffBranchId, _customerService, _validateInputs,
                        _currencyService);
                    break;

                case 10:// Transfer Amount 
                    _commonHelperService.TransferAmount(staffBankId, staffBranchId, _branchService, _bankService, _validateInputs,
                        _customerService, Miscellaneous.staff, null);
                    break;
            }
        }
    }
}
