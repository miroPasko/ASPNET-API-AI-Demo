using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Schemas.Responses;

namespace StarshipShop.Api.Services;

public interface IEngineService
{
    Task<PaginatedResponse<EngineResponse>> GetAllAsync(PaginationQuery query);
    Task<EngineResponse> GetByIdAsync(int id);
    Task<EngineResponse> CreateAsync(CreateEngineRequest request);
    Task<EngineResponse> UpdateAsync(int id, UpdateEngineRequest request);
    Task DeleteAsync(int id);
}
