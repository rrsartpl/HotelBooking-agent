using System;
using System.Collections.Generic;

namespace HotelBot.Models.DTO
{
    [Serializable]
    public class RoomDetailDto
    {

        public string Id { get; set; }

        // TODO: implement daterange implementation (checkin time between 10 - 11 am)
        public DateTime CheckinTime { get; set; }
        public DateTime CheckoutTime { get; set; }


        public string Title { get; set; }


        public string ShortDescription { get; set; }

        public int SquareFeet { get; set; }
        public string BedDescription { get; set; }

        public int LowestRate { get; set; }
        public string Description { get; set; }


        public bool SmokingAllowed { get; set; }
        public bool WeelChairAccessible { get; set; }
        public List<RoomImage> RoomImages { get; set; }

        public List<RoomRate> Rates { get; set; }

        public int Capacity { get; set; }

        public string ReservationAgreement { get; set; } // Onderstaand treft u de voorwaarden aan die bij annulering van kracht zijn....... 
    }
}
    