using AutoMapper;
using StationeryStore.Mvc.Dto.Response;
using StationeryStore.Mvc.Dto.Request;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.ViewModels;

namespace StationeryStore.Mvc.Mapper;

public class StationeryMapper : Profile
{
    public StationeryMapper()
    {
        CreateMap<StationeryUpdateRequest, Stationery>()
                    .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                    .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                    .ForMember(dest => dest.Category, opt => opt.Ignore());
        CreateMap<Stationery, StationeryResponse>();
        CreateMap<StationeryEditViewModel, StationeryUpdateRequest>()
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice));
        CreateMap<StationeryResponse, StationeryTrashViewModel>();
    }
}