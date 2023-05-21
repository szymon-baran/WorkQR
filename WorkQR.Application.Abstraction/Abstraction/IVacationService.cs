using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public interface IVacationService
    {
        Task<List<VacationRequestDTO>> GetVacationRequestsByUsername(string userName);
        List<SelectDTO<VacationType>> GetVacationTypes();
        Task AddVacationRequest(string userName, VacationRequestVM model);
        Task<bool> ValidateVacationRequest(string userName, VacationValidationVM model);
        Task<List<VacationRequestModeratorDTO>> GetModeratorVacationRequestsByUsername(string userName);
        Task AcceptVacationRequest(string userName, Guid id);
        Task RejectVacationRequest(string userName, ModeratorVacationRejectionVM model);
    }
}
