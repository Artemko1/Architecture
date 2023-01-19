using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image _imageCurrentHp;

        public void SetValue(float current, float max)
        {
            _imageCurrentHp.fillAmount = current / max;
        }
    }
}