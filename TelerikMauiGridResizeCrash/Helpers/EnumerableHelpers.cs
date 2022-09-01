using System.Collections.ObjectModel;

namespace TelerikMauiGridResizeCrash.Helpers
{
    public static class EnumerableHelpers
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null || action == null)
                return;

            foreach (var item in source)
            {
                action(item);
            }
        }

        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> source)
        {
            var result = new ObservableCollection<T>();

            if (source == null)
                return result;

            foreach (var item in source)
                result.Add(item);

            return result;
        }
    }
}
