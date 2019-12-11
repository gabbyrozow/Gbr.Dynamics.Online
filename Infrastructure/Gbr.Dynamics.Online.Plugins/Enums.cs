namespace Gbr.Dynamics.Online.Plugins
{
    public enum MessageProcessingMode
    {
        Synchronous = 0,
        Asynchronous = 1
    }

    public enum MessageProcessingStage
    {
        PreValidation = 10,
        PreOperation = 20,
        PostOperation = 40
    }
}