using CodeBase.UI.Services.Factory;
using Zenject;

namespace CodeBase.UI.Services.Windows
{
    public class WindowService
    {
        private readonly UIFactory _uiFactory;

        [Inject]
        public WindowService(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.None:
                    break;
                case WindowId.Shop:
                    _uiFactory.CreateShop();
                    break;
            }
        }
    }
}