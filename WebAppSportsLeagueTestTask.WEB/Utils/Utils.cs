using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppSportsLeagueTestTask.WEB.Models;

namespace WebAppSportsLeagueTestTask.WEB
{
    public class Utils
    {
        public static ApplicationUser GetCurrentAppUser()
        {
            return System.Web.HttpContext.Current
                .GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        }
    }
}