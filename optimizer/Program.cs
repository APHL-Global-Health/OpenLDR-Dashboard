using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;
using Newtonsoft.Json;
using Optimizer.Logging;
using Optimizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Optimizer
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Logger
            Core.Logger = LogProvider.GetCurrentClassLogger();
            #endregion

            #region Command Line Parser
            CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();
            ValueArgument<string> configArgument = new ValueArgument<string>('c', "config", "Application configuration file");
            configArgument.ValueOptional = false;

            ValueArgument<string> tableArgument = new ValueArgument<string>('t', "table", "Test to filter");
            tableArgument.AllowMultiple = true;

            var dt = DateTime.Now;

            ValueArgument<string> startDateArgument = new ValueArgument<string>('s', "startdate", "Start date");
            startDateArgument.ValueOptional = false;
            startDateArgument.Example = new DateTime(dt.Year, dt.Month, 1).ToString("yyyy-MM-dd");

            ValueArgument<string> endDateArgument = new ValueArgument<string>('e', "enddate", "End date");
            endDateArgument.ValueOptional = false;
            endDateArgument.Example = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month)).ToString("yyyy-MM-dd");

            ValueArgument<bool> autoCloseArgument = new ValueArgument<bool>('a', "autoclose", "Close app automatically");
            autoCloseArgument.ValueOptional = true;
            autoCloseArgument.DefaultValue = true;



            parser.Arguments.Add(configArgument);
            parser.Arguments.Add(tableArgument);
            parser.Arguments.Add(startDateArgument);
            parser.Arguments.Add(endDateArgument);
            parser.Arguments.Add(autoCloseArgument);

            try { parser.ParseCommandLine(args); }
            catch { parser.ShowUsage(); }
            #endregion

            #region Process Data
            if (tableArgument.Values.Any())
            {
                dynamic configuration = null;
                var ConfigurationFile = configArgument.Value;
                if (!string.IsNullOrEmpty(ConfigurationFile))
                {
                    if (File.Exists(ConfigurationFile))
                    {
                        try
                        {
                            var settingsString = File.ReadAllText(ConfigurationFile);
                            if (!string.IsNullOrEmpty(settingsString))
                                configuration = JsonConvert.DeserializeObject(settingsString);
                        }
                        catch (Exception ex) { Core.Logger.Error(string.Format("Error reading configuration file : {0}", ex.Message)); }
                    }
                    else Core.Logger.Error("Invalid configuration file : configuration file cannot be found");
                }
                else Core.Logger.Error("Invalid configuration file : configuration file cannot be empty");

                if(configuration != null)
                {
                    var ConnectionString = configuration["Sql"]["ConnectionString"].Value;
                    var StartDate = startDateArgument.Value;
                    var EndDate = endDateArgument.Value;

                    if (!string.IsNullOrEmpty(ConnectionString))
                    {
                        string connetionStringError = null;
                        try { var builder = new SqlConnectionStringBuilder(ConnectionString); }
                        catch (Exception ex) { connetionStringError = ex.Message; }

                        if (string.IsNullOrEmpty(connetionStringError))
                        {
                            if (!string.IsNullOrEmpty(StartDate) && Regex.IsMatch(StartDate, @"^\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])$"))
                            {
                                if (!string.IsNullOrEmpty(EndDate) && Regex.IsMatch(EndDate, @"^\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])$"))
                                {
                                    foreach (var table in tableArgument.Values)
                                    {
                                        if (table.Trim().Length != 0 && table.Trim().ToLower().Equals("transmission"))
                                        {
                                            Core.Logger.Info(string.Format("Starting to synchronize {0}", table));

                                            string error = null;
                                            var stopWatch = new Stopwatch();
                                            stopWatch.Start();
                                            if (Transmission.Sync(configuration["Api"]["Queries"], StartDate, EndDate, ConnectionString, out error))
                                            {
                                                if (Transmission.Sync(configuration["Api"]["Queries"], StartDate, EndDate, ConnectionString, out error, true))
                                                {
                                                    stopWatch.Stop();
                                                    TimeSpan ts = stopWatch.Elapsed;
                                                    Core.Logger.Info(string.Format("Finished synchronizing {0}, duration {1:00}:{2:00}:{3:00}.{4:00}", table, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
                                                }
                                                else
                                                {
                                                    stopWatch.Stop();
                                                    TimeSpan ts = stopWatch.Elapsed;
                                                    Core.Logger.Error(string.Format("Failed to sync {0}, duration {2:00}:{3:00}:{4:00}.{5:00} : {1}", table, error, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
                                                }
                                            }
                                            else
                                            {
                                                stopWatch.Stop();
                                                TimeSpan ts = stopWatch.Elapsed;
                                                Core.Logger.Error(string.Format("Failed to sync {0}, duration {2:00}:{3:00}:{4:00}.{5:00} : {1}", table, error, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
                                            }
                                        }
                                        else if (table.Trim().Length != 0 && table.Trim().ToLower().Equals("susceptability"))
                                        {
                                            Core.Logger.Info(string.Format("Starting to synchronize {0}", table));

                                            string error = null;
                                            var stopWatch = new Stopwatch();
                                            stopWatch.Start();
                                            if (Susceptability.Sync(configuration["Api"]["Queries"], StartDate, EndDate, ConnectionString, out error))
                                            {
                                                stopWatch.Stop();
                                                TimeSpan ts = stopWatch.Elapsed;
                                                Core.Logger.Info(string.Format("Finished synchronizing {0}, duration {1:00}:{2:00}:{3:00}.{4:00}", table, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
                                            }
                                            else
                                            {
                                                stopWatch.Stop();
                                                TimeSpan ts = stopWatch.Elapsed;
                                                Core.Logger.Error(string.Format("Failed to sync {0}, duration {2:00}:{3:00}:{4:00}.{5:00} : {1}", table, error, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
                                            }

                                            /*if (Susceptability.Sync(configuration["Api"]["Queries"], StartDate, EndDate, ConnectionString, out error, "Resistant"))
                                            {
                                                if (Susceptability.Sync(configuration["Api"]["Queries"], StartDate, EndDate, ConnectionString, out error, "Sensitive"))
                                                {
                                                    if (Susceptability.Sync(configuration["Api"]["Queries"], StartDate, EndDate, ConnectionString, out error, "Intermediate"))
                                                    {
                                                        stopWatch.Stop();
                                                        TimeSpan ts = stopWatch.Elapsed;
                                                        Core.Logger.Info(string.Format("Finished synchronizing {0}, duration {1:00}:{2:00}:{3:00}.{4:00}", table, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
                                                    }
                                                    else
                                                    {
                                                        stopWatch.Stop();
                                                        TimeSpan ts = stopWatch.Elapsed;
                                                        Core.Logger.Error(string.Format("Failed to sync {0}, duration {2:00}:{3:00}:{4:00}.{5:00} : {1}", table, error, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
                                                    }
                                                }
                                                else
                                                {
                                                    stopWatch.Stop();
                                                    TimeSpan ts = stopWatch.Elapsed;
                                                    Core.Logger.Error(string.Format("Failed to sync {0}, duration {2:00}:{3:00}:{4:00}.{5:00} : {1}", table, error, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
                                                }
                                            }
                                            else
                                            {
                                                stopWatch.Stop();
                                                TimeSpan ts = stopWatch.Elapsed;
                                                Core.Logger.Error(string.Format("Failed to sync {0}, duration {2:00}:{3:00}:{4:00}.{5:00} : {1}", table, error, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
                                            }*/
                                        }
                                        else Core.Logger.Error(string.Format("Sync for {0} not yet implemented", table));
                                    }
                                }
                                else Core.Logger.Error("Invalid end date. Use format yyyy-MM-dd");
                            }
                            else Core.Logger.Error("Invalid start date. Use format yyyy-MM-dd");
                        }
                        else Core.Logger.Error("Invalid connection : " + connetionStringError);
                    }
                    else Core.Logger.Error("Invalid connection : connection text cannot be empty");
                }
            }
            else Core.Logger.Error("No table to sync");
            #endregion

            if(!autoCloseArgument.Value) Console.ReadKey();
        }
    }
}
