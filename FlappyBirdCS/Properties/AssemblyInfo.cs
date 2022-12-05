using System.Resources;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Le informazioni generali relative a un assembly sono controllate dal seguente 
// set di attributi. Modificare i valori di questi attributi per modificare le informazioni
// associate a un assembly.
[assembly: AssemblyTitle("FlappyBirdCS")]
[assembly: AssemblyDescription("A simple Flappy Bird Clone Game")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Salvucci Franco")]
[assembly: AssemblyProduct("FlappyBirdCS")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Let log4net know that it can look for configuration in the default application config file
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
// Se si imposta ComVisible su false, i tipi in questo assembly non saranno visibili
// ai componenti COM. Se è necessario accedere a un tipo in questo assembly da
// COM, impostare su true l'attributo ComVisible per tale tipo.
[assembly: ComVisible(false)]

// Se il progetto viene esposto a COM, il GUID seguente verrà utilizzato come ID della libreria dei tipi
[assembly: Guid("ffdf6834-5094-4313-a4cb-c656adb81303")]

// Le informazioni sulla versione di un assembly sono costituite dai seguenti quattro valori:
//
//      Versione principale
//      Versione secondaria
//      Numero di build
//      Revisione
//
// È possibile specificare tutti i valori oppure impostare valori predefiniti per i numeri relativi alla revisione e alla build
// usando l'asterisco '*' come illustrato di seguito:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.1.1.0")]
[assembly: AssemblyFileVersion("2.1.1.0")]
[assembly: NeutralResourcesLanguage("it-IT")]
