using System.Linq;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Design;
using Gram.Rpg.Client.Domain.Entities;
using Gram.Rpg.Client.Domain.Values;
using Gram.Rpg.Client.Infrastructure.Dtos;

namespace Gram.Rpg.Client.Infrastructure.Mappers
{
    public interface IPlayer1Mapper
    {
        PlayerDto ToDto(IPlayer1 player1);

        [CanBeNull]
        IPlayer1 FromDto(PlayerDto dto);
    }

    public class Player1Mapper : IPlayer1Mapper
    {
        public PlayerDto ToDto(IPlayer1 player1)
        {
            return new PlayerDto
            {
                Id = player1.Id,
                OwnedHeroes = player1.HeroInventory.Select(s => new HeroDto
                {
                    Id               = s.Id,
                    Name             = s.Name,
                    MaxHealth        = s.MaxHealth,
                    AttackPower      = s.AttackPower,
                    ExperiencePoints = s.ExperiencePoints,
                    Level            = s.Level
                }).ToArray(),
                PlayerStats = new PlayerStatsDto()
                {
                    BattleHistoryEntries = player1.PlayerStats.Select(s => new BattleHistoryDto
                    {
                        MatchResult = (int) s.MatchResult,
                        HeroesUsed  = s.HeroesUsed.ToArray()
                    }).ToArray()
                }
            };
        }

        public IPlayer1 FromDto(PlayerDto dto)
        {
            if (dto == null)
            {
                G.LogError($"Tried to map Player from Dto but dto was null");
                return null;
            }

            var inventory = GetInventory(dto.OwnedHeroes);
            var stats     = GetPlayerStats(dto.PlayerStats);
            return new Player1(dto.Id,
                inventory,
                stats);
        }

        private static IPlayerStats GetPlayerStats(PlayerStatsDto stats)
        {
            if (stats == null)
                return new PlayerStats();

            return new PlayerStats(stats.BattleHistoryEntries
                .Select(s => new BattleHistoryEntry((MatchResult) s.MatchResult,
                    s.HeroesUsed)));
        }

        private static IHeroInventory GetInventory(HeroDto[] ownedHeroes)
        {
            if (ownedHeroes == null)
                return new HeroInventory();

            return new HeroInventory(ownedHeroes.ToDictionary(kv => kv.Id,
                h => new OwnedHero(h.Id,
                    h.Name,
                    h.AttackPower,
                    h.ExperiencePoints,
                    h.Level,
                    h.MaxHealth)));
        }
    }
}
