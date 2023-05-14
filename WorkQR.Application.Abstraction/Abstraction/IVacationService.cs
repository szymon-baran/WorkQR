using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public interface IVacationService
    {
        Task<List<VacationRequestDTO>> GetVacationRequestsByUsername(string userName);
        List<SelectDTO<VacationType>> GetVacationTypes();
        Task AddVacationRequest(string userName, VacationRequestVM model);
    }
}
