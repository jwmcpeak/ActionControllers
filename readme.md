# ActionControllers

Description coming soon.

## Instructions

1. Create a folder in your `Controllers` directory (although technically it can be anywhere) to represent your "controller". Example: Controllers\Home
2. Create a class named for your desired action (eg: Index) and inherit `ActionController`.
3. The methods in the action class match the HTTP method of the request. For example, GET requests correspond with the `Get()` method, POST with `Post()`, etc.
4. Configure your ASP.NET MVC application to use the `ActionControllerFactory`:


	ControllerBuilder.Current.SetControllerFactory(new ActionControllerFactory);

## Example Action Controller

    namespace MyApp.Controllers.Home
    {
        public class Index : ActionController
        {
			public ActionResult Get()
			{
				return View(); // returns view Views\Home\Index.cshtml
			}

			[ValidateAntiForgeryToken]
			public ActionResult Post()
			{
				return RedirectToAction("Index"); // goes to Get()
			}
        }
    }