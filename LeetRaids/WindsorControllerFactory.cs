using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Castle.Core;
using Castle.Facilities;
using Castle.MicroKernel;
using Castle.Windsor;
using Castle.Windsor.Configuration;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;
using System.Reflection;


public class WindsorControllerFactory : DefaultControllerFactory
{
    WindsorContainer container;

    public WindsorControllerFactory()
    {
        container = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));

        var controllerTypes = from type in Assembly.GetExecutingAssembly().GetTypes()
                              where typeof(IController).IsAssignableFrom(type)
                              select type;

        foreach (var controllerType in controllerTypes)
        {
            container.AddComponentLifeStyle(controllerType.FullName, controllerType, LifestyleType.Transient);
        }
    }

    protected override IController GetControllerInstance(Type controllerType)
    {
        if (controllerType != null)
        {
            return (IController)container.Resolve(controllerType);
        }

        return null;
    }

}

