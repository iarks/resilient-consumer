namespace ResilientConsumer.Controllers;

public static class QueueNotifs
{
    public static DateTime? TimeOut { private get; set; }
    
    public static bool ShouldProcess => TimeOut is null || DateTime.UtcNow > TimeOut;
}