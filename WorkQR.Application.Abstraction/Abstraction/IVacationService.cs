using WorkQR.Domain.Dictionaries;

namespace WorkQR.Application
{
    public interface IVacationService
    {
        Task<List<VacationRequestDTO>> GetVacationRequestsByUsername();
        List<SelectDTO<VacationType>> GetVacationTypes();
        Task AddVacationRequest(VacationRequestVM model);
        Task<bool> ValidateVacationRequest(VacationValidationVM model);
        Task<List<ModeratorVacationRequestDTO>> GetModeratorAllVacationRequests();
        Task<List<ModeratorEmployeeVacationDetailsDTO>> GetModeratorVacationRequestsByUser(GetUserDetailsVM model);
        Task AcceptVacationRequest(Guid id);
        Task RejectVacationRequest(ModeratorVacationRejectionVM model);
    }
}
