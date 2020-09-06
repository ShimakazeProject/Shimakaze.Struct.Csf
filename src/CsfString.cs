namespace Shimakaze.Struct.Csf
{
    public class CsfString
    {
        public bool IsWString = false;
        public string Content = string.Empty;
    }
    public class CsfWString : CsfString
    {
        public string Extra = string.Empty;
    }
}
