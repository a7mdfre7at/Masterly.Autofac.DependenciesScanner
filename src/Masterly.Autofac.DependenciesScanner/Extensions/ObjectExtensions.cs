namespace Masterly.Autofac.DependenciesScanner
{
    internal static class ObjectExtensions
    {
        internal static T As<T>(this object obj) where T : class => (T)obj;
    }
}