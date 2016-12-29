using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    interface IBuildOutputString<T>
    {
        string BuildString(List<T> filteredList);
        string Extension();
    }
}
