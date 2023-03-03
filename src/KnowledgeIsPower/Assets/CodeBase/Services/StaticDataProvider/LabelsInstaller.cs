using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace CodeBase.Services.StaticDataProvider
{
    [CreateAssetMenu(fileName = "LabelsInstaller", menuName = "Installers/LabelsInstaller", order = 0)]
    public class LabelsInstaller : ScriptableObjectInstaller<LabelsInstaller>
    {
        [SerializeField] private StaticDataLabels _staticDataLabels;


        public override void InstallBindings() =>
            Container.BindInstance(_staticDataLabels);
    }

    [Serializable]
    public class StaticDataLabels
    {
        [field: SerializeField] public AssetLabelReference Monsters { get; private set; }
        [field: SerializeField] public AssetLabelReference Levels { get; private set; }
    }
}