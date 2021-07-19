using System.Collections.Generic;
using Gram.Rpg.Client.Core.Collections;
using Gram.Rpg.Client.Core.Data;
using Gram.Rpg.Client.Core.Extensions;
using Gram.Rpg.Client.Domain.Entities;
using Gram.Rpg.Client.Domain.Values;

namespace Gram.Rpg.Client.Application.Repos
{
    public interface IHeroRepo : IEnumerable<IHero>
    {
        IHero              GetRandomHero();
        IEnumerable<IHero> GetRandomHeroes(int num);
    }

    public class HeroRepo : Repo<IHero>, IHeroRepo
    {
        public HeroRepo(IHero[] data) : base(data)
        {
        }

        public IHero GetRandomHero()
        {
            return data.PickRandom();
        }

        public IEnumerable<IHero> GetRandomHeroes(int num)
        {
            var res = new List<IHero>();

            var temp = data.ToShuffleBag();

            for (var i = 0; i < num; i++)
                res.Add(temp.Next());

            return res;
        }

        public static IHero[] GetData()
        {
            return new IHero[]
            {
                new Hero("HR001",  "Hero1",  AttackPower.Weak,        MaxHealth.WeakLow),
                new Hero("HR002",  "Hero2",  AttackPower.Weak,        MaxHealth.WeakHigh),
                new Hero("HR003",  "Hero3",  AttackPower.AverageLow,  MaxHealth.WeakLow),
                new Hero("HR004",  "Hero4",  AttackPower.AverageHigh, MaxHealth.WeakHigh),
                new Hero("HR005",  "Hero5",  AttackPower.Weak,        MaxHealth.AverageHigh),
                new Hero("HR006",  "Hero6",  AttackPower.AverageLow,  MaxHealth.StrongHigh),
                new Hero("HR007",  "Hero7",  AttackPower.AverageHigh, MaxHealth.StrongLow),
                new Hero("HR008",  "Hero8",  AttackPower.Strong,      MaxHealth.StrongLow),
                new Hero("HR009",  "Hero9",  AttackPower.AverageHigh, MaxHealth.StrongHigh),
                new Hero("HR0010", "Hero10", AttackPower.Weak,        MaxHealth.StrongHigh),
                new Hero("HR0011", "Hero11", AttackPower.Strong,      MaxHealth.WeakLow),
                new Hero("HR0012", "Hero12", AttackPower.Strong,      MaxHealth.WeakHigh),
                new Hero("HR0013", "Hero13", AttackPower.AverageLow,  MaxHealth.AverageLow),
                new Hero("HR0014", "Hero14", AttackPower.AverageHigh, MaxHealth.AverageLow),
            };
        }
    }
}
