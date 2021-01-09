using System.Collections.Generic;


namespace Shimakaze.Struct.Csf
{
    /// <summary>
    /// CSF文件的标签结构
    /// </summary>
    public struct CsfLabel
    {
        public string Label;
        public List<CsfValue> Values;
    }
}
