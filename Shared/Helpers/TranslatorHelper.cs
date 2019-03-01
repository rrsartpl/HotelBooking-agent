﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBot.Models.Translator;

namespace HotelBot.Shared.Helpers
{
    public class TranslatorHelper
    {
        private readonly MicrosoftTranslator _translator;
        //TODO: remove hardcoded key
        private readonly string key = "e4db663da7724d13924e2dcb2c39e971";
        public TranslatorHelper()
        {
            _translator = new MicrosoftTranslator(key);
        }

        public async Task<string> TranslateText(string text, string targetLocale)
        {
            return await _translator.TranslateAsync(text, targetLocale);
        }

        public bool ShouldTranslate(string facebookLocale)
        {
            var lang = facebookLocale.Substring(0, 2);
            return lang != TranslationSettings.DefaultLanguage;
        }
    }
}
