using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;

namespace ComCorpAssessment.Configs
{
    static class Utilities
    {
        // Characters to be removed at the begining and the end of the words
        public static readonly char[] s_charsToTrim = { '*', ' ', '\'', '.', '?', '!', '\"', ',', ':', '[', ']', '#' };
        internal static readonly char[] s_linesSpliter = {' '};
    }
}
