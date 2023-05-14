namespace WorkQR.Application
{
    public interface IPositionService
    {
        Task<List<CompanyPositionDTO>> GetCompanyPositionsByUserName(string userName);
        Task<List<SelectDTO<Guid>>> GetCompanyPositionsForUserToSelect(string userName);
    }
}
