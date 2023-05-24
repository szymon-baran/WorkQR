using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using WorkQR.Infrastructure.Abstraction;
using WorkQR.Domain.Dictionaries;
using WorkQR.Domain.Models;

namespace WorkQR.Application
{
    public class VacationService : IVacationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVacationRepository _vacationRepository;
        private readonly IMapper _mapper;

        public VacationService(UserManager<ApplicationUser> userManager, IVacationRepository vacationRepository, IMapper mapper)
        {
            _userManager = userManager;
            _vacationRepository = vacationRepository;
            _mapper = mapper;
        }

        public async Task<List<VacationRequestDTO>> GetVacationRequestsByUsername(string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<Vacation> vacationRequests = await _vacationRepository.GetWithConditionAsync(x => x.ApplicationUserId == user.Id);

            return _mapper.Map<IEnumerable<Vacation>, List<VacationRequestDTO>>(vacationRequests);
        }

        public List<SelectDTO<VacationType>> GetVacationTypes()
        {
            return Enum.GetValues(typeof(VacationType)).Cast<VacationType>().Select(x => new SelectDTO<VacationType>()
            {
                Label = EnumExtensions.GetEnumDescription(x),
                Value = x
            }).ToList();
        }

        public async Task AddVacationRequest(string userName, VacationRequestVM model)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            Vacation vacation = _mapper.Map<Vacation>(model);
            vacation.ApplicationUserId = user.Id;

            await _vacationRepository.AddAsync(vacation);
            await _vacationRepository.SaveChangesAsync();
        }

        public async Task<bool> ValidateVacationRequest(string userName, VacationValidationVM model)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            int usedDays = await GetUsedVacationDaysByUser(user.Id);
            int leftDays = ((user.VacationDaysPerYear ?? 0) - usedDays);

            if ((user.VacationDaysPerYear ?? 0) - usedDays <= (model.DateTo - model.DateFrom).TotalDays && model.VacationType == VacationType.AnnualLeave)
            {
                throw new Exception($"Pozostało Ci tylko {leftDays} dni urlopu, a próbowałeś złożyć wniosek na {(model.DateTo - model.DateFrom).TotalDays}.");
            }

            return true;
        }

        private async Task<int> GetUsedVacationDaysByUser(string userId)
        {
            IEnumerable<Vacation> vacationsThisYear = await _vacationRepository.GetWithConditionAsync(x => x.ApplicationUserId == userId && x.DateTo.Year == DateTime.Today.Year && x.IsApproved && !x.IsRejected && x.VacationType == VacationType.AnnualLeave);
            return (int)Math.Round(vacationsThisYear.Sum(x => (x.DateTo - x.DateFrom).TotalDays));
        }

        #region Moderation

        public async Task<List<VacationRequestModeratorDTO>> GetModeratorVacationRequestsByUsername(string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<Vacation> vacationRequests = await _vacationRepository.GetWithConditionAsync(x => x.ApplicationUser.Position.CompanyId == user.Position.CompanyId
                                                                                                          && !x.IsApproved
                                                                                                          && !x.IsRejected);
            return vacationRequests.Select(x => new VacationRequestModeratorDTO()
            {
                Id = x.Id,
                RequestDescription = x.RequestDescription,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                VacationType = x.VacationType,
                ApplicationUserId = x.ApplicationUserId,
                FirstName = x.ApplicationUser.FirstName,
                LastName = x.ApplicationUser.LastName,
                Username = x.ApplicationUser.UserName ?? "",
            }).ToList();
        }

        public async Task AcceptVacationRequest(string userName, Guid id)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            Vacation vacationRequest = await _vacationRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (vacationRequest == null)
                throw new KeyNotFoundException("Nie znaleziono wybranego rekordu!");

            vacationRequest.IsApproved = true;
            vacationRequest.IsRejected = false;
            _vacationRepository.Update(vacationRequest);
            await _vacationRepository.SaveChangesAsync();
        }

        public async Task RejectVacationRequest(string userName, ModeratorVacationRejectionVM model)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            Vacation vacationRequest = await _vacationRepository.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (vacationRequest == null)
                throw new KeyNotFoundException("Nie znaleziono wybranego rekordu!");

            vacationRequest.IsApproved = false;
            vacationRequest.IsRejected = true;
            vacationRequest.RejectionDescription = model.RejectionDescription;
            _vacationRepository.Update(vacationRequest);
            await _vacationRepository.SaveChangesAsync();
        }

        #endregion Moderation
    }
}