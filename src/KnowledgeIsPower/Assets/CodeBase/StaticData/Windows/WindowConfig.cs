using System;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.StaticData.Windows
{
    [Serializable]
    public class WindowConfig
    {
        [field: SerializeField] public WindowId WindowId { get; private set; }
        [field: SerializeField] public WindowBase Prefab { get; private set; }
    }
}