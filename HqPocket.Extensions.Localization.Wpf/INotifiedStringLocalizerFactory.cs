using Microsoft.Extensions.Localization;

namespace HqPocket.Extensions.Localization
{
    public interface INotifiedStringLocalizerFactory : IStringLocalizerFactory
    {
        void RefreshLocalizedString();
    }
}
