using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBot.Dialogs.FetchAvailableRooms;
using HotelBot.Models.LUIS;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace HotelBot.Dialogs.Shared.RecognizerDialogs.Delegates
{


    // todo: refactor in generic states
    public class UpdateStateHandlerDelegates: Dictionary<HotelBotLuis.Intent, Func<FetchAvailableRoomsState, HotelBotLuis, WaterfallStepContext, Task<DialogTurnResult>>>
    {
    }
}
