//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelBot.Dialogs.Prompts.LocationPrompt.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class LocationStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LocationStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HotelBot.Dialogs.Prompts.LocationPrompt.Resources.LocationStrings", typeof(LocationStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Navigate 🗺️.
        /// </summary>
        public static string HEROCARD_BUTTON_DIRECTION_TITLE {
            get {
                return ResourceManager.GetString("HEROCARD_BUTTON_DIRECTION_TITLE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Found you the shortest route to our hotel 🤓.
        /// </summary>
        public static string HEROCARD_REPLY_TEXT_DIRECTION {
            get {
                return ResourceManager.GetString("HEROCARD_REPLY_TEXT_DIRECTION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Send me your location and I can search for directions 👇.
        /// </summary>
        public static string QUICK_REPLY_ASK_LOCATION {
            get {
                return ResourceManager.GetString("QUICK_REPLY_ASK_LOCATION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Directions.
        /// </summary>
        public static string QUICK_REPLY_BUTTON_DIRECTION {
            get {
                return ResourceManager.GetString("QUICK_REPLY_BUTTON_DIRECTION", resourceCulture);
            }
        }
    }
}
