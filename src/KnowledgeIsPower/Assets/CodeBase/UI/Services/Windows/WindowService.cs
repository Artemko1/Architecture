using CodeBase.Services;
using CodeBase.UI.Services.Factory;

namespace CodeBase.UI.Services.Windows
{
    public class WindowService : IService
    {
        private readonly UIFactory _uiFactory;

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