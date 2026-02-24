using AutoMapper;
using backend_plac_pre.Models;

namespace backend_plac_pre.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Auth Mapper
            CreateMap<DTO.Register_Dto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

            CreateMap<User, DTO.UserProfileDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.roleName));
            #endregion

            #region Ticket Mapper
            CreateMap<DTO.Ticket_Dto, Ticket>();
            CreateMap<DTO.Ticket_Update_Dto, Ticket>();
            #endregion

            #region Comment Mapper
            CreateMap<DTO.Comment_Dto, Ticket_Coment>();
            #endregion
            #region Ticket Status Mapper
            CreateMap<DTO.Ticket_Status_Log_Dto, Ticket_Status_Log>();
            #endregion

        }
    }
}
