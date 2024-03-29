﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server
{
    internal class DatabaseHandler
    {
        private static readonly string _connectionString = "Host=localhost;Database=mtcgdb;Username=admin;Password=1234";

        public static void Start()
        {
            Console.WriteLine("Creating tables...");

            CreateUsersTable();
            CreateCardsTable();
            CreateStackTable();
            CreateDeckTable();
            CreateUserProfileTable();
            CreateTradingTable();

            Console.WriteLine("Finished creating tables...");
        }

        private static void CreateUsersTable()
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE IF NOT EXISTS users (
	                                                id SERIAL PRIMARY KEY,
	                                                username VARCHAR(20) UNIQUE NOT NULL,
	                                                password VARCHAR(255) NOT NULL,
	                                                coins int,
	                                                elo int,
	                                                wins int,
	                                                losses int
                                                );";

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }

            Console.WriteLine("Created users table...");
        }

        private static void CreateCardsTable()
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE IF NOT EXISTS cards (
	                                                id varchar(128) UNIQUE PRIMARY KEY,
	                                                name varchar(32),
	                                                damage double precision,
	                                                element int,
	                                                cardtype int,
	                                                package int
                                                );";

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }

            Console.WriteLine("Created cards table...");
        }

        private static void CreateStackTable()
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE IF NOT EXISTS stack (
                                                    id varchar(128) UNIQUE PRIMARY KEY,
	                                                name varchar(32),
	                                                damage double precision,
	                                                element int,
	                                                cardtype int,
	                                                owner varchar(20) REFERENCES users(username)
                                                );";

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }

            Console.WriteLine("Created stack table...");
        }

        private static void CreateDeckTable()
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE IF NOT EXISTS deck (
                                                    id varchar(128) references stack(id),
	                                                owner varchar(20)
                                                );";

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }

            Console.WriteLine("Created deck table...");
        }

        private static void CreateUserProfileTable()
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE IF NOT EXISTS userprofile (
	                                                username varchar(20) PRIMARY KEY REFERENCES users(username),
	                                                displayname varchar(50),
	                                                bio varchar(250),
	                                                image varchar(50)
                                                );";

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }

            Console.WriteLine("Created userprofile table...");
        }

        private static void CreateTradingTable()
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE IF NOT EXISTS trading (
	                                                id varchar(128) UNIQUE PRIMARY KEY,
	                                                cardid varchar(128) UNIQUE references stack(id),
	                                                username varchar(20),
	                                                cardtype int,
	                                                damage int
                                                );";

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }

            Console.WriteLine("Created trading table...");
        }
    }
}
