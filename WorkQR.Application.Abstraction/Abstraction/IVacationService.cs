using WorkQR.Domain.Dictionaries;

namespace WorkQR.Application
{
    public interface IVacationService
    {
        Task<List<VacationRequestDTO>> GetVacationRequestsByUsername(string userName);
        List<SelectDTO<VacationType>> GetVacationTypes();
        Task AddVacationRequest(string userName, VacationRequestVM model);
        Task<bool> ValidateVacationRequest(string userName, VacationValidationVM model);
        Task<List<ModeratorVacationRequestDTO>> GetModeratorAllVacationRequests(string userName);
        Task<List<ModeratorEmployeeVacationDetailsDTO>> GetModeratorVacationRequestsByUser(GetUserDetailsVM model);
        Task AcceptVacationRequest(string userName, Guid id);
        Task RejectVacationRequest(string userName, ModeratorVacationRejectionVM model);
    }
}
