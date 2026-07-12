using AutoMapper;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.ViewModels;

namespace StationeryStore.Mvc.Mapper;

//automapper for stationery website mvc
public class StationeryMapper : Profile
{
    public StationeryMapper()
    {
        //map stationery entities
        CreateMap<Stationery, StationeryDetailViewModel>();
        CreateMap<StationeryEditViewModel, Stationery>()
                    .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))
                    .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<StationeryCreateViewModel, Stationery>()
                    .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice));
        CreateMap<Stationery, StationeryTrashViewModel>();
        CreateMap<Stationery, StationeryListItemViewModel>();

        //map category entities
        CreateMap<Category, CategoryListItemViewModel>();
        //map cart entities
        CreateMap<Cart, CartViewModel>()
                    .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id));

        CreateMap<CartItem, CartItemListViewModel>()
                    .ForMember(dest => dest.StationeryName, opt => opt.MapFrom(src => src.Stationery.Name));
                    }
}