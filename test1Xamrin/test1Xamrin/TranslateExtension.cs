using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Telerik.XamarinForms.Common;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace test1Xamrin
{
    // You exclude the 'Extension' suffix when using in XAML
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci = null;
        const string ResourceId = "ChatBot.Resx.AppResources";
        //"xFatturazioneAttiva.Resx.AppResources";

        static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
            () => new ResourceManager(ResourceId, IntrospectionExtensions.GetTypeInfo(typeof(TranslateExtension)).Assembly));

        public string Text { get; set; }

        public TranslateExtension()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return string.Empty;

            CultureInfo ci2;
            if (ci.TwoLetterISOLanguageName.ToLower() == "en") //20181201
                ci2 = new CultureInfo("en-GB");
            else
                ci2 = ci; //ci comes from the two dependecies services
            //if de or it it will match the resource, otherwise it will fall to default (AppResources)
            //if one needs to add another language firts check with two letters if it works; if not add
            //a four letter resource (like en-GB) and redirect like in line 37

            var translation = ResMgr.Value.GetString(Text, ci2);

            //translation = Resx.AppResources_de.ResourceManager.GetString("TitoloRiga1");


            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    string.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
