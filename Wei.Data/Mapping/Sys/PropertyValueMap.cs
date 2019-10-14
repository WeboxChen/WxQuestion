using Wei.Core.Domain.Sys;

namespace Wei.Data.Mapping.Sys
{
    public class PropertyValueMap: WeiEntityTypeConfiguration<PropertyValue>
    {
        public PropertyValueMap()
        {
            this.ToTable("S_PropertyValue");

            this.HasKey(x => x.Id);
            this.HasRequired(x => x.Property)
                .WithMany(x => x.PropertyValueList)
                .HasForeignKey(x => x.PropertyId);
        }
    }
}
