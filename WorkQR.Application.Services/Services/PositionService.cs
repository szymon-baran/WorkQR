using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WorkQR.Data.Abstraction;
using WorkQR.Domain;

namespace WorkQR.Application
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public PositionService(IPositionRepository positionRepository, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _positionRepository = positionRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<CompanyPositionDTO>> GetCompanyPositionsByUserName(string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<Position> positions = await _positionRepository.GetWithConditionAsync(x => x.CompanyId == user.Position.CompanyId);

            return _mapper.Map<IEnumerable<Position>, List<CompanyPositionDTO>>(positions);
        }

        public async Task<List<SelectVM<Guid>>> GetCompanyPositionsForUserToSelect(string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<Position> positions = await _positionRepository.GetWithConditionAsync(x => x.CompanyId == user.Position.CompanyId);

            return positions.Select(x => new SelectVM<Guid>()
            {
                Label = x.Name,
                Value = x.Id
            }).ToList();
        }
    }
}