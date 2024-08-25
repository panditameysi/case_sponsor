using Npgsql;
using SponsorAPI.Models;
using System.Data;
using System.Numerics;
using System.Text.RegularExpressions;

namespace SponsorAPI.DAO
{
    public class SponsorDaoImplementation : ISponsorDao
    {
        NpgsqlConnection _connection;
        public SponsorDaoImplementation(NpgsqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<List<SponsorsWithPaymentDetailsView>> GetSponsorsWithPaymentDetails()
        {
            List<SponsorsWithPaymentDetailsView> sponsors = new List<SponsorsWithPaymentDetailsView>();
            string query = "select * from SponsorsWithPaymentDetails";
            string err = string.Empty;
            SponsorsWithPaymentDetailsView s = null;
            try
            {
                await _connection.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows) {
                    while (reader.Read())
                    {
                        var sponsor = new SponsorsWithPaymentDetailsView
                        {
                            SponsorID = reader.GetInt32("SponsorID"),
                            SponsorName = reader.GetString("SponsorName"),
                            IndustryType = reader.GetString("IndustryType"),
                            ContactEmail = reader.GetString("ContactEmail"),
                            Phone = reader.GetString("Phone"),
                            TotalPayments = reader.GetDouble("TotalPayments"),
                            NumberOfPayments = reader.GetInt32("NumberOfPayments"),
                            LatestPaymentDate = DateOnly.FromDateTime(reader.GetDateTime("LatestPaymentDate"))
                        };
                        sponsors.Add(sponsor);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Console.WriteLine(err);
            }
            return sponsors;
        }

        public async Task<List<MatchesWithTotalPaymentsView>> GetMatchesWithTotalPayments()
        {
            List<MatchesWithTotalPaymentsView> sponsors = new List<MatchesWithTotalPaymentsView>();
            string query = "select * from MatchesWithTotalPayments";
            string err = string.Empty;
            try
            {
                await _connection.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var sponsor = new MatchesWithTotalPaymentsView
                        {
                            MatchID = reader.GetInt32("MatchID"),
                            MatchName = reader.GetString("MatchName"),
                            MatchDate = DateOnly.FromDateTime(reader.GetDateTime("MatchDate")),
                            Location = reader.GetString("Location"),
                            TotalPayments = reader.GetDouble("TotalPayments"),
                        };
                        sponsors.Add(sponsor);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Console.WriteLine(err);
            }
            return sponsors;
        }

        public async Task<List<SponsorsByMatchCountAsPerYearView>> GetSponsorsByMatchCountAsPerYear(int year)
        {
            List<SponsorsByMatchCountAsPerYearView> sponsors = new List<SponsorsByMatchCountAsPerYearView>();
            string query = "SELECT SponsorID, SponsorName, CAST(COUNT(MatchCount) AS INT) AS MatchCount FROM SponsorsByMatchCountAsPerYear WHERE date_part('year',MatchDate)=" + year+" GROUP BY SponsorID, SponsorName;";
            Console.WriteLine(query);
            string err = string.Empty;
            try
            {
                await _connection.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var sponsor = new SponsorsByMatchCountAsPerYearView
                        {
                            SponsorID = reader.GetInt32("SponsorID"),
                            SponsorName = reader.GetString("SponsorName"),
                            MatchCount = reader.GetInt32("MatchCount"),
                        };
                        sponsors.Add(sponsor);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Console.WriteLine(err);
            }
            return sponsors;
        }

        async Task<bool> PaymentExists(Payment payment)
        {

            await _connection.OpenAsync();

            var query = @"
                SELECT COUNT(1) 
                FROM sponsor.Payments 
                WHERE ContractID = @ContractID AND PaymentDate = @PaymentDate AND AmountPaid = @AmountPaid";

                NpgsqlCommand command = new NpgsqlCommand(query, _connection);

                command.Parameters.AddWithValue("@ContractID", payment.ContractID);
                command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                command.Parameters.AddWithValue("@AmountPaid", payment.AmountPaid);

                var exists = (long)await command.ExecuteScalarAsync();
                _connection.Close();
                return exists > 0;
            
        }

        public async Task<int> AddPayment(Payment payment)
        {
            var exists = await PaymentExists(payment);
            if (exists)
            {
                return 0;
            }
            else
            {
                string query = @"INSERT INTO sponsor.Payments (ContractID, PaymentDate, AmountPaid, PaymentStatus)
                VALUES (@ContractID, @PaymentDate, @AmountPaid, @PaymentStatus)
                RETURNING PaymentID;";
                string errMessage = string.Empty;
                try
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                    command.Parameters.AddWithValue("@ContractID", payment.ContractID);
                    command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                    command.Parameters.AddWithValue("@AmountPaid", payment.AmountPaid);
                    command.Parameters.AddWithValue("@PaymentStatus", payment.PaymentStatus);
                    command.CommandType = CommandType.Text;
                    await command.ExecuteNonQueryAsync();
                    _connection.Close();
                }
                catch (NpgsqlException e)
                {
                    errMessage = e.Message;
                    Console.WriteLine("------Exception-----:" + errMessage);
                    return 0;
                }
                return 1;
            }
        }
    
    }
}
