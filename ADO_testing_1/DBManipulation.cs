﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_testing_1
{

    /// <summary>
    /// Класс, проводящий манипуляции с БД приложения (ado_test)
    /// </summary>
    static class DBManipulation
    {
        // строка подключения, подключение указано в App.config
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Выводит в консоль информацию о подключении
        /// </summary>
        public static void GetConnectionInfo()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Подключение открыто");

                // Вывод информации о подключении
                Console.WriteLine("Свойства подключения:");
                Console.WriteLine("\tСтрока подключения: {0}", connection.ConnectionString);
                Console.WriteLine("\tБаза данных: {0}", connection.Database);
                Console.WriteLine("\tСервер: {0}", connection.DataSource);
                Console.WriteLine("\tВерсия сервера: {0}", connection.ServerVersion);
                Console.WriteLine("\tСостояние: {0}", connection.State);
                Console.WriteLine("\tWorkstationld: {0}", connection.WorkstationId);
            }

            Console.WriteLine("Подключение закрыто... Нажать Enter");

            Console.Read();
        }

        #region Inserts

        /// <summary>
        /// Вносит в базу информацию из AudioFile
        /// </summary>
        /// <param name="auFile"></param>
        public static void InsertTrack(AudioFile auFile)
        {
            // название процедуры
            string sqlExpression = "sp_InsertTrack";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;

                // параметр для ввода album
                SqlParameter albumParam = new SqlParameter
                {
                    ParameterName = "@album",
                    Value = auFile.album
                };
                command.Parameters.Add(albumParam);

                // параметр для ввода artist
                SqlParameter artistParam = new SqlParameter
                {
                    ParameterName = "@artist",
                    Value = auFile.artist
                };
                command.Parameters.Add(artistParam);

                // параметр для ввода title
                SqlParameter titleParam = new SqlParameter
                {
                    ParameterName = "@title",
                    Value = auFile.title
                };
                command.Parameters.Add(titleParam);

                // параметр для ввода genre
                SqlParameter genreParam = new SqlParameter
                {
                    ParameterName = "@genre",
                    Value = auFile.genre
                };
                command.Parameters.Add(genreParam);

                // параметр для ввода year
                SqlParameter yearParam = new SqlParameter
                {
                    ParameterName = "@year",
                    Value = auFile.year
                };
                command.Parameters.Add(yearParam);

                // параметр для ввода trackNumber
                SqlParameter trackNumberParam = new SqlParameter
                {
                    ParameterName = "@trackNumber",
                    Value = Convert.ToInt32(auFile.trackNumber)
                };
                command.Parameters.Add(trackNumberParam);

                command.ExecuteNonQuery();
            }
        }

        public static void AlbumsTrackCountSP()
        {
            // название процедуры
            string sqlExpression = "sp_AlbumsTrackCount";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
        }

        public static void ArtistsAlbumsCountSP()
        {
            // название процедуры
            string sqlExpression = "sp_ArtistsAlbumsCount";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Selects

        /// <summary>
        /// Выводит в консоль информацию о треке из БД
        /// == Не доработан ==
        /// </summary>
        public static void GetTrackInfo()
        {
            // название процедуры
            string sqlExpression = "sp_TrackInfo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4), reader.GetName(5), reader.GetName(6), reader.GetName(7));
                    Console.WriteLine("-----------------------------------------");
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int trackNum = reader.GetInt32(1);
                        string trackName = reader.GetString(2);
                        string artist = reader.GetString(3);
                        string album = reader.GetString(4);
                        int year = reader.GetInt32(5);
                        string genre = reader.GetString(6);
                        string duration = reader.GetString(7);
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}", id, trackNum, trackName, artist, album, year, genre, duration);
                    }
                }
                reader.Close();
            }
        }

        /// <summary>
        /// Выводит в консоль имя исполнителя и его страну
        /// == пока что бесполезный, исполнителям не присвоены страны ==
        /// </summary>
        public static void GetArtistsWithCountries()
        {
            // название процедуры
            string sqlExpression = "sp_GetArtistsWithCountries";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1} \t\t\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));
                    Console.WriteLine("-----------------------------------------");
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string artist = reader.GetString(1);
                        string country = reader.GetString(2);
                        Console.WriteLine("{0}\t{1} \t\t{2}", id, artist, country);
                    }
                }
                reader.Close();
            }
        }
        #endregion
    }
}
