namespace AudioProcessor.Models
{

    public static class ExtensionMethods
    {
        public static string EnumValue(this ProcessStatusEnum e)
        {
            switch (e)
            {
                case ProcessStatusEnum.Uploaded:
                    return "Uploaded";
                case ProcessStatusEnum.Processing:
                    return "Processing";
                case ProcessStatusEnum.Completed:
                    return "Completed";
                case ProcessStatusEnum.Failed:
                    return "Failed";
            }
            return "Horrible Failure!!";
        }
    }
}
