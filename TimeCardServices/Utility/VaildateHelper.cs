using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCardServices.Domain;

namespace TimeCardServices.Utility
{
    public class VaildateHelper
    {
        public static ObjectResult ReturnMessageObjectResult(ModelStateDictionary ModelState)
        {
            List<MessageViewModel> messages = new List<MessageViewModel>();
            foreach (string key in ModelState.Keys)
            {
                if (ModelState[key].ValidationState == ModelValidationState.Invalid)
                {
                    foreach (ModelError errorItem in ModelState[key].Errors)
                    {
                        messages.Add(new MessageViewModel() { Field = key, Message = errorItem.ErrorMessage });
                    }
                }
            }

            return new NotFoundObjectResult(messages);
        }
    }
}
