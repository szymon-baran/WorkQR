﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using WorkQR.Infrastructure.Abstraction;
using WorkQR.Domain.Dictionaries;
using WorkQR.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace WorkQR.Application
{
    public class VacationService : IVacationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVacationRepository _vacationRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VacationService(UserManager<ApplicationUser> userManager,
                               IVacationRepository vacationRepository,
                               IMapper mapper,
                               IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _vacationRepository = vacationRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<VacationRequestDTO>> GetVacationRequestsByUsername()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity?.Name ?? throw new UnauthorizedAccessException();

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

        public async Task AddVacationRequest(VacationRequestVM model)
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity?.Name ?? throw new UnauthorizedAccessException();

            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            Vacation vacation = _mapper.Map<Vacation>(model);
            vacation.ApplicationUserId = user.Id;

            await _vacationRepository.AddAsync(vacation);
            await _vacationRepository.SaveChangesAsync();
        }

        public async Task<bool> ValidateVacationRequest(VacationValidationVM model)
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity?.Name ?? throw new UnauthorizedAccessException();

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

        public async Task<List<ModeratorVacationRequestDTO>> GetModeratorAllVacationRequests()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity?.Name ?? throw new UnauthorizedAccessException();

            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<Vacation> vacationRequests = await _vacationRepository.GetWithConditionAsync(x => x.ApplicationUser.Position.CompanyId == user.Position.CompanyId
                                                                                                          && !x.IsApproved
                                                                                                          && !x.IsRejected);
            return vacationRequests.Select(x => new ModeratorVacationRequestDTO()
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

        public async Task<List<ModeratorEmployeeVacationDetailsDTO>> GetModeratorVacationRequestsByUser(GetUserDetailsVM model)
        {
            IEnumerable<Vacation> vacationRequests = await _vacationRepository.GetWithConditionAsync(x => x.ApplicationUserId == model.UserId 
                                                                            && x.DateFrom.Date <= model.DateTo.Date
                                                                            && model.DateFrom.Date <= x.DateTo.Date
                                                                            && (string.IsNullOrEmpty(model.Description)
                                                                                || x.RejectionDescription.Contains(model.Description) 
                                                                                || x.RequestDescription.Contains(model.Description)));
            return _mapper.Map<IEnumerable<Vacation>, List<ModeratorEmployeeVacationDetailsDTO>>(vacationRequests);
        }

        public async Task AcceptVacationRequest(Guid id)
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity?.Name ?? throw new UnauthorizedAccessException();

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

        public async Task RejectVacationRequest(ModeratorVacationRejectionVM model)
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity?.Name ?? throw new UnauthorizedAccessException();

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