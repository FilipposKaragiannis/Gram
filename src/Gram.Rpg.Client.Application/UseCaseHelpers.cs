using System;
using Gram.Rpg.Client.Application.Exceptions;
using Gram.Rpg.Client.Core.Domain.Values;
using Gram.Rpg.Client.Domain.Entities;

namespace Gram.Rpg.Client.Application
{
    public static class UseCaseHelpers
    {
        public static StringArraySummary TryAwardHero(this IPlayer1 player1, IHeroAllocator heroAllocator)
        {
            try
            {
                if (player1.PlayerStats.TotalBattles % 5 == 0)
                    return heroAllocator.AllocateNewHero(player1);

                return null;
            }
            catch (Exception e)
            {
                throw new GApplicationException("Failed to Award hero");
            }
        }
    }
}
