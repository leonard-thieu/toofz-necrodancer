using System;

namespace toofz.NecroDancer.Web.Api.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// Describes a type model.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public abstract class ModelDescription
    {
        public string Documentation { get; set; }

        public Type ModelType { get; set; }

        public string Name { get; set; }
    }
}