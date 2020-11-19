using System;
using System.Collections.Generic;
using System.Text;

namespace DesdinovaModelPipeline.Helpers
{
     /// <summary>
    /// This class provides functionality to load classes from
    ///  external assemblies with ease. It also allow to create
    ///  instances of such classes.
    /// </summary>
    public abstract class ModulesFactory
    {
        #region Non-exposed members
        private static string[] allowedLibs = null;

        private static object locker = new object();
        private static string pvt_sLibrariesPath = null;

        private static string[] ListAvailableLibraries()
        {
            if (allowedLibs == null)
            {
                List<string> rv = new List<string>();
                string[] files = System.IO.Directory.GetFiles(LibrariesPath, "*.dll");
                rv.AddRange(files);
                files = System.IO.Directory.GetFiles(LibrariesPath, "*.exe");
                rv.AddRange(files);
                return rv.ToArray();
            }
            else
                return allowedLibs;
        }

        private static bool HasInterface(Type t, string sName)
        {
            Type[] types = t.GetInterfaces();
            foreach (Type type in types)
                if (type.Name == sName || type.FullName == sName)
                    return true;
            return false;
        }

        private static string GetModulePath()
        {
            string sPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            sPath = sPath.Replace("file://", "");
            sPath = sPath.Replace("file:\\", "");
            return sPath;
        }
        #endregion

        #region Exposed members
        /// <summary>
        /// Set the libraries that this class is allowed to scan.
        /// This can be used to enforce security and prevent the
        ///  use of unsafe libraries.
        /// </summary>
        /// <param name="libs">A list of full path names to dll or exe files.</param>
        public static void SetAllowedLibraries(string[] libs)
        {
            allowedLibs = libs;
        }

        /// <summary>
        /// Set or retrieve the path where libraries must be searched into.
        /// </summary>
        public static string LibrariesPath
        {
            get
            {
                lock (locker)
                {
                    if (pvt_sLibrariesPath == null)
                        pvt_sLibrariesPath = GetModulePath();
                    return pvt_sLibrariesPath;
                }
            }
            set
            {
                lock (locker)
                    pvt_sLibrariesPath = value;
            }
        }

        /// <summary>
        /// Create an instance of a class from a specified library.
        /// </summary>
        /// <param name="sLibName">The library from which load the class.</param>
        /// <param name="sClassName">The class to instantiate.</param>
        /// <returns>An object of the specified class.</returns>
        public static object LoadClass(string sLibName, string sClassName)
        {
            System.Reflection.Assembly a = null;
            try
            {
                a = System.Reflection.Assembly.LoadFrom(sLibName);
            }
            catch
            {
                return null;
            }
            if (a == null)
            {
                throw new NullReferenceException("The supplied module cannot be found.");
            }
            else
            {
                Type[] types = a.GetTypes();
                string fullName = "";
                for (int i = 0; i < types.Length; i++)
                    if (types[i].IsClass == true)
                        if(types[i].IsAbstract==false)
                            if (types[i].Name == sClassName)
                                fullName = types[i].FullName;
                if (fullName.Length == 0)
                    return null;
                return a.CreateInstance(fullName);
            }
        }

        /// <summary>
        /// Determine if there is a specified class implementation into a library.
        /// </summary>
        /// <param name="sLibName">The library to inspect.</param>
        /// <param name="sClassName">The class to look for.</param>
        /// <returns>Returns true if the class is found. Returns false otherwise.</returns>
        public static bool HasClass(string sLibName, string sClassName)
        {
            System.Reflection.Assembly a = null;
            try
            {
                a = System.Reflection.Assembly.LoadFrom(sLibName);
            }
            catch
            {
                return false;
            }
            if (a == null)
            {
                throw new NullReferenceException("The supplied module cannot be found.");
            }
            else
            {
                Type[] types = a.GetTypes();
                for (int i = 0; i < types.Length; i++)
                    if (types[i].IsClass == true)
                        if (types[i].Name == sClassName)
                            return true;
                return false;
            }
        }

