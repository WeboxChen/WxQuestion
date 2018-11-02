using System;

namespace Wei.Core.Domain.Logging
{
    public class ActionRecord : BaseEntity
    {
        public int ObjId { get; set; }
        public string ModuleName { get; set; }
        public int Type { get; set; }
        public string ViewName { get; set; }
        public string ProcName { get; set; }
        public int Status { get; set; }
        public string Msg { get; set; }
        public DateTime OperateTime { get; set; }
        public string Operator { get; set; }
        public int OperatorId { get; set; }
        public string Source { get; set; }
        public string RequestParas { get; set; }
        public string Payload { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string Info3 { get; set; }
        public string Info4 { get; set; }
        public string Info5 { get; set; }
    }
}
