using System;
using System.Collections.Generic;
using System.Text;

namespace Chap05.TranslationImplementations {
    public class TranslateToGerman : MarshalByRefObject, Chap05.TranslationDefinitions.ITranslationServices {
        public string Translate( string word ) {
            if( String.Compare( word, "Good Morning" ) == 0 ) {
                return "Guten Morgen";
            }
            else {
                return "Could not translate";
            }
        }
    }
}
