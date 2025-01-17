using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.StateUser
{
    public interface IUserSearchState
    {
        bool IsWaitingForSearch(long chatId);
        void SetSearchState(long chatId, string state);
        void ClearSearchState(long chatId);
    }
}
