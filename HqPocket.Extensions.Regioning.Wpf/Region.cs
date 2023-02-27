
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace HqPocket.Extensions.Regioning;

internal class Region : IRegion
{
    private readonly IRegionAdapterFactory _regionAdapterFactory;
    private readonly ObservableCollection<Type> _viewTypeCollection = new();

    private readonly ConcurrentStack<string> _backStack = new();
    private readonly ConcurrentStack<string> _forwardStack = new();
    internal string Name { get; }
    internal IRegionAdapter? Adapter { get; private set; }
    public bool CanNavigateBack => !_backStack.IsEmpty;
    public bool CanNavigateForward => !_forwardStack.IsEmpty;


    internal Region(string name, IRegionAdapterFactory regionAdapterFactory)
    {
        Name = name;
        _regionAdapterFactory = regionAdapterFactory;
    }

    internal void Initialize(FrameworkElement frameworkElement)
    {
        Adapter = _regionAdapterFactory.Create(frameworkElement);

        foreach (var viewType in _viewTypeCollection)
        {
            AddviewToAdapter(viewType);
        }

        _viewTypeCollection.CollectionChanged += ViewTypeCollection_CollectionChanged;
    }

    private void ViewTypeCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems?.Count > 0)
        {
            foreach (Type viewType in e.NewItems)
            {
                AddviewToAdapter(viewType);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems?.Count > 0)
        {
            foreach (Type viewType in e.OldItems)
            {
                Adapter!.RemoveView(viewType.Name);
            }
        }
    }

    public IRegion AddView(Type viewType)
    {
        _viewTypeCollection.Add(viewType);
        return this;
    }

    public void RemoveView(string viewName)
    {
        _viewTypeCollection.RemoveAll(v => v.Name == viewName);
    }

    private void AddviewToAdapter(Type viewType)
    {
        var view = Ioc.GetRequiredService(viewType);
        Adapter!.AddView(view);
    }


    public void NavigateTo(string viewName)
    {
        NavigateTo(viewName, _backStack);
    }

    public void NavigateBack()
    {
        if (CanNavigateBack && _backStack.TryPop(out string? viewName))
        {
            NavigateTo(viewName, _forwardStack);
        }
    }

    public void NavigateForward()
    {
        if (CanNavigateForward && _forwardStack.TryPop(out string? viewName))
        {
            NavigateTo(viewName, _backStack);
        }
    }

    private void NavigateTo(string viewName, ConcurrentStack<string> stack)
    {
        if (!Adapter!.CanNavigate)
        {
            throw new InvalidOperationException($"Region {Name} can't support navigation.");
        }

        var currentView = Adapter.CurrentView;
        if (currentView is not null)
        {
            stack.Push(currentView.GetType().Name);
        }
        Adapter.NavigateTo(viewName);

        foreach (var view in Adapter.Views.OfType<INavigationView>())
        {
            view.IsActive = false;
        }

        if (Adapter.CurrentView is INavigationView navigationView)
        {
            navigationView.IsActive = true;
        }
    }
}