        /// <summary>
        /// Determine if there is a specified interface implementation into a library.
        /// </summary>
        /// <param name="sLibName">The library to inspect.</param>
        /// <param name="sInterface">The interface to look for.</param>
        /// <returns>Returns true if there is an implementation, false otherwise.</returns>
        public static bool HasInterfaceImpl(string sLibName, string sInterface)
        {
            System.Reflection.Assembly a = null;

            try
            {
                a = System.Reflection.Assembly.LoadFrom(sLibName);
            }
            catch
            {
                return false;
            }
            if (a == null)
            {
                throw new NullReferenceException("The supplied module cannot be found or loaded.");
            }
            else
            {
                Type[] types = a.GetTypes();
                for (int i = 0; i < types.Length; i++)
                    if (types[i].IsClass == true)
                        if (HasInterface(types[i], sInterface) == true)
                            return true;
                //if (Functions.InterfaceName(types[i]) == sInterface)
                //    return true;
                return false;
            }
        }

        /// <summary>
        /// Return the name for a class of a specified implementation, by looking inside a specified library.
        /// </summary>
        /// <param name="sLibName">The library to inspect.</param>
        /// <param name="sInterface">The interface of the class.</param>
        /// <returns></returns>
        public static string GetClassName(string sLibName, string sInterface)
        {
            System.Reflection.Assembly a = null;
            try
            {
                a = System.Reflection.Assembly.LoadFrom(sLibName);
            }
            catch
            {
                return null;
            }
            if (a == null)
            {
                throw new NullReferenceException("The supplied module cannot be found.");
            }
            else
            {
                Type[] types = a.GetTypes();
                for (int i = 0; i < types.Length; i++)
                    if (types[i].IsClass == true)
                        if (HasInterface(types[i], sInterface) == true)
                            return types[i].Name;
                //if (Functions.InterfaceName(types[i]) == sInterface)
                //    return types[i].Name;
                return null;
            }
        }

        /// <summary>
        /// Return the names for all classes of a specified implementation that are present into a specified library.
        /// </summary>
        /// <param name="sLibName">The library to inspect.</param>
        /// <param name="sInterface">The interface of the class.</param>
        /// <returns>An array of strings containing all the classes with the supplied interface implemented.</returns>
        public static string[] GetClassNames(string sLibName, string sInterface)
        {
            System.Reflection.Assembly a = null;
            try
            {
                a = System.Reflection.Assembly.LoadFrom(sLibName);
            }
            catch
            {
                return null;
            }
            if (a == null)
            {
                throw new NullReferenceException("The supplied module cannot be found.");
            }
            else
            {
                List<string> rv = new List<string>();
                Type[] types = a.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    if (types[i].IsClass == true)
                    {
                        if (types[i].IsAbstract == false)
                        {
                            if (HasInterface(types[i], sInterface) == true)
                            {
                                rv.Add(types[i].Name);
                            }
                        }
                    }
                }
                return rv.ToArray();
            }
        }

        /// <summary>
        /// Return a new instance of a specified class, by looking into all available libraries.
        /// </summary>
        /// <param name="sClassName">The class to instantiate.</param>
        /// <returns>Return an object of the specified class.</returns>
        public static object LoadClass(string sClassName)
        {
            string[] libs = ListAvailableLibraries();
            for (int i = 0; i < libs.Length; i++)
            {
                if (HasClass(libs[i], sClassName) == true)
                    return LoadClass(libs[i], sClassName);
            }
            return null;
        }

        /// <summary>
        /// Initialize all libraries.
        /// </summary>
        public static void InitAllLibraries()
        {
            string[] libs = ListAvailableLibraries();
            for (int i = 0; i < libs.Length; i++)
            {
                System.Reflection.Assembly a = null;
                try
                {
                    a = System.Reflection.Assembly.LoadFrom(libs[i]);
                    Type[] t = a.GetTypes();
                    if (t != null)
                    {
                        if (t.Length > 0)
                        {
                            object obj = a.CreateInstance(t[0].FullName);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// List all classes responding to a specified interface, looking into all available libraries.
        /// </summary>
        /// <param name="sInterfaceName">The interface you need.</param>
        /// <returns>An array of string objects, representing all the class names found for the specified interface.</returns>
        public static string[] ListClasses(string sInterfaceName)
        {
            List<string> retval = new List<string>();
            string[] libs = ListAvailableLibraries();
            for (int i = 0; i < libs.Length; i++)
            {
                if (HasInterfaceImpl(libs[i], sInterfaceName) == true)
                    retval.AddRange(GetClassNames(libs[i], sInterfaceName));
            }
            return retval.ToArray();
        }
        #endregion
    }

}
