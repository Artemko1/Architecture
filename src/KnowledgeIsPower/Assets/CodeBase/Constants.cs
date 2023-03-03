namespace CodeBase
{
    public static class Constants
    {
        public const float Epsilon = 0.001f;

        public static class Tags
        {
            public const string InitialPoint = "InitialPoint";
        }

        public static class Layers
        {
            public const string Player = "Player";
            public const string Hittable = "Hittable";
        }

        public static class AssetAddress
        {
            public const string HeroPath = "Hero";
            public const string HudPath = "Hud";
            public const string Loot = "Loot";
            public const string EnemySpawner = "SpawnPoint";
            public const string UIRoot = "UIRoot";
        }

        public static class SceneNames
        {
            public const string Initial = "Initial";
            public const string Graveyard = "Graveyard";
            public const string Dungeon = "Dungeon";
        }
    }
}