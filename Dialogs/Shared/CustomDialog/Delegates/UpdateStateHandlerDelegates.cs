﻿using System;
using System.Collections.Generic;
using HotelBot.Dialogs.BookARoom;
using HotelBot.Models.LUIS;
using Microsoft.Bot.Builder.Dialogs;

namespace HotelBot.Dialogs.Shared.CustomDialog.Delegates
{


    // todo: refactor in generic states
    public class UpdateStateHandlerDelegates: Dictionary<HotelBotLuis.Intent, Action<BookARoomState, HotelBotLuis>>
    {
    }
}