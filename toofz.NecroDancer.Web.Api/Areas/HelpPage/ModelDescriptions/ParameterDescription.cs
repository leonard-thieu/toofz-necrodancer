using System.Collections.ObjectModel;

namespace toofz.NecroDancer.Web.Api.Areas.HelpPage.ModelDescriptions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ParameterDescription
    {
        public ParameterDescription()
        {
            Annotations = new Collection<ParameterAnnotation>();
        }

        public Collection<ParameterAnnotation> Annotations { get; private set; }

        public string Documentation { get; set; }

        public string Name { get; set; }

        public ModelDescription TypeDescription { get; set; }
    }
}