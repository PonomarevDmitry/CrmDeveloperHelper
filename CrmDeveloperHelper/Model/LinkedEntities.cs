namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class LinkedEntities<T>
    {
        public T Entity1 { get; private set; }

        public T Entity2 { get; private set; }

        public LinkedEntities(T entity1, T entity2)
        {
            this.Entity1 = entity1;
            this.Entity2 = entity2;
        }
    }
}