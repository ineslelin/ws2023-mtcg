﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Res;

namespace ws2023_mtcg.Server.Repository
{
    internal class TradingRepository
    {
        private readonly string _connectionString = "Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true";

        public TradingDeal Read(string id)
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"SELECT * FROM trading WHERE id=@id";

                        DbCommands.AddParameterWithValue(command, "id", DbType.String, id);
                        command.ExecuteNonQuery();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<TradingDeal> trades = new List<TradingDeal>();

                            if(reader.Read())
                            {
                                return new TradingDeal()
                                {
                                    Id = id,
                                    CardToTrade = reader.GetString(1),
                                    Type = (CardType)reader.GetInt32(3),
                                    MinimumDamage = reader.GetInt32(4),
                                };
                            }

                            return new TradingDeal() { Id = null };
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }

            return new TradingDeal() { Id = null };
        }


        public void Create(TradingDeal deal, string username)
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"INSERT INTO trading (id, cardid, username, cardtype, damage)
                                                VALUES (@id, @cardid, @username, @cardtype, @damage)";

                        DbCommands.AddParameterWithValue(command, "id", DbType.String, deal.Id);
                        DbCommands.AddParameterWithValue(command, "cardid", DbType.String, deal.CardToTrade);
                        DbCommands.AddParameterWithValue(command, "username", DbType.String, username);
                        DbCommands.AddParameterWithValue(command, "cardtype", DbType.Int32, (int)deal.Type);
                        DbCommands.AddParameterWithValue(command, "damage", DbType.Double, deal.MinimumDamage);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }
        }

        public void Update(Cards card)
        {

        }

        public void Delete(string username)
        {
            if (username == null)
                throw new ArgumentException("id can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"DELETE FROM deck WHERE owner=@owner";

                        DbCommands.AddParameterWithValue(command, "owner", DbType.String, username);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }
        }
        public TradingDeal[] AllTradingDeals()
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"SELECT * FROM trading";
                        command.ExecuteNonQuery();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<TradingDeal> trades = new List<TradingDeal>();

                            while (reader.Read())
                            {
                                TradingDeal deal = new TradingDeal()
                                {
                                    Id = reader.GetString(0),
                                    CardToTrade = reader.GetString(1),
                                    Type = (CardType)reader.GetInt32(3),
                                    MinimumDamage = reader.GetInt32(4),
                                };

                                trades.Add(deal);
                            }

                            return trades.ToArray();
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }

            return null;
        }
    }
}
