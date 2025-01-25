using BookingApp.Core.Contracts;
using BookingApp.Models.Bookings;
using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Hotels;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Models.Rooms;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IHotel> hotels;

        public Controller()
        {
            hotels = new HotelRepository();
        }

        public string AddHotel(string hotelName, int category)
        {
            if (hotels.Select(hotelName) != null)
            {
                return $"Hotel {hotelName} is already registered in our platform.";
            }

            IHotel hotel = new Hotel(hotelName, category);
            hotels.AddNew(hotel);
            return $"{category} stars hotel {hotelName} is registered in our platform and expects room availability to be uploaded.";
        }

        public string BookAvailableRoom(int adults, int children, int duration, int category)
        {
            if (hotels.All().Any(h => h.Category == category) == false)
            {
                return $"{category} star hotel is not available in our platform.";
            }

            IEnumerable<IHotel> orderedHotels = hotels.All()
                .Where(h => h.Category == category)
                .OrderBy(h => h.FullName);

            foreach (IHotel hotel in orderedHotels)
            {
                IRoom roomToAccommodate = hotel.Rooms.All()
                    .Where(r => r.PricePerNight > 0 && r.BedCapacity >= adults + children)
                    .OrderBy(r => r.BedCapacity)
                    .FirstOrDefault();

                if (roomToAccommodate != null)
                {
                    int bookingNumber = hotel.Bookings.All().Count + 1;
                    IBooking booking = new Booking(roomToAccommodate, duration, adults, children, bookingNumber);
                    hotel.Bookings.AddNew(booking);
                    return $"Booking number {bookingNumber} for {hotel.FullName} hotel is successful!";
                }
            }

            return "We cannot offer appropriate room for your request.";
        }

        public string HotelReport(string hotelName)
        {
            if (hotels.Select(hotelName) == null)
            {
                return $"Profile {hotelName} doesn't exist!";
            }

            IHotel hotel = hotels.Select(hotelName);

            StringBuilder builder = new();
            builder.AppendLine($"Hotel name: {hotelName}");
            builder.AppendLine($"--{hotel.Category} star hotel");
            builder.AppendLine($"--Turnover: {hotel.Turnover:F2} $");
            builder.AppendLine("--Bookings:");
            builder.AppendLine();

            if (hotel.Bookings.All().Any() == false)
            {
                builder.AppendLine("none");
            }
            else
            {
                foreach (IBooking booking in hotel.Bookings.All())
                {
                    builder.AppendLine(booking.BookingSummary());
                    builder.AppendLine();
                }
            }

            return builder.ToString().TrimEnd();
        }

        public string SetRoomPrices(string hotelName, string roomTypeName, double price)
        {
            if (hotels.Select(hotelName) == null)
            {
                return $"Profile {hotelName} doesn't exist!";
            }

            if (roomTypeName != nameof(Apartment) &&
                roomTypeName != nameof(DoubleBed) &&
                roomTypeName != nameof(Studio))
            {
                throw new ArgumentException("Incorrect room type!");
            }

            IHotel hotel = hotels.Select(hotelName);

            if (hotel.Rooms.Select(roomTypeName) == null)
            {
                return "Room type is not created yet!";
            }

            IRoom room = hotel.Rooms.Select(roomTypeName);

            if (room.PricePerNight > 0)
            {
                throw new InvalidOperationException("Price is already set!");
            }
            else
            {
                room.SetPrice(price);
                return $"Price of {roomTypeName} room type in {hotelName} hotel is set!";
            }
        }

        public string UploadRoomTypes(string hotelName, string roomTypeName)
        {
            if (hotels.Select(hotelName) == null)
            {
                return $"Profile {hotelName} doesn’t exist!";
            }

            IHotel hotel = hotels.Select(hotelName);

            if (hotel.Rooms.Select(roomTypeName) != null)
            {
                return "Room type is already created!";
            }

            if (roomTypeName != nameof(Apartment) &&
                roomTypeName != nameof(DoubleBed) &&
                roomTypeName != nameof(Studio))
            {
                throw new ArgumentException("Incorrect room type!");
            }

            IRoom room = null;

            if (roomTypeName == nameof(Apartment))
            {
                room = new Apartment();
            }
            else if (roomTypeName == nameof(DoubleBed))
            {
                room = new DoubleBed();
            }
            else
            {
                room = new Studio();
            }

            hotel.Rooms.AddNew(room);
            return $"Successfully added {roomTypeName} room type in {hotelName} hotel!";
        }
    }
}
