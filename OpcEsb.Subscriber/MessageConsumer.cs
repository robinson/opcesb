// Copyright 2016 lth
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

namespace OpcEsb.Subscriber
{
    using MassTransit;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using OpcEsb.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    public class MessageConsumer : IConsumer<Activity>
	{
		public Task Consume(ConsumeContext<Activity> context)
		{
			var jsonStr = context.Message.Message;
			MessageData newElement = ParseJsonObject<MessageData>(jsonStr);
			foreach (var notiItem in newElement.NotificationData)
			{
				foreach (var moniItem in notiItem.MonitoredItems)
				{
					Console.Write("Value: " + moniItem.Value.Value);
					Console.Write("  SENT: " + context.Message.Happened);
					Console.WriteLine(" (" + System.Threading.Thread.CurrentThread.ManagedThreadId + ")");
				}
			}
			
			return Task.FromResult(0);
		}
		static T ParseJsonObject<T>(string json) where T : class, new()
		{
			try
			{
				JObject jobject = JObject.Parse(json);
				return JsonConvert.DeserializeObject<T>(jobject.ToString());
			}
			catch (Exception ex)
			{
				Trace.TraceError("Exception parsing json message '{0}'", ex.Message);
				throw ex;
			}
		}
	}
	/*
	 *sample message { 
	"MonitoredItem ":{ 
		"Id ": "i=2258 ", 
		"Uri ": "urn:localhost:UA Sample Server "
	}, 
	"SequenceNumber ":39,
	"PublishTime ": "2016-07-08T13:10:21.4218586Z ", 
	"NotificationData ":[{ 
		"MonitoredItems ":[{ 
				"ClientHandle ":4, 
				"Value ":{
					"Value":"2016-07-08T13:10:21.3638586Z",
					"SourceTimestamp":"2016-07-08T13:10:21.3638586Z ", 
					"ServerTimestamp ": "2016-07-08T13:10:21.3638586Z "
				}
		}], 
		"DiagnosticInfos ":[null]}]}
	 */
	class MessageData
	{
		[JsonProperty("MonitoredItem")]
		public MonitoredItem MonitoredItem { get; set; }
		[JsonProperty("SequenceNumber")]
		public int SequenceNumber { get; set; }
		[JsonProperty("PublishTime")]
		public DateTime PublishTime { get; set; }
		[JsonProperty("NotificationData")]
		public List<NotificationData> NotificationData { get; set; }

	}
	class MonitoredItem
	{

		[JsonProperty("Id")]
		public string Id { get; set; }

		[JsonProperty("Uri")]
		public string Uri { get; set; }

	}
	class Values
	{

		[JsonProperty("Value")]
		public string Value { get; set; }

		[JsonProperty("SourceTimestamp")]
		public string SourceTimestamp { get; set; }

		[JsonProperty("ServerTimestamp")]
		public string ServerTimestamp { get; set; }
	}
	
	class NotificationData
	{
		[JsonProperty("MonitoredItems")]
		public List<Monitored> MonitoredItems { get; set; }
		[JsonProperty("DiagnosticInfos")]
		public List<string> DiagnosticInfos { get; set; }
	}
	class Monitored
	{

		[JsonProperty("ClientHandle")]
		public int ClientHandle { get; set; }
		[JsonProperty("Value")]
		public Values Value { get; set; }

	}

}
