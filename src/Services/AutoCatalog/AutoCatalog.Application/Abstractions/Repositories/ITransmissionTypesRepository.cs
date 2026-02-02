using AutoCatalog.Application.TransmissionTypes;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions.Repositories;

public interface ITransmissionTypesRepository : IRepository<int, TransmissionType, TransmissionTypeFilter>
{
}