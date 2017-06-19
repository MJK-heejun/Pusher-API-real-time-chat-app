using PusherServer;
using System.Web.Mvc;
using System.Net;
using System.Web.Script.Serialization;
using MyChat.Dto;
using System.Collections.Generic;
using System.Data.SqlClient;
using MyChat.Helper;
using System.Data;
using System.Linq;
using System.Configuration;
namespace MyChat.Controller
{
    public class MainController
    {

        Pusher pusher;
        private string MESSAGE_EVENT = "message_event";

        private string APP_KEY = ConfigurationManager.AppSettings["PusherAppKey"];
        private string APP_SECRET = ConfigurationManager.AppSettings["PusherAppSecret"];
        private string APP_ID = ConfigurationManager.AppSettings["PusherAppId"];


        public MainController()
        {
            var options = new PusherOptions();
            options.Encrypted = true;
            pusher = new Pusher(APP_KEY, APP_SECRET, APP_ID, options);
        }

        
        public string GetPreviousMessage(string channel)
        {
            string sqlQuery = @"SELECT t1.username as username, t1.message as message, t2.name as channel
                                FROM chat_log as t1 INNER JOIN channel as t2 ON t1.channel_id = t2.id 
                                WHERE t2.name = @channelName";
            SqlParameter[] parameters = {
                new SqlParameter("@channelName", SqlDbType.VarChar) { Value = channel}
            };

            DataSet ds = DBHelper.Query(sqlQuery, parameters);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<MessageDto> result = ds.Tables[0].AsEnumerable().Select(r => new MessageDto
                {
                    channel = r.Field<string>("channel"),
                    username = r.Field<string>("username"),
                    message = r.Field<string>("message")
                }).ToList();

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                return serialize.Serialize(result);
            }else
            {
                return "[]";
            }
        }


        //return list of channels from sql database
        public List<string> GetChannelList()
        {
            List<string> channelList = new List<string>();

            DataSet ds = DBHelper.Query("SELECT * FROM channel");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string channel = row["name"].ToString();
                    channelList.Add(channel);
                }
            }

            return channelList;

        }

        //add new channel to sql database
        public void AddNewChannel(string newChannelName)
        {
            string sqlQuery = @"INSERT INTO channel (name, status) VALUES (@channelName, 1)";

            SqlParameter[] parameters = {
                new SqlParameter("@channelName", SqlDbType.VarChar) { Value = newChannelName}
            };

            DBHelper.Query(sqlQuery,parameters);
        }


        [HttpPost]
        public ActionResult SendMessage(string data)
        {
            //send event data
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            MessageDto messageObj = serialize.Deserialize<MessageDto>(data);
            var result = pusher.Trigger(
                messageObj.channel, 
                MESSAGE_EVENT, 
                new {
                    message = messageObj.message,
                    username = messageObj.username
                });
            
            //log into sql database
            string sqlQuery = @"INSERT INTO chat_log 
                (channel_id, username, message) VALUES 
                ((SELECT id from channel WHERE name=@channelName), @username, @message)";

            SqlParameter[] parameters = {
                new SqlParameter("@channelName", SqlDbType.VarChar) { Value = messageObj.channel},
                new SqlParameter("@username", SqlDbType.VarChar) { Value = messageObj.username},
                new SqlParameter("@message", SqlDbType.VarChar) { Value = messageObj.message},
            };
            DBHelper.Query(sqlQuery, parameters);


            return new HttpStatusCodeResult((int)HttpStatusCode.OK);
        }


    }
}