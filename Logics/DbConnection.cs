using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Polly;
using MongoDB.Driver;
using StackExchange.Redis;
using ServiceMonitor.Web.Models;
namespace ServiceMonitor.Web.Logics
{
    public class DbConnection
    {

        public bool CheckMongoDbConnection(TaskConfig taskConfig)
        {
            try
            {
                return Policy.Timeout<bool>(TimeSpan.FromSeconds(taskConfig.Timeout)).Execute(() =>
                {
                    using (var client = new MongoClient(taskConfig.Data))
                    {
                        return client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Connected;
                    }
                });
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Connection Redis
        /// </summary>
        /// <param name="taskConfig"></param>
        /// <returns></returns>
        public bool CheckRedisConnection(TaskConfig taskConfig)
        {
            ConnectionMultiplexer? redis = null;
            try
            {
                return Policy.Timeout<bool>(TimeSpan.FromSeconds(taskConfig.Timeout)).Execute(() =>
                {
                    redis = ConnectionMultiplexer.Connect(taskConfig.Data);
                    return redis.IsConnected;
                });
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                redis?.Close();
                redis?.Dispose();
            }
        }

        /// <summary>
        /// Connection MySql
        /// </summary>
        /// <param name="taskConfig"></param>
        /// <returns></returns>
        public bool CheckMySqlConnection(TaskConfig taskConfig)
        {
            try
            {
                return Policy.Timeout<bool>(TimeSpan.FromSeconds(taskConfig.Timeout)).Execute(() =>
                {
                    var connection = new MySqlConnection(taskConfig.Data);
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return true;
                });
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Connection MSSQL
        /// </summary>
        /// <param name="taskConfig"></param>
        /// <returns></returns>
        public bool CheckMsSqlConnection(TaskConfig taskConfig)
        {
            try
            {
                return Policy.Timeout<bool>(TimeSpan.FromSeconds(taskConfig.Timeout)).Execute(() =>
                {
                    var connection = new SqlConnection(taskConfig.Data);
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return true;
                });
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Connection PostgreSql
        /// </summary>
        /// <param name="taskConfig"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CheckPostgreSqlConnection(TaskConfig taskConfig)
        {
            try
            {
                return Policy.Timeout<bool>(TimeSpan.FromSeconds(taskConfig.Timeout)).Execute(() =>
                {
                    using (var connection = new Npgsql.NpgsqlConnection(taskConfig.Data))
                    {
                        connection.Open();
                        return true;
                    }
                });
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Connection Oracle
        /// </summary>
        /// <param name="taskConfig"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CheckOracleConnection(TaskConfig taskConfig)
        {
            try
            {
                return Policy.Timeout<bool>(TimeSpan.FromSeconds(taskConfig.Timeout)).Execute(() =>
                {
                    using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(taskConfig.Data))
                    {
                        connection.Open();
                        return true;
                    }
                });
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
