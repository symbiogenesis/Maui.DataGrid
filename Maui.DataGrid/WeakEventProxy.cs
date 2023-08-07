namespace Maui.DataGrid;

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// An abstract base class for subscribing to an event via WeakReference.
/// See WeakNotifyCollectionChangedProxy below for sublcass usage.
/// </summary>
/// <typeparam name="TSource">The object type that has the event</typeparam>
/// <typeparam name="TEventHandler">The event handler type of the event</typeparam>
internal abstract class WeakEventProxy<TSource, TEventHandler>
    where TSource : class
    where TEventHandler : Delegate
{
    private WeakReference<TSource>? _source;
    private WeakReference<TEventHandler>? _handler;

    public bool TryGetSource([MaybeNullWhen(false)] out TSource source)
    {
        if (_source is not null && _source.TryGetTarget(out source))
        {
            return source is not null;
        }

        source = default;
        return false;
    }

    public bool TryGetHandler([MaybeNullWhen(false)] out TEventHandler handler)
    {
        if (_handler is not null && _handler.TryGetTarget(out handler))
        {
            return handler is not null;
        }

        handler = default;
        return false;
    }

    public virtual void Subscribe(TSource source, TEventHandler handler)
    {
        _source = new WeakReference<TSource>(source);
        _handler = new WeakReference<TEventHandler>(handler);
    }

    public virtual void Unsubscribe()
    {
        _source = null;
        _handler = null;
    }
}

internal class WeakNotifyCollectionChangedProxy : WeakEventProxy<INotifyCollectionChanged, NotifyCollectionChangedEventHandler>
{
    public override void Subscribe(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
    {
        if (TryGetSource(out var s))
        {
            s.CollectionChanged -= OnCollectionChanged;
        }

        source.CollectionChanged += OnCollectionChanged;
        base.Subscribe(source, handler);
    }

    public override void Unsubscribe()
    {
        if (TryGetSource(out var s))
        {
            s.CollectionChanged -= OnCollectionChanged;
        }
        base.Unsubscribe();
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (TryGetHandler(out var handler))
        {
            handler(sender, e);
        }
        else
        {
            Unsubscribe();
        }
    }
}

internal class WeakSelectionChangedProxy : WeakEventProxy<SelectableItemsView, EventHandler<SelectionChangedEventArgs>>
{
    public override void Subscribe(SelectableItemsView source, EventHandler<SelectionChangedEventArgs> handler)
    {
        if (TryGetSource(out var s))
        {
            s.SelectionChanged -= OnSelectionChanged;
        }

        source.SelectionChanged += OnSelectionChanged;
        base.Subscribe(source, handler);
    }

    public override void Unsubscribe()
    {
        if (TryGetSource(out var s))
        {
            s.SelectionChanged -= OnSelectionChanged;
        }
        base.Unsubscribe();
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (TryGetHandler(out var handler))
        {
            handler(sender, e);
        }
        else
        {
            Unsubscribe();
        }
    }
}

internal class WeakRefreshingProxy : WeakEventProxy<RefreshView, EventHandler>
{
    public override void Subscribe(RefreshView source, EventHandler handler)
    {
        if (TryGetSource(out var s))
        {
            s.Refreshing -= OnRefreshing;
        }

        source.Refreshing += OnRefreshing;
        base.Subscribe(source, handler);
    }

    public override void Unsubscribe()
    {
        if (TryGetSource(out var s))
        {
            s.Refreshing -= OnRefreshing;
        }
        base.Unsubscribe();
    }

    private void OnRefreshing(object? sender, EventArgs e)
    {
        if (TryGetHandler(out var handler))
        {
            handler(sender, e);
        }
        else
        {
            Unsubscribe();
        }
    }
}
