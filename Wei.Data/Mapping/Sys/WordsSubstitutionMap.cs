using Wei.Core.Domain.Sys;

namespace Wei.Data.Mapping.Sys
{
    public class WordsSubstitutionMap: WeiEntityTypeConfiguration<WordsSubstitution>
    {
        public WordsSubstitutionMap()
        {
            this.ToTable("S_WordsSubstitution");

            this.HasKey(x => x.Id);
            this.HasOptional(x => x.QuestionBank)
                .WithMany()
                .HasForeignKey(x => x.QuestionBankId);

        }
    }
}
