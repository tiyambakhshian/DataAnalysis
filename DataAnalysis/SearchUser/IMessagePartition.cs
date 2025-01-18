using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.SearchUser
{
    public interface IMessagePartition
    {
        IEnumerable<string> PartitionMessage(string message, int maxLength);
    }
}
