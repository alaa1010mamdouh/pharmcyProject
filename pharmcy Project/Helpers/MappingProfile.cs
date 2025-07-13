using AutoMapper;
using Pharmcy.Core.Entities;
using pharmcy_Project.DTO;

namespace pharmcy_Project.Helpers
{
    public class MappingProfile:Profile
    {
      public MappingProfile()
        {
            CreateMap<Medicine, MedicineDto>()
                .ReverseMap();
            // Add more mappings as needed

            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer,CreateUpdateCustomerDto>().ReverseMap();

            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<InvoiceItem, InvoiceItemDto>().ReverseMap();
            CreateMap<PrescriptionItem, PrescriptionItemDto>()
    .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.Medicine.Name))
    .ForMember(dest => dest.MedicineDescription, opt => opt.MapFrom(src => src.Medicine.Description))
    .ForMember(dest => dest.MedicinePrice, opt => opt.MapFrom(src => src.Medicine.Price));

            CreateMap<CreatePrescriptionItemDto, PrescriptionItem>();
        }
    }
}
