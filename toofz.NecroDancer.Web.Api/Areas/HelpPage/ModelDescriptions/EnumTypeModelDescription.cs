using System.Collections.ObjectModel;

namespace toofz.NecroDancer.Web.Api.Areas.HelpPage.ModelDescriptions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}