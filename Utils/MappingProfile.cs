using AutoMapper;
using Celsia.ViewModels;
using Celsia.Models;

namespace Celsia.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo para User
            CreateMap<UserCreateViewModel, User>();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<UserUpdateViewModel, User>();

            // Mapeo para Transaction
            CreateMap<TransactionCreateViewModel, Transaction>();
            CreateMap<Transaction, TransactionViewModel>().ReverseMap();
            CreateMap<TransactionUpdateViewModel, Transaction>();

            // Mapeo para Invoice
            CreateMap<InvoiceCreateViewModel, Invoice>();
            CreateMap<Invoice, InvoiceViewModel>().ReverseMap();
            CreateMap<InvoiceUpdateViewModel, Invoice>();

            // Mapeo para Platform
            CreateMap<PlatformCreateViewModel, Platform>();
            CreateMap<Platform, PlatformViewModel>().ReverseMap();
            CreateMap<PlatformUpdateViewModel, Platform>();
        }
    }
}
