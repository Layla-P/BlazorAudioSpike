namespace AudioProcessor.Models
{

    public static class ExtensionMethods
    {
        public static string EnumValue(this ProcessStatusEnum e)
        {
            switch (e)
            {
                case ProcessStatusEnum.NotStarted:
                    return "NotStarted";
                case ProcessStatusEnum.Running:
                    return "Running";
                case ProcessStatusEnum.Succeeded:
                    return "Succeeded";
                case ProcessStatusEnum.Failed:
                    return "Failed";
            }
            return "Horrible Failure!!";
        }
    }
}
