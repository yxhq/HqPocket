namespace HqPocket.Extensions.Regioning;

public interface IRegionManager
{
    IRegion GetRegion(string regionName);
}