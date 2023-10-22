using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows.Data;

namespace HqPocket.Extensions.Logging
{
    public static class LoggerExtenison
    {
        public static ICollectionView GetLogItems(this ILogger logger)
        {
            return CollectionViewSource.GetDefaultView(ObservableCollectionLogger.LogItems);
        }
    }
}
