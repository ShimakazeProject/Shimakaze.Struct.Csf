namespace Shimakaze.Struct.Csf
{
    public struct CsfHead
    {
        #region Constant
        // 标准CSF文件标识符
        public static readonly byte[] CSF_FLAG = { 32, 70, 83, 67, };
        /// <summary>
        /// Csf Version Constant
        /// </summary>
        public static class Versions
        {
            public const int VERSION_2 = 2;
            public const int VERSION_3 = 3;
        }
        /// <summary>
        /// Csf Languages Constant
        /// </summary>
        public static class Languages
        {
            /// <summary>
            /// Natural language for Ares
            /// </summary>
            public const int ares_auto = -1;
            public const int de = 2;
            public const int en_UK = 1;
            public const int en_US = 0;
            public const int es = 4;
            public const int fr = 3;
            public const int it = 5;
            public const int ja = 6;
            /// <summary>
            /// Unknown
            /// </summary>
            public const int jabberwockie = 7;
            public const int ko = 8;

            public const int zh = 9;
        }
        #endregion

        public int Version;
        public int LabelCount;
        public int StringCount;
        public int Unknown;
        public int Language;


        public static readonly string[] LanguageList = new[] {
            "en_US",
            "en_UK",
            "de",
            "fr",
            "es",
            "it",
            "jp",
            "Jabberwockie",
            "kr",
            "zh"
        };
    }
}
