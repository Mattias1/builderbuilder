namespace VipLive.Test.Basics.TestHelpers
{
    public interface IAbstractBuilder<out T>
    {
        T Build();
    }

    public abstract class AbstractBuilder<T> : IAbstractBuilder<T> where T : new()
    {
        protected readonly T Item;

        protected AbstractBuilder() : this(new T()) { }

        protected AbstractBuilder(T item) => Item = item;

        public virtual T Build() => Item;
    }

    public interface IAbstractEntityBuilder<out T> : IAbstractBuilder<T>
    {
        T AutoBuild();
        T Persist(IDbContext context);
    }

    public abstract class AbstractEntityBuilder<T> : AbstractBuilder<T>, IAbstractEntityBuilder<T> where T : class, new()
    {
        protected AbstractEntityBuilder() : base() { }

        protected AbstractEntityBuilder(T item) : base(item) { }

        public abstract T AutoBuild();

        public T Persist(IDbContext context)
        {
            SaveToDatabase(context);
            return Build();
        }

        protected virtual void SaveToDatabase(IDbContext context)
        {
            context.SaveToDatabase(Item);
        }
    }
}
