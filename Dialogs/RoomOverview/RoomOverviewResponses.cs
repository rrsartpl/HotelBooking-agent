﻿using System.Collections.Generic;
using HotelBot.Dialogs.FetchAvailableRooms.Resources;
using HotelBot.Dialogs.RoomOverview.Resources;
using HotelBot.Models.Wrappers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace HotelBot.Dialogs.RoomOverview
{
    public class RoomOverviewResponses: TemplateManager

    {
        private static readonly LanguageTemplateDictionary _responseTemplates = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                {
                    ResponseIds.RoomAdded, (context, data) =>
                        MessageFactory.Text(
                            RoomOverviewStrings.ROOM_ADDED,
                            RoomOverviewStrings.ROOM_ADDED,
                            InputHints.IgnoringInput)
                },
                {
                    ResponseIds.ContinueOrAddMoreRooms, (context, data) =>
                        MessageFactory.Text(
                            RoomOverviewStrings.CONTINUE_OR_ADD_MORE_ROOMS,
                            RoomOverviewStrings.CONTINUE_OR_ADD_MORE_ROOMS,
                            InputHints.IgnoringInput)
                },
                {
                    ResponseIds.ShowOverview, (context, data) =>
                        SendRoomsOverviewCarousel(context, data)
                }


            }
        };

        public RoomOverviewResponses()
        {
            Register(new DictionaryRenderer(_responseTemplates));
        }



        // could be pending or confirmed payment
        public static IMessageActivity SendRoomsOverviewCarousel(ITurnContext context, dynamic data)
        {

            var roomOverviewState = data as RoomOverviewState;
            var selectedRooms = roomOverviewState.SelectedRooms;

            var heroCards = new List<HeroCard>();
            var url = "https://www.google.com";
            foreach (var selectedRoom in selectedRooms) heroCards.Add(BuildHeroCard(selectedRoom));
            var reply = context.Activity.CreateReply();
            reply.Text =
                "Here are your selected rooms";
            var attachments = new List<Attachment>();
            foreach (var heroCard in heroCards) attachments.Add(heroCard.ToAttachment());
            reply.AttachmentLayout = "carousel";
            reply.Attachments = attachments;
            return reply;
        }



        private static HeroCard BuildHeroCard(SelectedRoom selectedRoom)
        {
            return new HeroCard
            {
                Title = selectedRoom.RoomDetailDto.Title,
                Text = BuildHeroCardText(selectedRoom),
                Images = new List<CardImage>
                {
                    //todo: refactor
                    new CardImage(selectedRoom.RoomDetailDto.RoomImages[0].ImageUrl)
                },

                Buttons = new List<CardAction>
                {
                    new CardAction
                    {
                        Type = ActionTypes.MessageBack,
                        Value = JsonConvert.SerializeObject(
                            new RoomAction
                            {
                                RoomId = selectedRoom.RoomDetailDto.Id,
                                Action = "info"
                            }),
                        Title = "\t More info \t"
                    },
                    new CardAction
                    {
                        Type = ActionTypes.MessageBack,
                        Value = JsonConvert.SerializeObject(
                            new RoomAction
                            {
                                RoomId = selectedRoom.RoomDetailDto.Id,
                                Action = "remove",
                                SelectedRate = selectedRoom.SelectedRate
                            }),
                        Title = "\t Remove \t"
                    }

                }

            };
        }


        public static string BuildHeroCardText(SelectedRoom selectedRoom)
        {
            // calculate total price etc etc
            var message = $"Rate: €{selectedRoom.SelectedRate.Price}\n";
            message += selectedRoom.RoomDetailDto.ShortDescription;
            message += " \n";
            message += GetSmokingString(selectedRoom.RoomDetailDto.SmokingAllowed);
            message += GetWheelChairAccessibleString(selectedRoom.RoomDetailDto.WeelChairAccessible);
            message += " \n";
            message += GetCapacityString(2); // add for x number of people
            return message;

        }

        //TODO: update into own responses or add shared resources for emoticons? 
        private static string GetSmokingString(bool smoking)
        {
            if (smoking) return FetchAvailableRoomsStrings.SMOKING_ALLOWED;

            return FetchAvailableRoomsStrings.SMOKING_NOT_ALLOWED;
        }

        private static string GetWheelChairAccessibleString(bool wheelChair)
        {
            if (wheelChair) return FetchAvailableRoomsStrings.WHEELCHAIR_ACCESSIBLE;

            return FetchAvailableRoomsStrings.WHEELCHAIR_INACCESIBLE;
        }

        private static string GetCapacityString(int capacity)
        {
            var mes = "";
            for (var x = 0; x < capacity; x++) mes += "🚹︎";

            return mes;
        }


        public class ResponseIds
        {
            public const string ShowOverview = "showOverview";
            public const string RoomAdded = "roomAdded";
            public const string ConfirmInfo = "confirmInfo";
            public const string ContinueOrAddMoreRooms = "continueOrAddMoreRooms";
            public const string SendPaymentCard = "sendPaymentCard";
        }
    }

}
