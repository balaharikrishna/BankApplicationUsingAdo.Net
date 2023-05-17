using API.Models;
using AutoMapper;
using BankApplicationModels;

namespace API.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile() {
            CreateMap<Bank,BankDto>();
            CreateMap<Branch, BranchDto>();
            CreateMap<Currency, CurrencyDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<HeadManager, HeadManagerDto>();
            CreateMap<Manager, ManagerDto>();
            CreateMap<Staff, StaffDto>();
            CreateMap<Message, MessageDto>();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<TransactionCharges,TransactionChargesDto>();
            CreateMap<ReserveBankManager, ReserveBankManagerDto>();
            CreateMap<AuthenticateUser, AuthenticateUserDto>();
        }
    }
}
