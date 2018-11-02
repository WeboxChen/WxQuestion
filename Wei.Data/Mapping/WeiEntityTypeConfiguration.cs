using System.Data.Entity.ModelConfiguration;
using Wei.Core;

namespace Wei.Data.Mapping
{
    public abstract class WeiEntityTypeConfiguration<T>
        : EntityTypeConfiguration<T> where T : BaseEntity
    {
        protected WeiEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {

        }
    }
}
