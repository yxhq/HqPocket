//using System;
//using System.Collections.Concurrent;

//namespace HqPocket.Wpf.Helpers
//{
//    public static class ViewModelHelper
//    {
//        private static readonly ConcurrentDictionary<Type, object> _viewFactory = new();

//        public static void SetViewForViewModel(Type viewModelType, object view)
//        {
//            _viewFactory[viewModelType] = view;
//        }

//        public static object? TryGetViewForViewModel(Type viewModelType)
//        {
//            return _viewFactory.TryGetValue(viewModelType, out object? view) ? view : null;
//        }

//        public static object? TryGetViewForViewModel<TViewModel>()
//        {
//            return TryGetViewForViewModel(typeof(TViewModel));
//        }

//        public static TView? TryGetViewForViewModel<TViewModel, TView>()
//        {
//            return (TView?)TryGetViewForViewModel(typeof(TViewModel));
//        }

//        public static object? TryGetViewForViewModel(object viewModel)
//        {
//            return _viewFactory.TryGetValue(viewModel.GetType(), out object? view) ? view : null;
//        }
//    }
//}
