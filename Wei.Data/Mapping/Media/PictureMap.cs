using Wei.Core.Domain.Media;

namespace Wei.Data.Mapping.Media
{
    public class PictureMap : WeiEntityTypeConfiguration<Picture>
    {
        public PictureMap()
        {
            this.ToTable("F_Picture");
            this.HasKey(p => p.Id);
            this.Property(p => p.ImgUrl).HasMaxLength(500);
            this.Property(p => p.PictureBinary).IsMaxLength();
            this.Property(p => p.MimeType).IsRequired().HasMaxLength(40);
        }
    }
}
