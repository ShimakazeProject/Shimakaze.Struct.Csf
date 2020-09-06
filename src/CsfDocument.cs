using System;
using System.Collections.Generic;
using System.Text;

namespace Shimakaze.Struct.Csf
{
    public class CsfDocument : List<CsfLabel>
    {
        public CsfHead Head;
    }
}
