using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using toofz.NecroDancer.Web.Api.Areas.HelpPage.ModelDescriptions;
using toofz.NecroDancer.Web.Api.Areas.HelpPage.Models;
using toofz.NecroDancer.Web.Api.Controllers;

namespace toofz.NecroDancer.Web.Api.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        private static readonly Dictionary<Type, int> DisplayOrder = new Dictionary<Type, int>
        {
            { typeof(ItemsController), 0 },
            { typeof(EnemiesController), 1 },
            { typeof(LeaderboardsController), 2 },
            { typeof(PlayersController), 3 },
            { typeof(ReplaysController), 4 },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpController"/> class.
        /// </summary>
        public HelpController() : this(GlobalConfiguration.Configuration) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpController"/> class with the specified configuration.
        /// </summary>
        /// <param name="config">The configuration to use.</param>
        public HelpController(HttpConfiguration config)
        {
            Configuration = config;

            ViewBag.DisplayOrder = DisplayOrder;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return HttpNotFound();
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return HttpNotFound();
        }
    }
}