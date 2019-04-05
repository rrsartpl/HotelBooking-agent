﻿using System.Collections.Generic;
using HotelBot.Models.LUIS;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace HotelBot.Dialogs.FetchAvailableRooms
{
    public class FetchAvailableRoomsState
    {

        public string Email { get; set; }
        public double? NumberOfPeople { get; set; }
        public TimexProperty ArrivalDate { get; set; }
        public TimexProperty LeavingDate { get; set; }

        public TimexProperty TempTimexProperty { get; set; } // used in delegates --> stores this in arrival/leaving



    }
}