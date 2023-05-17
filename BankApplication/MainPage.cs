using BankApplication;
using BankApplication.Exceptions;
using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;
using Microsoft.Extensions.DependencyInjection;

internal class MainPage
{
    public static readonly IServiceProvider services = DIContainerBuilder.Build();
    private static void Main()
    {
        IBankService bankService = services.GetService<IBankService>()!;
        IBranchService branchService = services.GetService<IBranchService>()!;
        ICommonHelperService commonHelperService = services.GetService<ICommonHelperService>()!;
        IValidateInputs validateInputs = services.GetService<IValidateInputs>()!;

        while (true)
        {
            try
            {
                Console.WriteLine("Welcome... Please Choose the Following Options..");
                foreach (MainPageOptions option in Enum.GetValues(typeof(MainPageOptions)))
                {
                    Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                }

                ushort Option = commonHelperService.GetOption(Miscellaneous.mainPage);

                switch (Option)
                {
                    case 1: //Customer Login
                        ICustomerHelperService customerHelperService = services.GetService<ICustomerHelperService>()!;
                        ICustomerService customerService = services.GetService<ICustomerService>()!;
                        commonHelperService.LoginAccountHolder(Miscellaneous.customer, bankService, branchService, validateInputs, customerHelperService,
                            null, null, null, null, customerService, null, null, null);
                        break;

                    case 2: //Staff Login
                        IStaffService staffService = services.GetService<IStaffService>()!;
                        IStaffHelperService staffHelperService = services.GetService<IStaffHelperService>()!;
                        commonHelperService.LoginAccountHolder(Miscellaneous.staff, bankService, branchService, validateInputs, null,
                            staffHelperService, null, null, null, null, staffService, null, null);
                        break;

                    case 3: //Manager Login
                        IManagerService managerService = services.GetService<IManagerService>()!;
                        IManagerHelperService managerHelperService = services.GetService<IManagerHelperService>()!;
                        commonHelperService.LoginAccountHolder(Miscellaneous.branchManager, bankService, branchService, validateInputs, null,
                            null, managerHelperService, null, null, null, null, managerService, null);
                        break;

                    case 4: //Head Manager Login
                        IHeadManagerHelperService headManagerHelperService = services.GetService<IHeadManagerHelperService>()!;
                        IHeadManagerService headManagerService = services.GetService<IHeadManagerService>()!;
                        commonHelperService.LoginAccountHolder(Miscellaneous.headManager, bankService, branchService, validateInputs, null,
                            null, null, headManagerHelperService, null, null, null, null, headManagerService);
                        break;

                    case 5: //Reserve Bank Login
                        IReserveBankManagerService reserveBankManagerService = services.GetService<IReserveBankManagerService>()!;
                        IReserveBankManagerHelperService reserveBankManagerHelperService = services.GetService<IReserveBankManagerHelperService>()!;
                        commonHelperService.LoginAccountHolder(Miscellaneous.reserveBankManager, bankService, branchService, validateInputs, null,
                            null, null, null, reserveBankManagerHelperService, null, null, null, null, reserveBankManagerService);
                        break;
                    default:
                        throw new InvalidOptionException(Option);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                continue;
            }
        }

    }
}