using System;


namespace AudioProcessor.Models
{
	//	public class TranscriptionRequest
	//	{
	//		public TranscriptionRequest() { }
	//		//public TranscriptionRequest(string self, string displayName, string description, string locale, TranscriptionProperties properties, DateTime createdDateTime, DateTime lastActionDateTime, string status)
	//		//{
	//		//	Self = self;
	//		//	DisplayName = displayName;
	//		//	Description = description;
	//		//	Locale = locale;
	//		//	Properties = properties;
	//		//	CreatedDateTime = createdDateTime;
	//		//	LastActionDateTime = lastActionDateTime;
	//		//	Status = status;
	//		//}

	//		//public string Self { get; set; }

	//		public string DisplayName { get; set; }

	//		public string Description { get; set; }

	//		public string Locale { get; set; } = "en-UK";

	//		public string[] ContentUrls { get; set; }

	//		public TranscriptionProperties Properties { get; set; }

	//		public DateTime CreatedDateTime { get; set; }

	//		public DateTime LastActionDateTime { get; set; }

	//		public string Status { get; set; }
	//	}
	//	public class TranscriptionProperties
	//	{
	//		public TranscriptionProperties(TranscriptionError error)
	//		{
	//			Error = error;
	//		}

	//		public TranscriptionError Error { get; set; }
	//	}
	//	public class TranscriptionError
	//	{
	//		public TranscriptionError(string code, string message)
	//		{
	//			Code = code;
	//			Message = message;
	//		}

	//		public string Code { get; set; }

	//		public string Message { get; set; }
	//	}
	//}


	public class TranscriptionRequest
	{
		public string[] contentUrls { get; set; }
		public Properties properties { get; set; }
		public string locale { get; set; }
		public string displayName { get; set; }
	}

	public class Properties
	{
		public bool diarizationEnabled { get; set; }
		public bool wordLevelTimestampsEnabled { get; set; }
		public string punctuationMode { get; set; }
		public string profanityFilterMode { get; set; }
	}
}