﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Models.Rooms
{
    public class Apartment : Room
    {
        private const int ApartmentCapacity = 6;

        public Apartment()
            : base(ApartmentCapacity)
        {
        }
    }
}
