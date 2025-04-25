using FormAdvanced.BuildingBlocks.Domain;
using System.Reflection;

namespace FormAdvanced.API.Constants
{
    public static class ModuleConstants
    {
        //todo: load this better (source generators? static type?)
        public static Assembly[] ModuleAssemblies =>
            Assembly.GetExecutingAssembly().GetReferencedAssembliesContaining("Modules"); 
        public static Assembly[] InfrastructureAssemblies => ModuleAssemblies.Where(a => a.FullName.Contains("Infrastructure")).ToArray(); 
    }
}
