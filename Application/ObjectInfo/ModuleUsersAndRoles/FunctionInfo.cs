﻿namespace ObjectInfos
{
    public sealed class FunctionInfo
    {
        public int Stt { get; set; }
        public int Id { get; set; }
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Country { get; set; }

        public int FunctionType { get; set; }
        public string FunctionTypeDisplayName { get; set; }
        public string HrefGet { get; set; }
        public string HrefPost { get; set; }
        public int Position { get; set; }
        public int ParentId { get; set; }
        public int Lev { get; set; }
        public int MenuId { get; set; }
        public int FunctionAddedToGroup { get; set; }
        public string FunctionName_Eng { get; set; }
        public string DisplayName_Eng { get; set; }
        public bool IsFunctionAddedToGroup()
        {
            return this.FunctionAddedToGroup == 1;
        }
    }
}
