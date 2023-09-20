using RestSharp.Portable;
using RestSharp.Portable.HttpClient.Impl;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace xUtilityPCL
{
    /*	
	public class RestClientCustom: RestClient
	{


		public RestClientCustom ()
		{

		}

		public RestClientCustom (Uri baseUrl)
			: this ()
		{
			BaseUrl = baseUrl;
		}

		/// <summary>
		/// Constructor that initializes the base URL and some default content handlers
		/// </summary>
		/// <param name="baseUrl">Base URL</param>
		public RestClientCustom (string baseUrl)
			: this (new Uri (baseUrl))
		{

		}

	
	}
*/
    public class HttpClientFactoryCustom : DefaultHttpClientFactory  // DefaultHttpClientFactory
    {
        public override IHttpClient CreateClient(IRestClient client, IRestRequest request)
        {
            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //	var currentPlataform = DependencyService.Get<platformSpecific>();
            //	var session = currentPlataform.getSession();
            //	(session as IHttpClient).BaseAddress = client.BaseUrl;
            //	return session as IHttpClient;
            //}

            var myHttpClient = base.CreateClient(client, request);
            myHttpClient.Timeout = TimeSpan.FromMilliseconds(300000);

            /*20171022
			var myProps = myHttpClient.GetType ().GetRuntimeProperties ();

			foreach (var pi in myProps) {
				if (pi.Name == "Client") {
					var internalc = pi.GetValue (myHttpClient) as HttpClient;
					var ba = internalc.BaseAddress;
					//internalc = xUtilityPCL.Global.myClient;
					xUtilityPCL.Global.myClient.BaseAddress = internalc.BaseAddress;
					//pi.SetValue (myHttpClient, xUtilityPCL.Global.myClient);
					//internalc.BaseAddress = ba;

						*/
            return myHttpClient;

        }




        //20171022 inizio
        public override IHttpRequestMessage CreateRequestMessage(IRestClient client, IRestRequest request, IList<Parameter> parameters)
        {
            //msg.Headers.Clear ();
            //parameters.Clear ();
            //client.DefaultParameters.Clear ();
            //client.Timeout = new TimeSpan (0, 0, 5);
            //client.UserAgent = "";
            //client.ContentHandlers.Clear ();
            return base.CreateRequestMessage(client, request, parameters);
        }

        protected override System.Net.Http.HttpMessageHandler CreateMessageHandler(IRestClient client, IRestRequest request)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var host = client.BaseUrl.Host;
                var isValidIP = Regex.IsMatch(host, @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
                //nota: assumo che l'ip sia sempre valido, la funzione serve x disctiminare se è statico o è un nome risolto da un dominio
                if (isValidIP)
                {
                    var msgHandler = base.CreateMessageHandler(client, request);
                    var currentPlataform = DependencyService.Get<platformSpecific>();
                    var newmsghandler = currentPlataform.getHandler();

                    return newmsghandler as System.Net.Http.HttpMessageHandler;
                }
                else
                {
                    //ha un nome dominio, quindi non cambio con nsurlsession
                    return base.CreateMessageHandler(client, request);
                }
            }
            else
            {
                return base.CreateMessageHandler(client , request);
            }
        }

        //20171022 fine





    }



}

