using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.DTO;
using Data.Models;

namespace BusinessLogic.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<CreateCarDto, Car>();
            CreateMap<CarDto, Car>();
            CreateMap<Car, CarDto>();
        }
    }
}
