using System.Reflection;
using System.Runtime.InteropServices;
using System.Resources;
using System.Security;
using System;
using System.Runtime.CompilerServices;

// Added to tell .NET that we don't need full trust access. We don't need to elevate
// [assembly: SecurityTransparent]
// We REMED this out because of http://msdn.microsoft.com/library/dd997445(VS.100).aspx

// http://msdn.microsoft.com/library/ms182156(VS.100).aspx
[assembly: CLSCompliant(false)]

#if !RELEASE
[assembly: InternalsVisibleTo("OdessaPCTestHelpers")]
[assembly: InternalsVisibleTo("TuningHostProject")]
#endif

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Highlight Hunter")]
[assembly: AssemblyDescription("Quickly find and share the highlights in your videos")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Authentically Digital LLC")]
[assembly: AssemblyProduct("Highlight Hunter")]
[assembly: AssemblyCopyright("Copyright © Authentically Digital 2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d147cba0-5103-4c93-a24c-558cb30c5f56")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion("2.1.1.0")]
[assembly: AssemblyFileVersion("2.1.1.0")]

/*
#if RELEASE // due to bug at http://social.msdn.microsoft.com/Forums/en/vsdebug/thread/6533739e-6554-4f03-a286-336716bcb954
[assembly: AssemblyVersion("1.0.3.*")]
[assembly: AssemblyFileVersion("1.0.3.0")]
#endif
*/
[assembly: NeutralResourcesLanguageAttribute("en")]
