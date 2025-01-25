using BookingApp.Models.Bookings.Contracts;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class BookingRepository : IRepository<IBooking>
    {
        private readonly List<IBooking> bookings;

        public BookingRepository()
        {
            bookings = new List<IBooking>();
        }

        public void AddNew(IBooking booking)
        {
            bookings.Add(booking);
        }

        public IReadOnlyCollection<IBooking> All()
        {
            return bookings.AsReadOnly();
        }

        public IBooking Select(string bookingNumberToString)
        {
            if (int.TryParse(bookingNumberToString, out int bookingNumber))
            {
                IBooking booking = bookings.FirstOrDefault(b => b.BookingNumber == bookingNumber);
                return booking;
            }
            else
            {
                return null;
            }
        }
    }
}
