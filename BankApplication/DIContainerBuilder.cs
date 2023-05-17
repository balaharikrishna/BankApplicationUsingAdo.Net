using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationServices.IServices;
using BankApplicationServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BankApplication
{
    public static class DIContainerBuilder
    {
        public static IServiceProvider Build()
        {
            ServiceCollection services = new();
            services.AddSingleton<IBankService, BankService>();
            services.AddSingleton<IBranchService, BranchService>();
            services.AddSingleton<ICurrencyService, CurrencyService>();
            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<IEncryptionService, EncryptionService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IHeadManagerService, HeadManagerService>();
            services.AddSingleton<IManagerService, ManagerService>();
            services.AddSingleton<IReserveBankManagerService, ReserveBankManagerService>();
            services.AddSingleton<IStaffService, StaffService>();
            services.AddSingleton<ITransactionChargeService, TransactionChargeService>();
            services.AddSingleton<ITransactionService, TransactionService>();
            services.AddSingleton<ICommonHelperService, CommonHelperService>();
            services.AddSingleton<ICustomerHelperService, CustomerHelperService>();
            services.AddSingleton<IHeadManagerHelperService, HeadManagerHelperService>();
            services.AddSingleton<IManagerHelperService, ManagerHelperService>();
            services.AddSingleton<IReserveBankManagerHelperService, ReserveBankManagerHelperService>();
            services.AddSingleton<IStaffHelperService, StaffHelperService>();
            services.AddSingleton<IValidateInputs, ValidateInputs>();
            services.AddSingleton<ICurrencyService, CurrencyService>();

            return services.BuildServiceProvider();
        }
    }
}