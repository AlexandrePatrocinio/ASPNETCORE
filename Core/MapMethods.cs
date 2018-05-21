using System;
using System.Linq;
using System.Runtime.Loader;
using System.Reflection;
using ASPNETCORE.Core.Interfaces;

public class MapMethods : IMapMethods
{
    private Assembly _Assembly;
    private Type _Class;
    private MethodInfo[] _Mtds;

    public MapMethods(string NameSpace, string AssemblyPath) => _Assembly = LoadAssembly(NameSpace, AssemblyPath);

    public Assembly LoadAssembly(string NameSpace, string AssemblyPath)
    {
        var aspnetcore = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((ass) => ass.FullName.ToUpper().Contains(NameSpace.ToUpper()));

        if (aspnetcore == null)
        {
            aspnetcore = AssemblyLoadContext.Default.LoadFromAssemblyPath(AssemblyPath);
        }

        return aspnetcore;
    }

    public MethodInfo[] LoadMethodsfromClass(string classname)
    {
        if (_Assembly != null)
        {
            _Class = _Assembly.GetType(classname);
            if (_Class != null)
            {
                _Mtds = _Class.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            }
        }

        return _Mtds;
    }

    public MethodInfo SelectMethod(string methodname) => _Mtds.FirstOrDefault((oM) => oM.Name.ToUpper() == methodname.ToUpper());    

    public object ExecuteMap(string classname, object[] constructorparams, string methodname, object[] methodparams)
    {
        object result = null;
        if (_Mtds != null && _Mtds.Length > 0)
        {
            var oMtd = SelectMethod(methodname);
            if (oMtd != null)
            {
                try
                {
                    if (methodparams == null)
                        if (constructorparams == null)
                            result = oMtd.Invoke(Activator.CreateInstance(_Class), null);
                        else
                            result = oMtd.Invoke(Activator.CreateInstance(_Class, constructorparams), null);
                    else if (constructorparams == null)
                        result = oMtd.Invoke(Activator.CreateInstance(_Class), methodparams);
                    else
                        result = oMtd.Invoke(Activator.CreateInstance(_Class, constructorparams), methodparams);
                }
                catch (Exception e)
                {
                    result = null;
                }
            }
        }
        return result;
    }

    public object ExecuteMap(string classname, string methodname, object[] methodparams) => ExecuteMap(classname, null, methodname, methodparams);

    public T ExecuteMap<T>(string classname, string methodname, object[] methodparam) => ExecuteMap<T>(classname, null, methodname, methodparam);

    public T ExecuteMap<T>(string classname, object[] constructorparams, string methodname, object[] methodparams) => (T)ExecuteMap(classname, constructorparams, methodname, methodparams);
}
