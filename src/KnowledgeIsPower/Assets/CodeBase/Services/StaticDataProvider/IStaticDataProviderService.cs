using CodeBase.StaticData;
using CodeBase.StaticData.Hero;
using CodeBase.StaticData.Monsters;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Services.StaticDataProvider
{
    public interface IStaticDataProviderService : IService
    {
        void Load();
        MonsterStaticData ForMonster(MonsterTypeId typeId);
        LevelStaticData ForLevel(string sceneName);
        PlayerProgressStaticData ForNewGame();
        HeroStaticData ForHero();
        WindowConfig ForWindow(WindowId shop);
    }
}