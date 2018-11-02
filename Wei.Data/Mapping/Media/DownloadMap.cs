using Wei.Core.Domain.Media;

namespace Wei.Data.Mapping.Media
{
    public class DownloadMap : WeiEntityTypeConfiguration<Download>
    {
        public DownloadMap()
        {
            this.ToTable("F_Download");
            this.HasKey(p => p.Id);
            this.Property(p => p.DownloadBinary).IsMaxLength();
        }
    }
}
