using System;

namespace HqPocket.Extensions.Regioning;

public interface IRegion
{
    IRegion AddView(Type viewType);
    void RemoveView(string viewName);

    bool CanNavigateBack { get; }
    bool CanNavigateForward { get; }
    void NavigateTo(string viewName);
    void NavigateBack();
    void NavigateForward();
}