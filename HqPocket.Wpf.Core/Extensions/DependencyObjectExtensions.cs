using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
// ReSharper disable CheckNamespace

namespace System.Windows;

public static class DependencyObjectExtensions
{
    public static T? TryFindParent<T>(this DependencyObject? child) where T : DependencyObject
    {
        DependencyObject? parent = GetParentObject(child);

        return parent switch
        {
            null => null,
            T t => t,
            _ => TryFindParent<T>(parent)
        };
    }

    public static DependencyObject? GetParentObject(this DependencyObject? child)
    {
        if (child is null) return null;
        // handle content elements separately
        if (child is ContentElement contentElement)
        {
            DependencyObject parent = ContentOperations.GetParent(contentElement);
            return parent ?? (contentElement is FrameworkContentElement fce ? fce.Parent : null);
        }
        var childParent = VisualTreeHelper.GetParent(child);
        if (childParent is not null)
        {
            return childParent;
        }
        // also try searching for parent in framework elements (such as DockPanel, etc)
        if (child is FrameworkElement frameworkElement)
        {
            DependencyObject parent = frameworkElement.Parent;
            if (parent is not null) return parent;
        }
        return null;
    }

    public static DependencyObject? GetVisualOrLogicalParent(this DependencyObject sourceElement)
    {
        return sourceElement switch
        {
            null => null,
            Visual _ => VisualTreeHelper.GetParent(sourceElement) ?? LogicalTreeHelper.GetParent(sourceElement),
            _ => LogicalTreeHelper.GetParent(sourceElement)
        };
    }

    public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject child)
    {
        var parent = VisualTreeHelper.GetParent(child);
        while (parent is not null)
        {
            yield return parent;
            parent = VisualTreeHelper.GetParent(parent);
        }
    }

    public static T? FindChild<T>(this DependencyObject? parent, string? childName = null) where T : DependencyObject
    {
        if (parent is null) return null;
        T? foundChild = null;

        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is not T tchild)
            {
                foundChild = FindChild<T>(child, childName);
                if (foundChild is not null) break;
            }
            else if (!string.IsNullOrEmpty(childName))
            {
                if (tchild is IFrameworkInputElement frameworkInputElement && frameworkInputElement.Name == childName)
                {
                    foundChild = tchild;
                    break;
                }
                else
                {
                    foundChild = FindChild<T>(tchild, childName);
                    if (foundChild is not null) break;
                }
            }
            else
            {
                foundChild = tchild;
                break;
            }
        }
        return foundChild;
    }

    public static IEnumerable<T> FindChildren<T>(this DependencyObject? source, bool forceUsingTheVisualTreeHelper = false) where T : DependencyObject
    {
        if (source is not null)
        {
            var childs = GetChildObjects(source, forceUsingTheVisualTreeHelper);
            foreach (DependencyObject child in childs)
            {
                if (child is T t)
                {
                    yield return t;
                }
                foreach (T descendant in FindChildren<T>(child, forceUsingTheVisualTreeHelper))
                {
                    yield return descendant;
                }
            }
        }
    }

    public static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject? parent, bool forceUsingTheVisualTreeHelper = false)
    {
        if (parent is null) yield break;
        if (!forceUsingTheVisualTreeHelper && (parent is ContentElement || parent is FrameworkElement))
        {
            foreach (object obj in LogicalTreeHelper.GetChildren(parent))
            {
                if (obj is DependencyObject depObj)
                {
                    yield return depObj;
                }
            }
        }
        else if (parent is Visual || parent is Visual3D)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                yield return VisualTreeHelper.GetChild(parent, i);
            }
        }
    }

    public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
    {
        return parent.GetChildObjects(true);
    }
}