using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Project_BanLapTop.Areas.Admin.Data
{
    public static class MyLibrary
    {
        private static MydataDataContext data = new MydataDataContext();
        public static string GetFullnameByUserLogin()
        {
            var user = HttpContext.Current.Session["userlogin"] as tb_user;
            return user.Fullname;
        }

        public static int GetIDByUserLogin()
        {
            var user = HttpContext.Current.Session["userlogin"] as tb_user;
            return user.Id;
        }

        public static bool CheckUserExists(string username, string email)
        {
            var check = data.tb_users.Any(m => m.Username == username || m.Email == email);
            if (check == true)
                return true;
            return false;
        }
    }
}