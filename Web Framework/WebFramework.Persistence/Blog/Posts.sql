

DECLARE @Title nvarchar(max), @ShortDescription nvarchar(max), @Content nvarchar(max), 
	@UrlSlug nvarchar(250), @PostedOn datetime, @Category_Id int, @Tags nvarchar(1000)

SET @Title = 'Deploy NuGet Packages During CI Build with TeamCity'
SET @ShortDescription = 'One great benefit of having a Continuous Integration server like TeamCity building your code is that you can hook into the build process to have it also handle tedious or time-consuming tasks for you, such as running all of your unit tests, code coverage analyses, etc...'
SET @Content = '
    <h4>Introduction</h4>
    <p>One great benefit of having a Continuous Integration server like TeamCity (http://www.jetbrains.com/teamcity/) building your code is that you can hook into the build process to have it also handle tedious or time-consuming tasks for you, such as running all of your unit tests, code coverage analyses, etc.</p>
    <p>Now that NuGet (http://nuget.org/) has arrived and simplified package management in .NET, wouldn’t it be nice to have your CI server build an updated NuGet package for you along with every build?  An why not go one step further and have your build process automatically push the newest version of your package to the central NuGet server (or even your own hosted NuGet server)?</p>
    <p>It turns out this is actually very easy to do – let’s see how.</p>
    <h4>Super-Quick NuGet Package Introduction</h4>
    <p>If you would like to familiarize yourself with what NuGet is and how to use it, take a look at the project homepage at http://nuget.codeplex.com/. </p>
    <p>For this post it’s really only necessary to know that a NuGet package starts out as a specification file (*.nupec) and usually some deliverable content (Dlls, script files, etc).  Once you have a specification file, you use the NuGet.exe command line utility to turn it into a package (*.nupkg) that can then be uploaded to the masses via http://nuget.org/.  [Full details are available at http://nuget.codeplex.com/documentation?title=Creating%20a%20Package]. </p>
    <h4>Building and Deploying Your Package</h4>
    <p>You’ll need to have your *.nuspec file checked into source control, and I also find it useful to check in the nuget.exe command-line utility as well (although this is not necessary as long as it is on your build server).  I tend to put my specifications in a /Build/NuGet folder, so it’s outside of the main /src/ folder.</p>
    <p>Note: The following steps are TeamCity specific, but any build process with the ability to run a batch file should be able to do the same thing.</p>
    <div>1. Open up your project’s build configuration and select #3, “Build Steps.”</div>
    <div><img src="https://aspblogs.blob.core.windows.net/media/srkirkland/Media/image_59A2ABDE.png" alt="Build Steps" /></div>
    <div>2. Click “Add Build Step.”</div>
    <div>3. Under Runner Type choose “Command Line,” and then under Run choose “Custom Script.”  You can optionally specify a working directory to have your script run in that directory, which I find to be quite helpful.</div>
    <div><img src="https://aspblogs.blob.core.windows.net/media/srkirkland/Media/image_7C173A99.png" alt="Command Line" /></div>
    <div>4.  Now you are ready to write your custom script, which should be able to perform the following steps: (1) cleanup old *.nupack files, (2) create your newest package with the correct version number, and (3) push that package up to nuget.org or another nuget server.</div>
    <p>My script is as follows:</p>
    <div class="csharpcode">
        <pre>del *.nupkg</pre>
        <pre>NuGet.exe pack Project\Project.nuspec -Version %system.build.number%</pre>
        <pre>forfiles /m *.nupkg /c <span class="str">"cmd /c NuGet.exe push @@FILE &lt;your-key&gt;"</span></pre>
    </div>
    <p>5. That’s all there is to it, just replace <your-key> with your access key from nuget.org (look under MyAccount)</p>
    <div><img src="https://aspblogs.blob.core.windows.net/media/srkirkland/Media/image_thumb_666CDC2F.png" alt="Command Line" /></div>
    <p>Script Breakdown:</p>
    <p>On line two notice how I use TeamCity’s %system.build.number% to inject the build number into the generated package.  This is very important because NuGet pays close attention to your package version number (as David Ebbo describes in detail in his NuGet versioning blog series http://blog.davidebbo.com/2011/01/nuget-versioning-part-3-unification-via.html). </p>
    <p>The third line is pretty fun—basically it is saying for every file that ends in *.nupkg, call ‘NuGet.exe push <filename> <your-key>’.  I really like this approach, and it could even be adapted to build and push multiple NuGet packages during every build run.</p>
    <h4>Conclusion</h4>
    <p>Overall there wasn’t too much work to do, we just created a command line script build runner and added a few lines of code to automatically build and push versioned NuGet packages.</p>
    <p>Now that I’m using NuGet a lot more, with one OSS project on nuget.org (http://dataannotationsextensions.org/) and several hosted on my own internal company NuGet server, I find this automatic build and deploy process the perfect way to keep my packages up to date.  </p>
'
SET @UrlSlug = 'deploy-nuget-packages-during-ci-build-with-teamcity'
SET @PostedOn = '2016-01-21'
SET @Category_Id = 13
SET @Tags = 'TFS,Visual Studio'


EXEC [dbo].[CreateBlogPost] @Title=@Title, @ShortDescription=@ShortDescription, @Content=@Content, @UrlSlug=@UrlSlug, @PostedOn=@PostedOn, @Category_Id=@Category_Id, @Tags=@Tags


SET @Title = 'Guarding against CSRF Attacks in ASP.NET MVC'
SET @ShortDescription = 'Alongside XSS (Cross Site Scripting) and SQL Injection, Cross-site Request Forgery (CSRF) attacks represent the three most common and dangerous vulnerabilities to common web applications today. CSRF attacks are probably the least well known but they are relatively easy to exploit and extremely and increasingly dangerous...'
SET @Content = N'
    <h4>Introduction</h4>
    <p>Alongside XSS (Cross Site Scripting) and SQL Injection, Cross-site Request Forgery (CSRF) attacks represent the three most common and dangerous vulnerabilities to common web applications today. CSRF attacks are probably the least well known but they are relatively easy to exploit and extremely and increasingly dangerous.</p>
    <p>The recognized solution for preventing CSRF attacks is to put a user-specific token as a hidden field inside your forms, then check that the right value was submitted. It''s best to use a random value which you’ve stored in the visitor’s Session collection or into a Cookie (so an attacker can''t guess the value). </p>
    <h4>ASP.NET MVC to the rescue</h4>
    <p>ASP.NET MVC provides an HTMLHelper called AntiForgeryToken(). When you call @@Html.AntiForgeryToken() in a form on your page you will get a hidden input and a Cookie with a random string assigned.</p>
    <p>Next, on your target Action you need to include [ValidateAntiForgeryToken], which handles the verification that the correct token was supplied.</p>
    <h4>Good, but we can do better?</h4>
    <p>Using the AntiForgeryToken is actually quite an elegant solution, but adding [ValidateAntiForgeryToken] on all of your POST methods is not very DRY, and worse can be easily forgotten.</p>
    <p>Let''s see if we can make this easier on the program but moving from an "Opt-In" model of protection to an "Opt-Out" model. </p>
    <h4>Using AntiForgeryToken by default</h4>
    <p>In order to mandate the use of the AntiForgeryToken, we''re going to create an ActionFilterAttribute which will do the anti-forgery validation on every POST request.</p>
    <p>First, we need to create a way to Opt-Out of this behavior, so let''s create a quick action filter called BypassAntiForgeryToken:</p>
    <pre class="CSharp">[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
public class BypassAntiForgeryTokenAttribute : ActionFilterAttribute { }</pre>
    <p>Now we are ready to implement the main action filter which will force anti forgery validation on all post actions within any class it is defined on:</p>
    <p>
        <pre class="CSharp">[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class UseAntiForgeryTokenOnPostByDefault : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (ShouldValidateAntiForgeryTokenManually(filterContext))
        {
            var authorizationContext = new AuthorizationContext(filterContext.Controller.ControllerContext, filterContext.ActionDescriptor);

            //Use the authorization of the anti forgery token, 
            //which can''t be inhereted from because it is sealed
            <b>new ValidateAntiForgeryTokenAttribute().OnAuthorization(authorizationContext);</b>
        }
 
        base.OnActionExecuting(filterContext);
    }
 
    // We should validate the anti forgery token manually if the following criteria are met:
    // 1. The http method must be POST
    // 2. There is not an existing [ValidateAntiForgeryToken] attribute on the action
    // 3. There is no [BypassAntiForgeryToken] attribute on the action
    private static bool ShouldValidateAntiForgeryTokenManually(ActionExecutingContext filterContext)
    {
        var httpMethod = filterContext.HttpContext.Request.HttpMethod;
 
        //1. The http method must be POST
        if (httpMethod != "POST") return false;
 
        // 2. There is not an existing anti forgery token attribute on the action
        var antiForgeryAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), false);
 
        if (antiForgeryAttributes.Length > 0) return false;
 
        <b>// 3. There is no [BypassAntiForgeryToken] attribute on the action</b>
        var ignoreAntiForgeryAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(BypassAntiForgeryTokenAttribute), false);
 
        if (ignoreAntiForgeryAttributes.Length > 0) return false;
 
        return true;
    }
}</pre>
    </p>
    <p>The code above is pretty straight forward -- first we check to make sure this is a POST request, then we make sure there aren''t any overriding *AntiForgeryTokenAttributes on the action being executed. If we have a candidate then we call the ValidateAntiForgeryTokenAttribute class directly and execute OnAuthorization() on the current authorization context.</p>
    <p>Now on our base controller, you could use this new attribute to start protecting your site from CSRF vulnerabilities.</p>
    <p><pre class="CSharp"><b>[UseAntiForgeryTokenOnPostByDefault]</b>
public class ApplicationController : System.Web.Mvc.Controller { }
 
//Then for all of your controllers
public class HomeController : ApplicationController {}</pre></p>
    <h4>Conclusion</h4>
    <p>If your base controller has the new default anti-forgery token attribute on it, when you don''t use @@Html.AntiForgeryToken() in a form (or of course when an attacker doesn''t supply one), the POST action will throw the descriptive error message "A required anti-forgery token was not supplied or was invalid". Attack foiled!</p>
    <p>In summary, I think having an anti-CSRF policy by default is an effective way to protect your websites, and it turns out it is pretty easy to accomplish as well.</p>
    <p>Stay secured!</p>
'
SET @UrlSlug = 'guarding-against-csrf-attacks-asp-dot-net-mvc'
SET @PostedOn = '2016-02-16'
SET @Category_Id = 11
SET @Tags = 'ASP.NET,ASP.NET MVC,Validation'


EXEC [dbo].[CreateBlogPost] @Title=@Title, @ShortDescription=@ShortDescription, @Content=@Content, @UrlSlug=@UrlSlug, @PostedOn=@PostedOn, @Category_Id=@Category_Id, @Tags=@Tags




SET @Title = 'Authorizing Access via Attributes in ASP.NET MVC Without Magic Strings'
SET @ShortDescription = 'When creating the custom authorize attribute I inherit from AuthorizeAttribute since it already contains most of the logic I need.  All I need to do is set the Roles property in the constructor to a comma delimited list of the authorized roles, and the authorize attribute base class will take care of the rest...'
SET @Content = N'
    <h4>Introduction</h4>
    <p>Recently I developed a strategy which I think works well for authorizing access to user groups (Roles) without using the string names of those groups.</p>
    <p>The problem I am trying to avoid is doing something like [Authorize(Roles=”AdminRole”)] on a controller or action since I know the role names can change & one typo can mess everything up.</p>
    <h4>Role Names</h4>
    <p>So first of all I usually have a static class which has the names & aliases for all roles in case they change:</p>
    <p><pre class="CSharp">public static class RoleNames
{
    public static readonly string Supervisor = "Supervisor";
    public static readonly string Admin = "StateOffice";
    public static readonly string ProjectAdmin = "ProjectAdmin";
    public static readonly string DelegateSupervisor = "Delegate";
}</pre></p>
    <p>This is pretty standard for me, but unfortunately I can’t just do [Authorize(Roles=RolesNames.Admin)] because attributes requires constant expressions.  So as a solution I came up with the idea of creating a custom attribute which will tightly control access based on specific role criteria.</p>
    <h4>Creating a Custom Authorize Attribute</h4>
    <p>When creating the custom authorize attribute I inherit from AuthorizeAttribute since it already contains most of the logic I need.  All I need to do is set the Roles property in the constructor to a comma delimited list of the authorized roles, and the authorize attribute base class will take care of the rest.</p>
    <p>For example – to restrict access to just the admin role:   </p>
    <pre class="CSharp">[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminOnlyAttribute : AuthorizeAttribute
{
    public AdminOnlyAttribute()
    {
        Roles = RoleNames.Admin;
    }
}</pre>
    <p>Or if you want to include the project admins as well:</p>
    <p><pre class="CSharp">[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminOnlyAttribute : AuthorizeAttribute
{
    public AdminOnlyAttribute()
    {
        var authorizedRoles = new[] {RoleNames.Admin, RoleNames.ProjectAdmin};
 
        Roles = string.Join(",", authorizedRoles);
    }
}</pre></p>
    <h4>Usage</h4>
    <p>Just apply the attribute either at controller-level or at action:</p>
    <p><pre class="CSharp">public class AdminController : Controller
{
    [AdminOnly]
    public ActionResult AdminOnlyAction()
    {
        return View();
    }
}</pre></p>
    <p>Goodluck!</p>
'
SET @UrlSlug = 'authorizing-access-via-attributes-asp-dot-net-mvc-without-magic-strings'
SET @PostedOn = '2016-02-28'
SET @Category_Id = 5
SET @Tags = 'ASP.NET,ASP.NET MVC,Validation'


EXEC [dbo].[CreateBlogPost] @Title=@Title, @ShortDescription=@ShortDescription, @Content=@Content, @UrlSlug=@UrlSlug, @PostedOn=@PostedOn, @Category_Id=@Category_Id, @Tags=@Tags

