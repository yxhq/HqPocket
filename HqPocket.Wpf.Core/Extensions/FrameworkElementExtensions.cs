using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
// ReSharper disable CheckNamespace

namespace System.Windows;

public static class FrameworkElementExtensions
{
    public static T? TryGetSelfOrParent<T>(this FrameworkElement? child) where T : FrameworkElement
    {
        return child switch
        {
            null => null,
            T t => t,
            _ => TryGetSelfOrParent<T>(child.Parent as FrameworkElement)
        };
    }

    public static Window? GetOwnerWindow(this FrameworkElement ownerElement)
    {
        var application = Application.Current;
        if (application?.MainWindow is null) return null;

        var active = application.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
        active ??= (PresentationSource.FromVisual(application.MainWindow) is null ? null : application.MainWindow);
        return Equals(active, ownerElement) ? null : active;
    }

    /// <summary>
    /// Retrieves all the logical children of a framework element using a 
    /// breadth-first search.  A visual element is assumed to be a logical 
    /// child of another visual element if they are in the same namescope.
    /// For performance reasons this method manually manages the queue 
    /// instead of using recursion.
    /// </summary>
    /// <param name="parent">The parent framework element.</param>
    /// <returns>The logical children of the framework element.</returns>
    public static IEnumerable<FrameworkElement> GetLogicalChildrenBreadthFirst(this FrameworkElement parent)
    {
        Debug.Assert(parent is not null, "The parent cannot be null.");

        Queue<FrameworkElement> queue =
            new(parent.GetVisualChildren().OfType<FrameworkElement>());

        while (queue.Count > 0)
        {
            FrameworkElement element = queue.Dequeue();
            yield return element;

            foreach (FrameworkElement visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
            {
                queue.Enqueue(visualChild);
            }
        }
    }

    public static Type? ResolveBindingSourceType(this FrameworkElement frameworkElement, string bindingPath)
    {
        FrameworkElement? element = frameworkElement;
        Type? sourceType = null;
        string sourceName = "";

        while (element is not null && sourceType is null)
        {
            sourceType = element.DataContext?.GetType();
            if (sourceType is null && element.GetBindingExpression(FrameworkElement.DataContextProperty) is BindingExpression expression)
            {
                sourceType = expression.ResolvedSource?.GetType();
                if (string.IsNullOrWhiteSpace(sourceName))
                {
                    sourceName = expression.ParentBinding.Path.Path;
                }
            }

            if (sourceType is not null)
            {
                if (sourceType.GetProperty(bindingPath) is null)
                {
                    sourceType = sourceType.GetProperty(sourceName)?.PropertyType;
                }
            }
            element = LogicalTreeHelper.GetParent(element) as FrameworkElement;
        }
        return sourceType;
    }

    public static bool IsDataContextDataBound(this FrameworkElement element)
    {
        ArgumentNullException.ThrowIfNull(element);
        return element.GetBindingExpression(FrameworkElement.DataContextProperty) is not null;
    }
}