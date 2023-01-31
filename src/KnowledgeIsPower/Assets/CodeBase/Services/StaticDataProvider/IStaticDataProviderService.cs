using CodeBase.StaticData;
using CodeBase.StaticData.Monsters;

namespace CodeBase.Services.StaticDataProvider
{
    public interface IStaticDataProviderService : IService
    {
        void Load();
        MonsterStaticData ForMonster(MonsterTypeId typeId);
        LevelStaticData ForLevel(string sceneName);
        PlayerProgressStaticData ForNewGame();
        HeroStaticData ForHero();
    }
}