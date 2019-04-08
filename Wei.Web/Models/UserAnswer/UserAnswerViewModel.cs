using System;

namespace Wei.Web.Models.UserAnswer
{
    public class UserAnswerViewModel
    {
        public int id { get; set; }
        public int questionbank_id { get; set; }
        public int user_id { get; set; }
        public string nickname { get; set; }
        public DateTime begintime { get; set; }
        public DateTime? completedtime { get; set; }
        public string questionbank { get; set; }
        public int questionbanktype { get; set; }
        public string questionbanktypename { get; set; }
    }
}