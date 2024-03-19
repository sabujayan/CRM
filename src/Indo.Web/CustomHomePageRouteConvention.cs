using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indo.Web
{
    public class CustomHomePageRouteConvention : IPageRouteModelConvention
    {
        public void Apply(PageRouteModel model)
        {
            if (model.RelativePath == "/Pages/Index.cshtml")
            {
                var currentHomePage = model.Selectors.Single(s => s.AttributeRouteModel.Template == string.Empty);
                model.Selectors.Remove(currentHomePage);
            }

            if (model.RelativePath == "/Pages/Dashboard/Index.cshtml")
            {
                model.Selectors.Add(new SelectorModel()
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = string.Empty
                    }
                });
            }
        }
    }
}
