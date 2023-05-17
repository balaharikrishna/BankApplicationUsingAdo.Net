using BankApplicationModels;
using BankApplicationRepository.IRepository;
using BankApplicationServices.IServices;


namespace BankApplicationServices.Services
{
    public class TransactionChargeService : ITransactionChargeService
    {
        private readonly IBranchService _branchService;
        private readonly ITransactionChargeRepository _transactionChargeRepository;

        public TransactionChargeService(IBranchService branchService, ITransactionChargeRepository transactionChargeRepository)
        {
            _branchService = branchService;
            _transactionChargeRepository = transactionChargeRepository;
        }

        public async Task<Message> ValidateTransactionChargesAsync(string branchId)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                bool isTransactionChargeExist = await _transactionChargeRepository.IsTransactionChargesExist(branchId);

                if (isTransactionChargeExist)
                {
                    message.Result = true;
                    message.ResultMessage = $"Transaction Charges Exist";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Transaction Charges Not Exist";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public async Task<TransactionCharges> GetTransactionCharges(string branchId)
        {
            return await _transactionChargeRepository.GetTransactionCharges(branchId);
        }

        public async Task<Message> AddTransactionChargesAsync(string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
        {
            Message message;
            message = await _branchService.AuthenticateBranchIdAsync(branchId);
            if (message.Result)
            {
                TransactionCharges transactionChargesObject = await GetTransactionCharges(branchId);
                if (transactionChargesObject is null)
                {
                    TransactionCharges transactionCharges = new()
                    {
                        RtgsSameBank = rtgsSameBank,
                        RtgsOtherBank = rtgsOtherBank,
                        ImpsSameBank = impsSameBank,
                        ImpsOtherBank = impsOtherBank,
                        IsActive = true
                    };
                    bool isTransactionChargesAdded = await _transactionChargeRepository.AddTransactionCharges(transactionCharges, branchId);
                    if (isTransactionChargesAdded)
                    {
                        message.Result = true;
                        message.ResultMessage = "Transaction Charges Added Successfully";
                        message.Data = $"RtgsSameBank:{rtgsSameBank},RtgsOtherBank = {rtgsOtherBank},ImpsSameBank = {impsSameBank},ImpsOtherBank = {impsOtherBank}";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Failed to Transaction Charges";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Transaction Charges already Available.";
                }
                
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> UpdateTransactionChargesAsync(string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
        {
            Message message;
            message = await ValidateTransactionChargesAsync(branchId);
            if (message.Result)
            {
                TransactionCharges transactionCharges = new()
                {
                    RtgsSameBank = rtgsSameBank,
                    RtgsOtherBank = rtgsOtherBank,
                    ImpsSameBank = impsSameBank,
                    ImpsOtherBank = impsOtherBank,
                    IsActive= true
                };
                bool isTransactionChargeUpdated = await _transactionChargeRepository.UpdateTransactionCharges(transactionCharges, branchId);
                if (isTransactionChargeUpdated)
                {
                    message.Result = true;
                    message.ResultMessage = "Transaction Charges Updated Successfully";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Updating Transaction Charges Failed";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Transaction Charges not Found";
            }
            return message;
        }

        public async Task<Message> DeleteTransactionChargesAsync(string branchId)
        {
            Message message;
            message = await ValidateTransactionChargesAsync(branchId);
            if (message.Result)
            {
                bool isTransactionChargeDeleted = await _transactionChargeRepository.DeleteTransactionCharges(branchId);
                if (isTransactionChargeDeleted)
                {
                    message.Result = true;
                    message.ResultMessage = $"Transaction Charges Deleted Successfully.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Delete Transaction Charges";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Transaction Charges not Found";
            }
            return message;
        }
    }
}
