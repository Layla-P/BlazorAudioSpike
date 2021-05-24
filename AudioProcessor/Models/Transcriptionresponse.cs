using System;
namespace AudioProcessor.Models
{

	public class TranscriptionResponse
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

}
