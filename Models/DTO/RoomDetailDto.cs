﻿using System;
using System.Collections.Generic;

namespace HotelBot.Models.DTO
{
    public class RoomDetailDto
    {
        // TODO: implement daterange implementation (checkin time between 10 - 11 am)
        public DateTime CheckinTime { get; set; }

        public DateTime CheckoutTime { get; set; }

        public bool SmokingAllowed { get; set; }
        public bool WeelChairAccessible { get; set; }

        public List<RoomImage> RoomImages { get; set; }

        public string ReservationAgreement { get; set; } // Onderstaand treft u de voorwaarden aan die bij annulering van kracht zijn....... 
    }
}
