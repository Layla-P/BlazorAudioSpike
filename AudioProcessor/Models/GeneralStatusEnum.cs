namespace AudioProcessor.Models
{
	public enum GeneralStatusEnum
	{
		Default = 0,
		Ok = 1,
		BadRequest = 2,
		ServerError = 4,
		Fail = 5,
		Timeout = 6,
		Processing = 7
	}
}