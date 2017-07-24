using System.Collections.ObjectModel;

namespace toofz.NecroDancer.Web.Api.Areas.HelpPage.ModelDescriptions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ComplexTypeModelDescription : ModelDescription
    {
        public ComplexTypeModelDescription()
        {
            Properties = new Collection<ParameterDescription>();
        }

        public Collection<ParameterDescription> Properties { get; private set; }
    }
}