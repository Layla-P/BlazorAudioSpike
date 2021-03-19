using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioSpikeBlazorServer
{

	public class Rootobject
	{
		public Audioentity audioEntity { get; set; }
		public Transcriptionresponse transcriptionResponse { get; set; }
		public int generalStatusEnum { get; set; }
	}

	public class Audioentity
	{
		public string fileName { get; set; }
		public string processStatusEnum { get; set; }
		public object processedUrl { get; set; }
		public string partitionKey { get; set; }
		public string rowKey { get; set; }
		public DateTime timestamp { get; set; }
		public string eTag { get; set; }
	}

	public class Transcriptionresponse
	{
		public string id { get; set; }
		public string status { get; set; }
		public int acousticModel { get; set; }
		public float audioDuration { get; set; }
		public string audioUrl { get; set; }
		public float confidence { get; set; }
		public bool dualChannel { get; set; }
		public bool formatText { get; set; }
		public int languageModel { get; set; }
		public bool punctuate { get; set; }
		public string text { get; set; }
		public object utterances { get; set; }
		public object webhookStatusCode { get; set; }
		public object webhookUrl { get; set; }
		public Word[] words { get; set; }
		public object autoHighlightsResult { get; set; }
	}

	public class Word
	{
		public float confidence { get; set; }
		public int end { get; set; }
		public int start { get; set; }
		public string text { get; set; }
		public object speaker { get; set; }
	}

}
