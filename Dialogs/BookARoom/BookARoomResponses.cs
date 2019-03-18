﻿using System;
using System.Collections.Generic;
using System.Linq;
using HotelBot.Dialogs.BookARoom.Resources;
using HotelBot.Dialogs.Shared.CustomDialog;
using HotelBot.Extensions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace HotelBot.Dialogs.BookARoom
{
    public class BookARoomResponses: TemplateManager

    {
        private static readonly LanguageTemplateDictionary _responseTemplates = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                {
                    ResponseIds.EmailPrompt, (context, data) =>
                        MessageFactory.Text(
                            BookARoomStrings.EMAIL_PROMPT,
                            BookARoomStrings.EMAIL_PROMPT,
                            InputHints.ExpectingInput)
                },
                {
                    ResponseIds.HaveEmailMessage, (context, data) =>
                        MessageFactory.Text(
                            text: string.Format(BookARoomStrings.HAVE_EMAIL, data),
                            ssml: string.Format(BookARoomStrings.HAVE_EMAIL, data),
                            inputHint: InputHints.IgnoringInput)
                },
                {
                    ResponseIds.ArrivalDatePrompt, (context, data) =>
                        MessageFactory.Text(
                            BookARoomStrings.ARRIVALDATE_PROMPT,
                            BookARoomStrings.ARRIVALDATE_PROMPT,
                            InputHints.IgnoringInput)
                },
                {
                    ResponseIds.HaveArrivalDate, (context, data) =>
                        MessageFactory.Text(
                            text: string.Format(BookARoomStrings.HAVE_ARRIVALDATE, data),
                            ssml: string.Format(BookARoomStrings.HAVE_ARRIVALDATE, data),
                            inputHint: InputHints.IgnoringInput)
                },
                {
                    ResponseIds.LeavingDatePrompt, (context, data) =>
                        MessageFactory.Text(
                            BookARoomStrings.LEAVINGDATE_PROMPT,
                            BookARoomStrings.LEAVINGDATE_PROMPT,
                            InputHints.IgnoringInput)
                },
                {
                    ResponseIds.HaveLeavingDate, (context, data) =>
                        MessageFactory.Text(
                            text: string.Format(BookARoomStrings.HAVE_LEAVINGDATE, data),
                            ssml: string.Format(BookARoomStrings.HAVE_LEAVINGDATE, data),
                            inputHint: InputHints.IgnoringInput)
                },
                {
                    ResponseIds.NumberOfPeoplePrompt, (context, data) =>
                        MessageFactory.Text(
                            BookARoomStrings.NUMBEROFPEOPLE_PROMPT,
                            BookARoomStrings.NUMBEROFPEOPLE_PROMPT,
                            InputHints.ExpectingInput)
                },
                {
                    ResponseIds.HaveNumberOfPeople, (context, data) =>
                        MessageFactory.Text(
                            text: string.Format(BookARoomStrings.HAVE_NUMBEROFPEOPLE, data),
                            ssml: string.Format(BookARoomStrings.HAVE_NUMBEROFPEOPLE, data),
                            inputHint: InputHints.IgnoringInput)
                },
                {
                    ResponseIds.IncorrectDate, (context, data) =>
                        MessageFactory.Text(
                            BookARoomStrings.INCORRECT_DATE,
                            BookARoomStrings.INCORRECT_DATE,
                            InputHints.IgnoringInput)
                },
                {
                    ResponseIds.NotRecognizedDate, (context, data) =>
                        MessageFactory.Text(
                            BookARoomStrings.NOT_RECOGNIZED_DATE,
                            BookARoomStrings.NOT_RECOGNIZED_DATE,
                            InputHints.IgnoringInput)
                },
                {
                    ResponseIds.UpdateEmail, (context, data) =>
                        UpdateEmail(context)
                },
                {
                    ResponseIds.UpdateNumberOfPeople, (context, data) =>
                        UpdateNumberOfPeople(context)
                },
                {
                    ResponseIds.UpdateLeavingDate, (context, data) =>
                        UpdateLeavingDate(context)
                },
                {
                    ResponseIds.UpdateArrivalDate, (context, data) =>
                        UpdateArrivalDate(context)
                },
                {
                    ResponseIds.Overview, (context, data) =>
                        SendOverview(context, data)
                },
                {
                    ResponseIds.Introduction, (context, data) =>
                        MessageFactory.Text(
                            BookARoomStrings.INTRODUCTION,
                            BookARoomStrings.INTRODUCTION,
                            InputHints.IgnoringInput)
                },
                {
                    ResponseIds.SpecificTimePrompt, (context, data) =>
                        MessageFactory.Text(
                            BookARoomStrings.SPECIFICTIME_REPLY,
                            BookARoomStrings.SPECIFICTIME_REPLY,
                            InputHints.IgnoringInput)
                },
                {
                    ResponseIds.Help, (context, data) =>
                        MessageFactory.Text(
                            BookARoomStrings.HELP_MESSAGE,
                            BookARoomStrings.HELP_MESSAGE,
                            InputHints.IgnoringInput)
                },
                {
                    ResponseIds.SendRooms, (context, data) =>
                        SendRoomsCarousel(context, data)
                },
            }
        };



        public BookARoomResponses()
        {
            Register(new DictionaryRenderer(_responseTemplates));
        }

        public static IMessageActivity SendRoomsCarousel(ITurnContext context, dynamic data)
        {
            if (data.GetType() != typeof(BookARoomState))
            {
                throw new InvalidCastException("Invalid type given");
            }
            var bookARoomState = data as BookARoomState;
            var url = "https://www.google.com";
            var heroCards = new []
            {
                new HeroCard
                {
                    Title = "Hotel in Bruges",
                    Subtitle = $"Available on {bookARoomState.ArrivalDate}",
                    Images = new List<CardImage>
                    {
                        new CardImage(
                            "https://www.marinabaysands.com/content/dam/singapore/marinabaysands/master/main/home/hotel/rooms-suites/roomv2-mobile_500x454.jpg")
                    },
                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Book now", value: url),
                        new CardAction(ActionTypes.OpenUrl, "More info",value: url)
                    }
                },
                new HeroCard
                {
                    Title = "Another Hotel in Bruges",
                    Subtitle = $"Available on {bookARoomState.ArrivalDate}",
                    Images = new List<CardImage>
                    {
                        new CardImage("https://media.cntraveler.com/photos/580e72a51dbfcd3538b953ec/4:3/w_480,c_limit/Bedroom-ThePeninsulaParis-ParisFrance-CRHotel.jpg")
                    },
                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Book now", value: url),
                        new CardAction(ActionTypes.OpenUrl, "More info", value: url)
                    }
                },
                new HeroCard
                {
                    Title = "Yet another hotel in Bruges",
                    Subtitle = $"Available on {bookARoomState.ArrivalDate}",
                    Images = new List<CardImage>
                    {
                        new CardImage("https://t-ec.bstatic.com/images/hotel/max1024x768/159/159071869.jpg")
                    },
                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Book now", value: url),
                        new CardAction(ActionTypes.OpenUrl, "More info", value: url)
                    }
                }


            };

            var reply = context.Activity.CreateReply();
            reply.Text = "Here are some available rooms, book one now or find more info.";
            var attachments = new List<Attachment>();

            foreach (var heroCard in heroCards) attachments.Add(heroCard.ToAttachment());


            reply.AttachmentLayout = "carousel";
            reply.Attachments = attachments;
            return reply;
        }


        public static IMessageActivity UpdateEmail(ITurnContext context)
        {

            context.TurnState.TryGetValue("bookARoomState", out var x);
            var state = x as BookARoomState;
            state.LuisResults.TryGetValue("LuisResult_BookARoom", out var luisResult);
            string message;
            if (luisResult.HasEntityWithPropertyName(EntityNames.Email))
            {
                var emailString = luisResult.Entities.email[0];
                message = string.Format(BookARoomStrings.UPDATE_EMAIL_WITH_ENTITY, emailString);
            }
            else
            {
                message = BookARoomStrings.UPDATE_EMAIL_WITHOUT_ENTITY;
            }

            return MessageFactory.Text(message);
        }

        public static IMessageActivity UpdateArrivalDate(ITurnContext context)
        {

            context.TurnState.TryGetValue("tempTimex", out var t);
            var timexProperty = t as TimexProperty;
            string message;
            if (timexProperty != null)
            {
                var dateAsNaturalLanguage = timexProperty.ToNaturalLanguage(DateTime.Now);
                message = string.Format(BookARoomStrings.UPDATE_ARRIVALDATE_WITH_ENTITY, dateAsNaturalLanguage);
                // todo: add old value in string? --> use bookaroomstate, passed in turnstate

            }
            else
            {
                message = BookARoomStrings.UPDATE_ARRIVALDATE_WITHOUT_ENTITY;
            }

            return MessageFactory.Text(message);

        }


        public static IMessageActivity UpdateNumberOfPeople(ITurnContext context)
        {

            context.TurnState.TryGetValue("bookARoomState", out var x);
            var state = x as BookARoomState;
            state.LuisResults.TryGetValue("LuisResult_BookARoom", out var luisResult);
            string message;
            if (luisResult.Entities.number != null)
            {
                var numberOfPeopleString = luisResult.Entities.number.First().ToString();
                message = string.Format(BookARoomStrings.UPDATE_NUMBEROFPEOPLE_WITH_ENTITY, numberOfPeopleString);

            }
            else
            {
                message = BookARoomStrings.UPDATE_NUMBEROFPEOPLE_WITHOUT_ENTITY;
            }

            return MessageFactory.Text(message);
        }


        public static IMessageActivity UpdateLeavingDate(ITurnContext context)
        {
            context.TurnState.TryGetValue("tempTimex", out var t);
            var timexProperty = t as TimexProperty;
            string message;
            if (timexProperty != null)
            {
                var dateAsNaturalLanguage = timexProperty.ToNaturalLanguage(DateTime.Now);
                message = string.Format(BookARoomStrings.UPDATE_LEAVINGDATE_WITH_ENTITY, dateAsNaturalLanguage);
                // todo: add old value in string? --> use bookaroomstate, passed in turnstate

            }
            else
            {
                message = BookARoomStrings.UPDATE_LEAVINGDATE_WITHOUT_ENTITY;
            }

            return MessageFactory.Text(message);
        }


        public static IMessageActivity SendOverview(ITurnContext context, BookARoomState state)
        {
            var message = string.Format(BookARoomStrings.STATE_OVERVIEW, state.NumberOfPeople, state.ArrivalDate, state.LeavingDate, state.Email);
            return MessageFactory.Text(message);
        }




        public class ResponseIds
        {
            public const string EmailPrompt = "emailPrompt";
            public const string HaveEmailMessage = "haveEmail";

            public const string ArrivalDatePrompt = "arrivalDatePrompt";
            public const string HaveArrivalDate = "haveArrivalDate";

            public const string LeavingDatePrompt = "leavingDatePrompt";
            public const string HaveLeavingDate = "HaveLeavingDate";

            public const string NumberOfPeoplePrompt = "numberOfPeoplePrompt";
            public const string HaveNumberOfPeople = "HaveNumberOfPeople";

            public const string IncorrectDate = "incorrectDate";
            public const string NotRecognizedDate = "notRecognizedDate";

            public const string SpecificTimePrompt = "specificTimePrompt";

            public const string Help = "help";

            public const string Overview = "overview";
            public const string Introduction = "introduction";
            public const string SendRooms = "sendRooms";


            // intents

            public const string UpdateEmail = "Update_email"; // todo: update in LUIS
            public const string UpdateArrivalDate = "Update_ArrivalDate";
            public const string UpdateLeavingDate = "Update_Leaving_Date"; // todo: update in LUIS
            public const string UpdateNumberOfPeople = "Update_Number_Of_People";
        }
    }
}
