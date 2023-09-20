using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace test1Xamrin
{
    public class SabicomRepeatBOTService
    {
        private static Action<string> onReceiveMessage;
        internal void AttachOnReceiveMessage(Action<string> onMessageReceived)
        {
            onReceiveMessage = onMessageReceived;

        }
        internal async void SendToBot(string text)
        {
            var botreply = new BotMessage();
            //if(text.StartsWith("IMG-Path:")){
            //    botreply = await SendMessage3("alessandro", text.TrimStart("IMG-Path:".ToCharArray()), mystream);
            //}
            //else
            botreply = await SendMessage2("alessandro", text);

            //20181230onReceiveMessage?.Invoke(botreply.Text);

            //Task.Delay(500).ContinueWith(t => this.onReceiveMessage?.Invoke(text));
        }

        public static async Task<BotMessage> SendMessage2(string name, string message)
        {

            var messageToSend = new BotMessage() { From = name, Text = message };
            var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
            var conversationUrl = SabicomChatMain.URL + "api/conversations/" + SabicomChatMain.conversationId + "/messages/";

            //if (message == App.K_ChatChiusa)
            //return null;

            if (message == "start")
            {
                //get
                var messagesReceived = await SabicomChatMain._httpClient.GetAsync(conversationUrl);
                var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
                var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
                var messages = messagesRoot.Messages;
                if (messages.Count == 0)
                {
                    //post + get
                    var response = await SabicomChatMain._httpClient.PostAsync(conversationUrl, contentPost);
                    var messagesReceived1 = await SabicomChatMain._httpClient.GetAsync(conversationUrl);
                    var messagesReceivedData1 = await messagesReceived1.Content.ReadAsStringAsync();
                    var messagesRoot1 = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData1);
                    var messages1 = messagesRoot1.Messages;
                    if (messages1.Count == 0)
                        return null;
                    else
                        return messages1.First();
                }

                else
                {
                    if (messages.Count == 0)
                        return null;
                    else
                        return messages.First();  //non dovrebbe passare mai in quanto c'e sempre un nuovo conversationid  
                }

            }

            else
            {
                //post + get
                var response = await SabicomChatMain._httpClient.PostAsync(conversationUrl, contentPost);
                var messagesReceived = await SabicomChatMain._httpClient.GetAsync(conversationUrl);
                var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
                var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
                var messages = messagesRoot.Messages;
                if (messages.Count == 0)
                    return null;
                else
                {
                    var botReply = messages.Last();
                    onReceiveMessage?.Invoke(botReply.Text);
                    return botReply;
                    //20181230return messages.Last();
                }
            }
        }




        public static async Task<BotMessage> SendMessage3(string name, string message, StreamContent mystream)
        {

            var messageToSend = new BotMessage() { From = name, Text = message };
            var att = new Attachment()
            {
                Url = message,
                ContentType = "jpeg"

            };
            List<Attachment> latt = new List<Attachment>();
            messageToSend.Attachments = latt.ToArray();

            //var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
            var conversationUrl = SabicomChatMain.URL + "api/conversations/" + SabicomChatMain.conversationId + "/upload?userId=alessandro";


            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent("fileToUpload");
            //form.Add(content, "fileToUpload");


            //var stream = await file.OpenStreamForReadAsync();
            //content = new StreamContent(stream);
            content = mystream;
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "fileToUpload",
                FileName = "Test Image"
            };

            content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            form.Add(content);


            try
            {


                var response = await SabicomChatMain._httpClient.PostAsync(conversationUrl, form);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //var b = 0;
            }
            conversationUrl = SabicomChatMain.URL + "api/conversations/" + SabicomChatMain.conversationId + "/messages/";

            var messagesReceived = await SabicomChatMain._httpClient.GetAsync(conversationUrl);
            var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
            var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
            var messages = messagesRoot.Messages;
            var botReply = messages.Last();
            onReceiveMessage?.Invoke(botReply.Text);
            return botReply;

        }

        public static async Task<BotMessage> SendMessage4(string name, string message, StreamContent mystream) //not used
        {

            var messageToSend = new BotMessage() { From = name, Text = message };

            List<Attachment> latt = new List<Attachment>();
            messageToSend.Attachments = latt.ToArray();

            //var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
            var conversationUrl = SabicomChatMain.URL + "api/conversations/" + SabicomChatMain.conversationId + "/upload?userId=alessandro";


            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent("fileToUpload");
            //form.Add(content, "fileToUpload");


            //var stream = await file.OpenStreamForReadAsync();
            //content = new StreamContent(stream);
            content = mystream;
            //content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            //{
            //    Name = "fileToUpload",
            //    FileName = "Test Image"
            //};

            content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            form.Add(content);

            HttpResponseMessage response;

            try
            {

                response = await SabicomChatMain._httpClient.PostAsync(conversationUrl, form);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //var b = 0;
            }
            conversationUrl = SabicomChatMain.URL + "api/conversations/" + SabicomChatMain.conversationId + "/messages/";

            var messagesReceived = await SabicomChatMain._httpClient.GetAsync(conversationUrl);
            var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
            var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
            var messages = messagesRoot.Messages;
            var botReply = messages.Last();
            onReceiveMessage?.Invoke(botReply.Text);
            return botReply;

        }


    }
}
