﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelBot.Dialogs.Shared.CustomDialog;
using HotelBot.Dialogs.Shared.Validators;
using HotelBot.Services;
using HotelBot.Shared.Helpers;
using HotelBot.StateAccessors;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Newtonsoft.Json;
using AdaptiveCards;
using Microsoft.Bot.Schema;
using File = System.IO.File;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Builder;

namespace HotelBot.Dialogs.BookARoom
{
    public class BookARoomDialog: CustomDialog
    {
        private static BookARoomResponses _responder;
        private readonly StateBotAccessors _accessors;
        private readonly BotServices _services;
        private BookARoomState _state;
        private TranslatorHelper _translatorHelper = new TranslatorHelper();
        private readonly Validators _validators = new Validators();
        

        public BookARoomDialog(BotServices services, StateBotAccessors accessors)
            : base(services, accessors, nameof(BookARoomDialog))
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));
            _responder = new BookARoomResponses();
            InitialDialogId = nameof(BookARoomDialog);


            var bookARoom = new WaterfallStep []
            {
                AskForEmail, AskForNumberOfPeople, AskForArrivalDate, AskForLeavingDate, PromptConfirm, ProcessConfirmPrompt, LoopDialog
            };
            AddDialog(new WaterfallDialog(InitialDialogId, bookARoom));
            AddDialog(new DateTimePrompt(DialogIds.ArrivalDateTimePrompt, _validators.DateValidatorAsync));
            AddDialog(new DateTimePrompt(DialogIds.LeavingDateTimePrompt, _validators.DateValidatorAsync));
            AddDialog(new TextPrompt(DialogIds.EmailPrompt, _validators.EmailValidatorAsync));
            AddDialog(new NumberPrompt<int>(DialogIds.NumberOfPeopleNumberPrompt));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));


        }

  


        // first step --> intent checking and entity gathering was done in the general book a room intent
        public async Task<DialogTurnResult> AskForEmail(WaterfallStepContext sc, CancellationToken cancellationToken)

        {

            if (sc.Options == "fromMainDialog")
            {
                await _responder.ReplyWith(sc.Context, BookARoomResponses.ResponseIds.Introduction);

            }

            _state = await _accessors.BookARoomStateAccessor.GetAsync(sc.Context, () => new BookARoomState());
            // property was gathered by LUIS or replaced manually after a confirm prompt
            if (_state.Email != null)
            {
                // skip to next step and send a reply with the email
                //await _responder.ReplyWith(
                //    sc.Context,
                //    BookARoomResponses.ResponseIds.HaveEmailMessage,
                //    _state.Email);
                return await sc.NextAsync();
            }

            // else prompt for email
            return await sc.PromptAsync(
                DialogIds.EmailPrompt,
                new PromptOptions
                {
                    Prompt = await _responder.RenderTemplate(sc.Context, sc.Context.Activity.Locale, BookARoomResponses.ResponseIds.EmailPrompt)
                });
        }


        // step 
        public async Task<DialogTurnResult> AskForNumberOfPeople(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _state = await _accessors.BookARoomStateAccessor.GetAsync(sc.Context, () => new BookARoomState());
            if (sc.Result != null)
            {
                _state.Email = (string) sc.Result;
                await _responder.ReplyWith(
                    sc.Context,
                    BookARoomResponses.ResponseIds.HaveEmailMessage,
                    _state.Email);
            }

            if (_state.NumberOfPeople != null)
            {
                //await _responder.ReplyWith(sc.Context, BookARoomResponses.ResponseIds.HaveNumberOfPeople, _state.NumberOfPeople);
                return await sc.NextAsync();
            }

            return await sc.PromptAsync(
                DialogIds.NumberOfPeopleNumberPrompt,
                new PromptOptions
                {
                    Prompt = await _responder.RenderTemplate(sc.Context, sc.Context.Activity.Locale, BookARoomResponses.ResponseIds.NumberOfPeoplePrompt)
                });

        }

        public async Task<DialogTurnResult> AskForArrivalDate(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _state = await _accessors.BookARoomStateAccessor.GetAsync(sc.Context, () => new BookARoomState());
            if (sc.Result != null)
            {
                _state.NumberOfPeople = (int) sc.Result;
                await _responder.ReplyWith(sc.Context, BookARoomResponses.ResponseIds.HaveNumberOfPeople, _state.NumberOfPeople);
            }

            if (_state.ArrivalDate != null) return await sc.NextAsync();

            return await sc.PromptAsync(
                DialogIds.ArrivalDateTimePrompt,
                new PromptOptions
                {
                    Prompt = await _responder.RenderTemplate(sc.Context, Culture.Dutch, BookARoomResponses.ResponseIds.ArrivalDatePrompt)
                });

        }

        public async Task<DialogTurnResult> AskForLeavingDate(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _state = await _accessors.BookARoomStateAccessor.GetAsync(sc.Context, () => new BookARoomState());
            if (sc.Result != null)
            {
                var resolution = (sc.Result as IList<DateTimeResolution>).First();
                var timexProp = new TimexProperty(resolution.Timex);
                var arrivalDateAsNaturalLanguage = timexProp.ToNaturalLanguage(DateTime.Now);
                _state.ArrivalDate = timexProp;
                await _responder.ReplyWith(sc.Context, BookARoomResponses.ResponseIds.HaveArrivalDate, arrivalDateAsNaturalLanguage);
            }


            if (_state.LeavingDate != null)
            {

                //await _responder.ReplyWith(sc.Context, BookARoomResponses.ResponseIds.HaveLeavingDate, _state.LeavingDate);
                return await sc.NextAsync();
            }

            return await sc.PromptAsync(
                DialogIds.LeavingDateTimePrompt,
                new PromptOptions
                {
                    Prompt = await _responder.RenderTemplate(sc.Context, sc.Context.Activity.Locale, BookARoomResponses.ResponseIds.LeavingDatePrompt)
                });

        }

        public async Task<DialogTurnResult> PromptConfirm(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _state = await _accessors.BookARoomStateAccessor.GetAsync(sc.Context, () => new BookARoomState());

            if (sc.Result != null)
            {
                var resolution = (sc.Result as IList<DateTimeResolution>).First();
                var timexProp = new TimexProperty(resolution.Timex);
                _state.LeavingDate = timexProp;
                await _responder.ReplyWith(sc.Context, BookARoomResponses.ResponseIds.HaveLeavingDate, _state.LeavingDate);
            }

            // var bookARoomEmpty = new BookARoomState();
            //  await _accessors.BookARoomStateAccessor.SetAsync(sc.Context, bookARoomEmpty);

            return await sc.PromptAsync(
                nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = await _responder.RenderTemplate(sc.Context, sc.Context.Activity.Locale, BookARoomResponses.ResponseIds.Overview, _state)
                });
        }

        public async Task<DialogTurnResult> ProcessConfirmPrompt(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            var confirmed = (bool)sc.Result;
            if (confirmed)
            {
                // skip to next step to show carousel of available rooms. 
                // send new cards --> end this dialog --> main dialog
                return await sc.EndDialogAsync();

            }
            return await sc.PromptAsync(
                nameof(ChoicePrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("What would you like to adjust?"),
                    RetryPrompt = MessageFactory.Text("Pick an item or tell me"),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Arrival", "Leaving", "Number of people", "Email" }),
                },
                cancellationToken);

            // end the current dialog (this waterfall) and replace it with a bookaroomdialog
            //  await sc.EndDialogAsync(cancellationToken);
        }

        public async Task<DialogTurnResult> LoopDialog(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _state = await _accessors.BookARoomStateAccessor.GetAsync(sc.Context, () => new BookARoomState());
            var choice = sc.Result as FoundChoice;
            
            switch (choice.Value)
            {
                case "Arrival":
                    _state.ArrivalDate = null;
                    break;
                case "Leaving":
                    _state.LeavingDate = null;
                    break;
                case "Number of people":
                    _state.NumberOfPeople = null;
                    break;
                case "Email":
                    _state.Email = null;
                    break;


            }
            return await sc.ReplaceDialogAsync(InitialDialogId, null);
        }




        public class DialogIds
        {
            public const string ArrivalDateTimePrompt = "arrivalDateTimePrompt";
            public const string LeavingDateTimePrompt = "leavingDateTimePrompt";
            public const string NumberOfPeopleNumberPrompt = "NumberOfPeople";
            public const string EmailPrompt = "Email";
        }
    }
}
