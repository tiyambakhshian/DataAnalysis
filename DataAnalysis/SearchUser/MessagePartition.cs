using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.SearchUser
{
    public class MessagePartition : IMessagePartition
    {
        
        public IEnumerable<string> PartitionMessage(string message, int maxLength)
        {
            while (message.Length > maxLength)
            {
                yield return message.Substring(0, maxLength);
                message = message.Substring(maxLength);
            }
            if(message.Length>0)
            {
                yield return message;
            }
        }
    }
}
