using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Design;

namespace Gram.Rpg.Client.Domain.Entities
{
    public interface IHeroInventory : IEnumerable<IOwnedHero>
    {
        [CanBeNull] IOwnedHero this[string key] { get; }

        void Add(IHero  hero);
        bool Has(string id);
    }

    public class HeroInventory : IHeroInventory
    {
        private readonly Dictionary<string, OwnedHero> _ownedHeroes;

        public HeroInventory(IEnumerable<OwnedHero> heroes)
        {
            _ownedHeroes = heroes.ToDictionary(s => s.Id, s => s);
        }

        public HeroInventory(Dictionary<string, OwnedHero> ownedHeroes = null)
        {
            _ownedHeroes = ownedHeroes ?? new Dictionary<string, OwnedHero>();
        }

        public IEnumerator<IOwnedHero> GetEnumerator()
        {
            return _ownedHeroes.Select(s => s.Value).GetEnumerator();
        }

        public IOwnedHero this[string key]
        {
            get
            {
                if (_ownedHeroes.TryGetValue(key, out var h))
                    return h;

                return null;
            }
        }

        public void Add(IHero hero)
        {
            if (_ownedHeroes.ContainsKey(hero.Id))
            {
                G.LogWarning($"Hero {hero.Id} already exists on inventory");
                return;
            }

            _ownedHeroes[hero.Id] = new OwnedHero(hero);
        }

        public bool Has(string id)
        {
            return _ownedHeroes.ContainsKey(id);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
