using System.Reflection;

namespace ASPNETCORE.Core.Interfaces
{
    public interface IMapMethods
    {
        Assembly LoadAssembly(string NameSpace, string AssemblyPath);

        MethodInfo[] LoadMethodsfromClass(string classname);

        MethodInfo SelectMethod(string methodname);

        object ExecuteMap(string classname, object[] constructorparams, string methodname, object[] methodparams);

        object ExecuteMap(string classname, string methodname, object[] methodparams);

        T ExecuteMap<T>(string classname, string methodname, object[] methodparams);

        T ExecuteMap<T>(string classname, object[] constructorparams, string methodname, object[] methodparams);
    }
}