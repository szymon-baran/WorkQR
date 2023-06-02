namespace WorkQR.Application
{
    public interface IPositionService
    {
        Task<List<CompanyPositionDTO>> GetCompanyPositionsByUserName();
        Task<List<SelectDTO<Guid>>> GetCompanyPositionsForUserToSelect();
    }
}
