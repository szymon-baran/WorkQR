namespace WorkQR.Application
{
    public interface IPositionService
    {
        Task<List<CompanyPositionDTO>> GetCompanyPositionsByUserName(string userName);
        Task<List<SelectVM<Guid>>> GetCompanyPositionsForUserToSelect(string userName);
    }
}
