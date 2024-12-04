using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// Les informations générales relatives à un assembly dépendent de
// l'ensemble d'attributs suivant. Changez les valeurs de ces attributs pour modifier les informations
// associées à un assembly.
[assembly: AssemblyTitle("Gmail notifications for Windows")]
[assembly: AssemblyDescription("Notifications of new Gmail messages for Windows platforms")]
[assembly: AssemblyCompany("Xavier Foucrier")]
[assembly: AssemblyProduct("Inbox Notifier")]
[assembly: AssemblyCopyright("Copyright © 2024 Inbox Notifier")]
[assembly: AssemblyTrademark("Xavier Foucrier")]
[assembly: AssemblyCulture("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
	[assembly: AssemblyConfiguration("Release")]
#endif

// L'affectation de la valeur false à ComVisible rend les types invisibles dans cet assembly
// aux composants COM.  Si vous devez accéder à un type dans cet assembly à partir de
// COM, affectez la valeur true à l'attribut ComVisible sur ce type.
[assembly: ComVisible(false)]

// Le GUID suivant est pour l'ID de la typelib si ce projet est exposé à COM
[assembly: Guid("80166a24-d83d-43cd-b290-133edb9f3713")]

// Les informations de version pour un assembly se composent des quatre valeurs suivantes :
//
//      Version principale
//      Version secondaire
//      Numéro de build
//      Révision
//
// Vous pouvez spécifier toutes les valeurs ou indiquer les numéros de build et de révision par défaut
// en utilisant '*', comme indiqué ci-dessous :
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.15.0.0")]
[assembly: AssemblyFileVersion("3.15.0.0")]
[assembly: NeutralResourcesLanguageAttribute("fr-FR")]