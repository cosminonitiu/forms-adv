using System.Reflection;

namespace FormAdvanced.BuildingBlocks.Domain
{
    public static class ReflectionTypeExtensions
    {
        public static Assembly[] GetReferencedAssembliesContaining(this Assembly assembly, string name)
        {
            var mathching = assembly.GetReferencedAssemblies().Where(a => a.FullName.Contains(name));

            return mathching.Select(a => Assembly.Load(a)).ToArray();
        }

        /// <summary>
        /// When called with the type of an generic interface (i.e. <see cref="IEventHandler{TEvent}"/>), searches the given assembly for all implementations of the interface
        /// and returns a dictionary containing containing as the key the specific type of the implemented interface (i.e. <see cref="Event"/>) and as value the class implementing
        /// the interface (i.e. LocationEventHandler)
        /// </summary>
        /// <param name="type">A generic interface (i.e. <see cref="IEventHandler{TEvent}"/>)</param>
        /// <param name="assembly">The assembly in which to search implementations, if null will default to calling assembly</param>
        /// <returns>{ <see cref="Event"/>, LocationEventHandler }</returns>
        public static Dictionary<Type, Type> GetGenericArgumentAndImplementationOfType(this Type type,
            Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();

            var result = new Dictionary<Type, Type>();

            //Iterate all implementations of given interface
            foreach (var typeInfo in assembly.DefinedTypes.Where(t =>
                         t.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition().Equals(type))))
            {
                var definitionType = typeInfo.AsType(); //CommandHandler
                var paramType =
                    definitionType.GetInterfaces().First().GetGenericArguments()
                        .First(); //Get the type parameter of the implemented interface (i.e. Event)

                result.Add(paramType, definitionType);
            }

            return result;
        }
    }
}
