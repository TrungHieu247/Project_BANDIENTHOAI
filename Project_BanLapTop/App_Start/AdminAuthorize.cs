using Project_BanLapTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_BanLapTop.App_Start
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        public string idRole { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // 1. Check session: Đã đăng nhập => cho thực hiện filter
            // Ngược lại thì cho trở lại => trang đăng nhập

            // 2. Check quyền: có quyền thì => cho phép thực hiện filter
            // Ngược lại thì cho trở lại trang => Trang báo lỗi không có quyền truy cập
            tb_user userLogin = (tb_user)HttpContext.Current.Session["userLogin"];
            if (userLogin != null)
            {
                MydataDataContext data = new MydataDataContext();
                var count = data.tb_user_roles.Count(m => m.idUser == userLogin.Id && m.idRole == idRole);
                if (count > 0)
                {
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(
                            new
                            {
                                controller = "Error",
                                action = "Role",
                                area = "Admin",
                            }
                        ));
                }
                return;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            controller = "Account",
                            action = "Login",
                            area = "Admin",
                        }
                    ));
            }
        }
    }
}