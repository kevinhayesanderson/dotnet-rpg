using System.Collections.Generic;
using dotnet_rpg.Models;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using AutoMapper;
using dotnet_rpg.Data;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            Character character = _mapper.Map<Character>(newCharacter);
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            return new ServiceResponse<List<GetCharacterDto>>()
            {
                Data = _context.Characters.Select(character => _mapper.Map<GetCharacterDto>(character)).ToList()
            };
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character = await _context.Characters.FirstAsync(c => c.Id == id);

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _context.Characters.Select(character => _mapper.Map<GetCharacterDto>(character)).ToList();
            }
            catch (System.Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
        {
            List<Character> dbCharater = await _context.Characters.ToListAsync();
            return new ServiceResponse<List<GetCharacterDto>>()
            {
                Data = dbCharater.Select(character => _mapper.Map<GetCharacterDto>(character)).ToList()
            };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            Character dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            return new ServiceResponse<GetCharacterDto>()
            {
                Data = _mapper.Map<GetCharacterDto>(dbCharacter)
            };
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
                character.Name = updateCharacter.Name;
                character.Class = updateCharacter.Class;
                character.Defence = updateCharacter.Defence;
                character.HitPoints = updateCharacter.HitPoints;
                character.Intelligence = updateCharacter.Intelligence;
                character.Strength = updateCharacter.Strength;

                _context.Characters.Update(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (System.Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}