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
			string processedUrl,
			string transcriptionId = null)
		{
			PartitionKey = partitionKey;
			RowKey = id;
			FileName = fileName;
			ProcessStatusEnum = processStatusEnum.EnumValue();
			ProcessedUrl = processedUrl;
			TranscriptionId = transcriptionId;
		}

		public string FileName { get; set; }
		public string ProcessStatusEnum { get; set; }
		public string ProcessedUrl { get; set; }
		public string TranscriptionId { get; set; }
	}

}