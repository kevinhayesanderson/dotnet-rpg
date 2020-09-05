using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Fight;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public FightService(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto fightRequestDto)
        {
            ServiceResponse<FightResultDto> serviceResponse = new ServiceResponse<FightResultDto>()
            {
                Data = new FightResultDto()
            };
            try
            {
                List<Character> characters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
                .Where(c => fightRequestDto.CharacterIds.Contains(c.Id))
                .ToListAsync();

                bool defeated = false;
                while (!defeated)
                {
                    foreach (Character attacker in characters)
                    {
                        List<Character> opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        Character opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else
                        {
                            int randomSkill = new Random().Next(attacker.CharacterSkills.Count);
                            attackUsed = attacker.CharacterSkills[randomSkill].Skill.Name;
                            damage = DoSkillAttack(attacker, opponent, attacker.CharacterSkills[randomSkill]);
                        }

                        serviceResponse.Data.Log.Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage");

                        if (opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            serviceResponse.Data.Log.Add($"{opponent.Name} has been deafeted!");
                            serviceResponse.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left!");
                            break;
                        }
                    }
                }

                characters.ForEach(c => { c.Fights++; c.HitPoints = 100; });

                _context.Characters.UpdateRange(characters);
                await _context.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttackDto)
        {
            ServiceResponse<AttackResultDto> serviceResponse = new ServiceResponse<AttackResultDto>();
            try
            {
                Character attacker = await _context.Characters
                    .Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
                    .FirstOrDefaultAsync(c => c.Id == skillAttackDto.AttackerID);
                if (attacker == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Attacker not found";
                    return serviceResponse;
                }

                Character opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == skillAttackDto.OpponentId);
                if (opponent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Opponent not found";
                    return serviceResponse;
                }

                CharacterSkill characterSkill = attacker.CharacterSkills.FirstOrDefault(cs => cs.Skill.Id == skillAttackDto.SkillId);
                if (characterSkill == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"{attacker.Name} doesn't know that skill";
                    return serviceResponse;
                }

                int damage = DoSkillAttack(attacker, opponent, characterSkill);

                if (opponent.HitPoints <= 0)
                {
                    serviceResponse.Message = $"{opponent.Name} has been defeated";
                }

                _context.Characters.Update(opponent);
                await _context.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDto()
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponenetHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (System.Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        private static int DoSkillAttack(Character attacker, Character opponent, CharacterSkill characterSkill)
        {
            int damage = characterSkill.Skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponent.Defence);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttackDto)
        {
            ServiceResponse<AttackResultDto> serviceResponse = new ServiceResponse<AttackResultDto>();
            try
            {
                Character attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == weaponAttackDto.AttackerID);
                if (attacker == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Attacker not found";
                    return serviceResponse;
                }

                Character opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == weaponAttackDto.OpponentId);
                if (opponent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Opponent not found";
                    return serviceResponse;
                }

                int damage = DoWeaponAttack(attacker, opponent);

                if (opponent.HitPoints <= 0)
                {
                    serviceResponse.Message = $"{opponent.Name} has been defeated";
                }

                _context.Characters.Update(opponent);
                await _context.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDto()
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponenetHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (System.Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defence);
            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<List<HighscoreDto>>> GetHighscore()
        {
            List<Character> characters = await _context.Characters
            .Where(c => c.Fights > 0)
            .OrderByDescending(c => c.Victories)
            .ThenBy(c => c.Defeats)
            .ToListAsync();

            return new ServiceResponse<List<HighscoreDto>>()
            {
                Data = characters.Select(c => _mapper.Map<HighscoreDto>(c)).ToList()
            };

        }
    }
}