using System.Collections.Generic;

namespace Shimakaze.Struct.Csf
{
    public sealed class CsfClass : List<CsfLabel>
    {
        private string name;

        /// <summary>
        /// 类型名
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value.ToUpper();
        }
        private CsfClass() { }

        public static CsfClass Create(string name) => new CsfClass { Name = name };
        public static CsfClass Create(string name, IEnumerable<CsfLabel> csfValues)
        {
            var tmp = Create(name);
            tmp.AddRange(csfValues);
            return tmp;
        }

        public static implicit operator CsfClass(KeyValuePair<string, IEnumerable<CsfLabel>> keyValuePair) => Create(keyValuePair.Key, keyValuePair.Value);

        public static implicit operator KeyValuePair<string, IEnumerable<CsfLabel>>(CsfClass csfClass) => new KeyValuePair<string, IEnumerable<CsfLabel>>(csfClass.Name, csfClass);
    }
}
