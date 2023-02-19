using UnityEngine;

namespace CodeBase.Services.Randomizer
{
    public class RandomService : IService
    {
        public int Next(int min, int max) =>
            Random.Range(min, max);
    }
}