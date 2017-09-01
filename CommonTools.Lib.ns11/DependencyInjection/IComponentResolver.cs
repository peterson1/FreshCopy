namespace CommonTools.Lib.ns11.DependencyInjection
{
    public interface IComponentResolver
    {
        T Resolve<T>() where T : class;
    }
}
