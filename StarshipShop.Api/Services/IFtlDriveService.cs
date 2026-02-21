using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Schemas.Responses;

namespace StarshipShop.Api.Services;

public interface IFtlDriveService
{
    Task<PaginatedResponse<FtlDriveResponse>> GetAllAsync(PaginationQuery query);
    Task<FtlDriveResponse> GetByIdAsync(int id);
    Task<FtlDriveResponse> CreateAsync(CreateFtlDriveRequest request);
    Task<FtlDriveResponse> UpdateAsync(int id, UpdateFtlDriveRequest request);
    Task DeleteAsync(int id);
}
