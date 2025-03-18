public interface IFlightService
{
    IList<string> GetAllFlightsInfo();
    IList<string> GetCustomersByFlight(int flightId);
    IList<string> GetFlightsByCityAndDate(string departureCity, DateTime date);
    IList<Tuple<string, int?>> GetTop5FlightsByBookings();
    IList<string> GetFlightsWithMaxBookings();
    (int? Min, double? Average, int? Max) GetBookingStatisticsByCity(string departureCity);
}