using Masterly.DependencyInjection.Abstraction;

namespace Masterly.Autofac.DependenciesScanner.UnitTests
{
    public interface IDependencyA : IScopedDependency { }
    public interface IDependencyB : IDependencyA { }
    public interface IDependencyC : ISingletonDependency { }


    public abstract class AbstractDependencyClassA : IDependencyB { }
    public abstract class AbstractDependencyClassB : ITransientDependency { }

    public class DependencyA : IDependencyA { }
    public class DependencyB : AbstractDependencyClassA { }
    public class DependencyC : AbstractDependencyClassB { }
    public class DependencyD : IDependencyC { }

}
