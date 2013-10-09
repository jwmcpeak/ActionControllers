using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActionControllers
{
    public class ActionControllerFactory : DefaultControllerFactory
    {
        protected readonly Dictionary<string, Type> TypeCache =
            new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            var action = requestContext.RouteData.Values["action"];
            var typeName = string.Format("{0}.{1}", controllerName, action);

            var controller = TryGetControllerFromCacheWithKeys(typeName, controllerName) // cache frist
                ?? GetControllerFromAppDomain(typeName) // appdomain second
                ?? base.CreateController(requestContext, controllerName); // fallback

            // cache; default factory caches but caching here will save us from future
            // assembly searches. Dupe cache data is smaller price to pay.
            if (!(controller is ActionController) && !TypeCache.ContainsKey(controllerName))
            {
                TypeCache[controllerName] = controller.GetType();
            }

            return controller;
        }

        protected virtual IController TryGetControllerFromCacheWithKeys(params string[] keys)
        {
            return keys.Where(k => TypeCache.ContainsKey(k))
                                  .Select(k => (IController)Activator.CreateInstance(TypeCache[k])).SingleOrDefault();
        }

        protected virtual IController GetControllerFromAppDomain(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var types in assemblies.Select(assembly => assembly.GetTypes()))
            {
                foreach (var type in types.Where(type => typeof(ActionController).IsAssignableFrom(type)))
                {
                    var ns = type.UnderlyingSystemType.FullName;

                    if (ns.EndsWith(typeName, true, CultureInfo.InvariantCulture))
                    {
                        TypeCache[typeName] = type;
                        return (IController)Activator.CreateInstance(type);
                    }
                }
            }

            return null;
        }
    }
}