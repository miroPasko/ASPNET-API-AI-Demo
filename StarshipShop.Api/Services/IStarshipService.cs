using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Schemas.Responses;

namespace StarshipShop.Api.Services;

public interface IStarshipService
{
    Task<PaginatedResponse<StarshipResponse>> GetAllAsync(PaginationQuery query);
    Task<StarshipResponse> GetByIdAsync(int id);
    Task<StarshipResponse> CreateAsync(CreateStarshipRequest request);
    Task<StarshipResponse> UpdateAsync(int id, UpdateStarshipRequest request);
    Task DeleteAsync(int id);
}
