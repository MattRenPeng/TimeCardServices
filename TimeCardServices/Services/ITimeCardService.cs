using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCardServices.Domain;

namespace TimeCardServices.Services
{
    public interface ITimeCardService 
    {

        public TimeCardViewModel GetOneWeekData(string userName,DateTime weekStart);

        public int AddOneWeekData(TimeCardViewModel oneWeekData);

        public int DeleteTimeCardDataOfOneUser(string userName);
        public int DeleteOneWeekData(string userName, DateTime weekStart);

    }
}
