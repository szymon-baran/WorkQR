using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using WorkQR.Data.Abstraction;
using WorkQR.Dictionaries;
using WorkQR.Domain;

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
    }
}