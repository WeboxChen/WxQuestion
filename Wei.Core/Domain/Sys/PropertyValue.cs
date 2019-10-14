namespace Wei.Core.Domain.Sys
{
    public class PropertyValue: BaseEntity
    {
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }
        public string Text { get; set; }
        public int IsDef { get; set; }
        public int Sort { get; set; }
    }
}
