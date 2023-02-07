using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public Button CloseButton;

        protected virtual void Awake() => CloseButton.onClick.AddListener(() => Destroy(gameObject));
    }
}