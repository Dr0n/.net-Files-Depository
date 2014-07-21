using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Web;
using AutoMapper;
using FilesDepositoryApplication.Models.Info;
using FilesDepositoryApplication.Models.SqlRepository;

namespace FilesDepositoryApplication.Models
{
    public class CommonMapper : IMapper
    {
        static CommonMapper()
        {
            Mapper.CreateMap<File, UploadedFile>();
            //mapping rules for convert uploaded file to sql model
            Mapper.CreateMap<UploadedFile, File>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.GeneratedName))
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.Created))
                .ForMember(dest => dest.original_name, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.type, opt => opt.MapFrom(src => src.FileExtention));
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}