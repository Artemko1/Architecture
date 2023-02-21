using System.Threading.Tasks;
using CodeBase.StaticData;
using CodeBase.StaticData.Monsters;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Services.StaticDataProvider
{
    public interface IStaticDataProviderService
    {
        Task Load();
        MonsterStaticData ForMonster(MonsterTypeId typeId);
        LevelStaticData ForLevel(string sceneName);
        LevelStaticData ForDefaultLevel();
        WindowConfig ForWindow(WindowId shop);
    }
}