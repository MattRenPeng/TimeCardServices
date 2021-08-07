using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCardServices.Domain;
using TimeCardServices.Model;
using TimeCardServices.Repository;

namespace TimeCardServices.Services
{
    public class TimeCardService : ITimeCardService
    {
        IRepository<TimeCard> Repository;
        public TimeCardService(IRepository<TimeCard> repository)
        {
            Repository = repository;
        }
        public int AddOneWeekData(TimeCardViewModel oneWeekData)
        {
            string JsonData = Newtonsoft.Json.JsonConvert.SerializeObject(oneWeekData);
            TimeCard weekObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TimeCard>(JsonData);
            return Repository.Insert(weekObject);
            //wekkObject.CreateDate = DateTime.Now;
            //wekkObject.UpdateDate = DateTime.Now;

        }
        public int DeleteOneWeekData(string userName,DateTime weekStart)
        {
            TimeCard weekObject = Repository.SearchFor(f => f.UserName == userName && f.WeekStart.Equals(weekStart)).FirstOrDefault();
            if (weekObject == null)
                return 0;
            else
            {
                return Repository.Delete(weekObject);
            }
        }
        public int DeleteTimeCardDataOfOneUser(string userName)
        {
            return Repository.Delete(f => f.UserName == userName);
        }

        public TimeCardViewModel GetOneWeekData(string userName, DateTime weekStart)
        {
            TimeCard weekObject = Repository.SearchFor(f => f.UserName == userName && f.WeekStart.Equals(weekStart)).FirstOrDefault();
            string JsonData = Newtonsoft.Json.JsonConvert.SerializeObject(weekObject);
            TimeCardViewModel weekViewmodelObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TimeCardViewModel>(JsonData);
            return weekViewmodelObject;
        }
    }
}
