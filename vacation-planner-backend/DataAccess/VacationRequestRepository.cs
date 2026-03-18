using Base.Contracts.DTO;
using Base.DataAccess.EF;
using Contract.Application;
using Domain;
using DTO.DataAccess;
using DTO.DataAccess.Mappers;

namespace DataAccess; 

public class VacationRequestRepository : BaseRepository<VacationRequest, VacationRequestEntity, IMapper<VacationRequest, VacationRequestEntity>>, IVacationRequestRepository
{
    public VacationRequestRepository(AppDbContext repositoryDbContext, IMapper<VacationRequest, VacationRequestEntity> repositoryMapper) 
        : base(repositoryDbContext, repositoryMapper)
    {
    }
}