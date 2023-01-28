using CodeBase.Infrastructure.StaticData;
using CodeBase.Infrastructure.StaticData.Monsters;

namespace CodeBase.Infrastructure.Services.StaticDataProvider
{
    public interface IStaticDataProviderService : IService
    {
        void Load();
        MonsterStaticData ForMonster(MonsterTypeId typeId);
        LevelStaticData ForLevel(string sceneName);
    }
}