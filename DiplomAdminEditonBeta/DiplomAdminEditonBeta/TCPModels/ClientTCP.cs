using Newtonsoft.Json;
using SimpleTCP;
using System;
using System.Text;

namespace DiplomAdminEditonBeta.TCPModels
{
    public class ClientTCP
    {
        private static SimpleTcpClient client = new SimpleTcpClient();
        public static bool IsConnected = false;

        public static void Connect()
        {
            client.Connect("192.168.1.4", 5000);
        }

        public static void Disconnect()
        {
            client.Disconnect();
        }

        public static TCPMessege SendMessegeAndGetAnswer(TCPMessege tCPMessege)
        {
            try
            {
                var eM = client.WriteLineAndGetReply(JsonConvert.SerializeObject(tCPMessege), new TimeSpan(0, 0, 15));
                if(eM == null)
                {
                    IsConnected = false;
                    return null;
                }
                var msg = Encoding.UTF8.GetString(eM.Data);
                return JsonConvert.DeserializeObject<TCPMessege>(msg);
            }
            catch {
                IsConnected = false;
                return null;
            }
        }

        public static bool SendMessege(TCPMessege tCPMessege)
        {
            try
            {
                client.WriteLine(JsonConvert.SerializeObject(tCPMessege));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
