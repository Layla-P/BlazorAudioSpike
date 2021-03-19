using System;
using Microsoft.Azure.Cosmos.Table;
using System.Reflection;
using System.ComponentModel;

namespace AudioProcessor.Models
{
	public class AudioEntity : TableEntity
	{
		public AudioEntity() { }

		public AudioEntity(
			string partitionKey,
			string id,
			string fileName,
			ProcessStatusEnum processStatusEnum,
			string processedUrl)
		{
			PartitionKey = partitionKey;
			// this is the transcription id from AssemblyAI
			RowKey = id;
			FileName = fileName;
			ProcessStatusEnum = processStatusEnum.EnumValue();
			ProcessedUrl = processedUrl;
		}

		public string FileName { get; set; }
		public string ProcessStatusEnum { get; set; }
		public string ProcessedUrl { get; set; }
	}

}