using System;
namespace AudioProcessor.Models
{

	public class TranscriptionResponse
	{
		public string self { get; set; }
		public string[] contentUrls { get; set; }
		public Model model { get; set; }
		public Links links { get; set; }
		public Properties properties { get; set; }
		public DateTime lastActionDateTime { get; set; }
		public string status { get; set; }
		public DateTime createdDateTime { get; set; }
		public string locale { get; set; }
		public string displayName { get; set; }
		public Customproperties customProperties { get; set; }
	}

	public class Model
	{
		public string self { get; set; }
	}

	public class Links
	{
		public string files { get; set; }
	}



	public class Customproperties
	{
		public string key { get; set; }
	}

	public class TranscriptionResponseOLD
	{
		public string source { get; set; }
		public DateTime timestamp { get; set; }
		public int durationInTicks { get; set; }
		public string duration { get; set; }
		public Combinedrecognizedphrases[] combinedRecognizedPhrases { get; set; }
		public Recognizedphrase[] recognizedPhrases { get; set; }
	}

	public class Combinedrecognizedphrases
	{
		public int channel { get; set; }
		public string lexical { get; set; }
		public string itn { get; set; }
		public string maskedITN { get; set; }
		public string display { get; set; }
	}

	public class Recognizedphrase
	{
		public string recognitionStatus { get; set; }
		public int speaker { get; set; }
		public int channel { get; set; }
		public string offset { get; set; }
		public string duration { get; set; }
		public float offsetInTicks { get; set; }
		public float durationInTicks { get; set; }
		public Nbest[] nBest { get; set; }
	}

	public class Nbest
	{
		public float confidence { get; set; }
		public string lexical { get; set; }
		public string itn { get; set; }
		public string maskedITN { get; set; }
		public string display { get; set; }
		public Word[] words { get; set; }
	}

	public class Word
	{
		public string word { get; set; }
		public string offset { get; set; }
		public string duration { get; set; }
		public float offsetInTicks { get; set; }
		public float durationInTicks { get; set; }
		public float confidence { get; set; }
	}


	public class ErrorResponse
	{
		public string code { get; set; }
		public string message { get; set; }
		public Innererror innerError { get; set; }
	}

	public class Innererror
	{
		public string code { get; set; }
		public string message { get; set; }
	}


}
