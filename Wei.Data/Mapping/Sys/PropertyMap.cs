using Wei.Core.Domain.Sys;

namespace Wei.Data.Mapping.Sys
{
    public class PropertyMap: WeiEntityTypeConfiguration<Property>
    {
        public PropertyMap()
        {
            this.ToTable("S_Property");

            this.HasKey(x => x.Id);
        }
    }
}
