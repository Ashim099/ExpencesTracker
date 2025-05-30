using System.Globalization;
using ExpencesTracker.Mobile.Resources.Strings;

namespace ExpencesTracker.Mobile.Services;

public static class LocalizationService
{
    /// <summary>
    /// Constant string value of English language
    /// </summary>
    public const string English = "en";
    
    /// <summary>
    /// Constant string value of Russian language
    /// </summary>
    public const string Russian = "ru";
    
    /// <summary>
    /// Array of supported languages. Represented by code
    /// </summary>
    public static readonly string[] SupportedLanguages = [English, Russian];
    
    public static string CurrentLanguage => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    
    public static void ChangeCurrentLanguage(string language)
    {
        if (!SupportedLanguages.Contains(language))
        {
            throw new ArgumentException("This language is not supported.");
        }
            
        var culture = new CultureInfo(language);
        
        
        AppResources.Culture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}