﻿using System;
using System.Threading;
using System.Threading.Tasks;
using HotelBot.Dialogs.Cancel;
using HotelBot.Dialogs.FetchAvailableRooms;
using HotelBot.Dialogs.Prompts.UpdateState;
using HotelBot.Extensions;
using HotelBot.Models.LUIS;
using HotelBot.Models.Wrappers;
using HotelBot.Services;
using HotelBot.StateAccessors;
using Microsoft.Bot.Builder.Dialogs;

namespace HotelBot.Dialogs.Shared.RecognizerDialogs.RoomDetail
{
    public class RoomDetailRecognizerDialog: InterruptableDialog
    {
        protected const string LuisResultBookARoomKey = "LuisResult_BookARoom";
        private readonly StateBotAccessors _accessors;
        private readonly BotServices _services;

        public RoomDetailRecognizerDialog(BotServices services, StateBotAccessors accessors, string dialogId)
            : base(dialogId)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));
            AddDialog(new CancelDialog(_accessors));
            AddDialog(new UpdateStatePrompt(accessors));
        }

        protected override async Task<InterruptionStatus> OnDialogInterruptionAsync(DialogContext dc, CancellationToken cancellationToken)
        {

            var text = dc.Context.Activity.Text;
            if (text == "changenow") return await OnReroute(dc, HotelBotLuis.Intent.Book_A_Room);

            if (FetchAvailableRoomsDialog.FetchAvailableRoomsChoices.Choices.Contains(text)) return InterruptionStatus.NoAction;


            _services.LuisServices.TryGetValue("hotelbot", out var luisService);
            if (luisService == null) throw new Exception("The specified LUIS Model could not be found in your Bot Services configuration.");
            var luisResult = await luisService.RecognizeAsync<HotelBotLuis>(dc.Context, cancellationToken);
            var intent = luisResult.TopIntent().intent;

            // Only triggers interruption if confidence level is high
            if (luisResult.TopIntent().score > 0.80)
                switch (intent)
                {

                    //todo: add routing intents --> finding available rooms + show booked rooms 

                    case HotelBotLuis.Intent.Cancel:
                    {
                        return await OnCancel(dc);
                    }
                    case HotelBotLuis.Intent.Help:
                    {
                        // todo: provide contextual help
                        return await OnHelp(dc);
                    }
                    case HotelBotLuis.Intent.Book_A_Room:
                    {
                        return await OnReroute(dc, intent);
                    }
                    case HotelBotLuis.Intent.Update_ArrivalDate:
                    case HotelBotLuis.Intent.Update_Leaving_Date:
                    case HotelBotLuis.Intent.Update_email:
                    case HotelBotLuis.Intent.Update_Number_Of_People:
                    {
                        var isDateUpdateIntent = intent.IsUpdateDateIntent();
                        return await OnUpdate(dc, isDateUpdateIntent);
                    }
                }

            // call the non overriden continue dialog in componentdialog
            return InterruptionStatus.NoAction;
        }

        protected virtual async Task<InterruptionStatus> OnCancel(DialogContext dc)
        {
            if (dc.ActiveDialog.Id != nameof(CancelDialog))
            {
                // Don't start restart cancel dialog
                await dc.BeginDialogAsync(nameof(CancelDialog));

                // Signal that the dialog is waiting on user response
                return InterruptionStatus.Waiting;
            }

            // Else, continue
            return InterruptionStatus.NoAction;
        }


        //todo: expand with other intents, show previous room etc
        protected virtual async Task<InterruptionStatus> OnReroute(DialogContext dc, HotelBotLuis.Intent intent)
        {
            // do not reroute to active dialog
            // add targetdialog to turnstate
            if (intent == HotelBotLuis.Intent.Book_A_Room)
            {
                var targetDialog = nameof(FetchAvailableRoomsDialog);
                if (targetDialog != dc.ActiveDialog.Id)
                {
                    var dialogResult = new DialogResult
                    {
                        PreviousOptions = new DialogOptions(),
                        TargetDialog = nameof(FetchAvailableRoomsDialog)
                    };

                    dc.Context.TurnState.Add("dialogResult", dialogResult);
                    return InterruptionStatus.Route;
                }
            }
            // and so on --> refactor into dictionary

            // continue if intent matches same dialog
            return InterruptionStatus.NoAction;
        }



        protected virtual async Task<InterruptionStatus> OnHelp(DialogContext dc)
        {
            var view = new FetchAvailableRoomsResponses();
            await view.ReplyWith(dc.Context, FetchAvailableRoomsResponses.ResponseIds.Help);

            // Signal the conversation was interrupted and should immediately continue (calls reprompt)
            return InterruptionStatus.Interrupted;
        }

        protected virtual async Task<InterruptionStatus> OnUpdate(DialogContext dc, bool isUpdateDate)
        {
            // do not restart this running dialog
            if (dc.ActiveDialog.Id != nameof(UpdateStatePrompt))
            {
                // example: in dialog number prompt --> i want to update my email, this prompt is a dialog pushed on the stack
                // --> old comment: to handle this interruption and start the correct new waterfall, we need to cancel the stack: bookaroomdialog>numberprompt
                // --> old comment: await dc.CancelAllDialogsAsync(); // removes entire stack
                // begin our own new dialogwaterfall step

                await dc.BeginDialogAsync(nameof(UpdateStatePrompt), isUpdateDate);

                return InterruptionStatus.Waiting;
            }

            return InterruptionStatus.NoAction;
        }
    }
}
