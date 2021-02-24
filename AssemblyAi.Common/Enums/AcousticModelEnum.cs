using System.ComponentModel;

namespace AssemblyAi.Common.Enums
{
	public enum AcousticModelEnum 
	{
		[Description("assemblyai_default")]
		Default = 0,
		[Description("assemblyai_en_uk")]
		UnitedKingdom = 1,
		[Description("assemblyai_en_au")]
		Australian = 2

			//todo: create something to handle custom acoustic model
	}
}
