using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WorkQR.Infrastructure.Abstraction;
using WorkQR.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace WorkQR.Application
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PositionService(IPositionRepository positionRepository,
                               UserManager<ApplicationUser> userManager,
                               IMapper mapper,
                               IHttpContextAccessor httpContextAccessor)
        {
            _positionRepository = positionRepository;
            _userManager = userManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CompanyPositionDTO>> GetCompanyPositionsByUserName()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity?.Name ?? throw new UnauthorizedAccessException();

            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<Position> positions = await _positionRepository.GetWithConditionAsync(x => x.CompanyId == user.Position.CompanyId);

            return _mapper.Map<IEnumerable<Position>, List<CompanyPositionDTO>>(positions);
        }

        public async Task<List<SelectDTO<Guid>>> GetCompanyPositionsForUserToSelect()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity?.Name ?? throw new UnauthorizedAccessException();

            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<Position> positions = await _positionRepository.GetWithConditionAsync(x => x.CompanyId == user.Position.CompanyId);

            return positions.Select(x => new SelectDTO<Guid>()
            {
                Label = x.Name,
                Value = x.Id
            }).ToList();
        }
    }
}