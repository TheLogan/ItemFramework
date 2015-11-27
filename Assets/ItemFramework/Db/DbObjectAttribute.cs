using Newtonsoft.Json;
using System;

namespace ItemFramework.Db {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class DbObjectAttribute : Attribute {
        public string Name;

        private MemberSerialization _memberSerialization = MemberSerialization.OptIn;

        public MemberSerialization MemberSerialization {
            get { return _memberSerialization; }
            set { _memberSerialization = value; }
        }

        public DbObjectAttribute(string name) {
            this.Name = name;
        }
    }
}