using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadMonsters();
        MonsterStaticData ForMonster(MonsterTypeId typeId);
    }
}