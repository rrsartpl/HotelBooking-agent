using System;
using HotelBot.Models.LUIS;
using HotelBot.Shared.Helpers;

namespace HotelBot.Extensions
{
    public static class HotelBotLuisExtensions
    {
        public static bool HasEntityWithPropertyName(this HotelBotLuis luisResult, string propertyName)
        {
            if (luisResult == null) throw new ArgumentNullException(nameof(luisResult));

            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            var GetDynamicProperty = TypeUtility<HotelBotLuis._Entities>.GetMemberGetDelegate<dynamic>(propertyName);
            var dynamicResult = GetDynamicProperty(luisResult.Entities);
            if (dynamicResult != null) return true;
            return false;

        }

        public static bool IsUpdateDateIntent(this HotelBotLuis.Intent luisIntent)
        {
            return (luisIntent == HotelBotLuis.Intent.Update_ArrivalDate || luisIntent == HotelBotLuis.Intent.Update_Leaving_Date);
        }

    }
}
