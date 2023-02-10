using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [SerializeField] private WindowId _windowId;
        private IWindowService _windowService;

        private void Awake() =>
            _button.onClick.AddListener(Open);

        public void Construct(IWindowService windowService) =>
            _windowService = windowService;

        private void Open() =>
            _windowService.Open(_windowId);
    }
}