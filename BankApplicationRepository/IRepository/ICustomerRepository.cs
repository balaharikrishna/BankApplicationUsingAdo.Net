using BankApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationRepository.IRepository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer?>> GetAllCustomers(string branchId);
        Task<bool> AddCustomerAccount(Customer customer, string branchId);
        Task<bool> UpdateCustomerAccount(Customer customer, string branchId);
        Task<bool> DeleteCustomerAccount(string customerAccountId, string branchId);
        Task<bool> IsCustomerExist(string customerAccountId, string branchId);
        Task<Customer?> GetCustomerById(string customerAccountId, string branchId);
        Task<Customer?> GetCustomerByName(string customerName, string branchId);
    }
}
