using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.StateUser
{
    public class UserSearchState : IUserSearchState
    {
        private readonly Dictionary<long, string> _userSearchStates  = new ();

        public bool IsWaitingForSearch(long chatId)
        {
           return _userSearchStates.ContainsKey(chatId) && _userSearchStates[chatId] == "waiting_for_search";
        }
        public void SetSearchState(long chatId, string state)
        {
            _userSearchStates[chatId] = state;
        }
        public void ClearSearchState(long chatId)
        {
            _userSearchStates.Remove(chatId);
        }
    }
    
}
