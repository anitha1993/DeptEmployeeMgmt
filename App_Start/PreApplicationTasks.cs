using System.Web;
using System.Web.Security;

[assembly: PreApplicationStartMethod(typeof(PreApplicationTasks), "Initializer")]

public static class PreApplicationTasks
{
    public static void Initializer()
    {
        Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility
            .RegisterModule(typeof(UserInitializationModule));
    }
}

public class UserInitializationModule : IHttpModule
{
    private static bool initialized;
    private static object lockObject = new object();

    private const string _username = "Owner";
    private const string _password = "p@ssword123";
    private const string _role = "Admin";

    void IHttpModule.Init(HttpApplication context)
    {
        lock (lockObject)
        {
            if (!initialized)
            {
                new InitializeSimpleMembershipAttribute().OnActionExecuting(null);

                if (!WebSecurity.UserExists(_username))
                    WebSecurity.CreateUserAndAccount(_username, _password);

                if (!Roles.RoleExists(_role))
                    Roles.CreateRole(_role);

                if (!Roles.IsUserInRole(_username, _role))
                    Roles.AddUserToRole(_username, _role);
            }
            initialized = true;
        }
    }

    void IHttpModule.Dispose() { }
}