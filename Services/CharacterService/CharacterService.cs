using System.Collections.Generic;
using dotnet_rpg.Models;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using AutoMapper;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> Characters = new List<Character>(){
            new Character(),
            new Character() {Id=1, Name="Sam"}
        };
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            Character character = _mapper.Map<Character>(newCharacter);
            character.Id = Characters.Max(c => c.Id) + 1;
            Characters.Add(character);
            return new ServiceResponse<List<GetCharacterDto>>()
            {
                Data = Characters.Select(character => _mapper.Map<GetCharacterDto>(character)).ToList()
            };
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character = Characters.First(c => c.Id == id);
                Characters.Remove(character);
                serviceResponse.Data = Characters.Select(character => _mapper.Map<GetCharacterDto>(character)).ToList();
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
            return new ServiceResponse<List<GetCharacterDto>>()
            {
                Data = Characters.Select(character => _mapper.Map<GetCharacterDto>(character)).ToList()
            };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            return new ServiceResponse<GetCharacterDto>()
            {
                Data = _mapper.Map<GetCharacterDto>(Characters.FirstOrDefault(c => c.Id == id))
            };
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = Characters.FirstOrDefault(c => c.Id == updateCharacter.Id);
                character.Name = updateCharacter.Name;
                character.Class = updateCharacter.Class;
                character.Defence = updateCharacter.Defence;
                character.HitPoints = updateCharacter.HitPoints;
                character.Intelligence = updateCharacter.Intelligence;
                character.Strength = updateCharacter.Strength;
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