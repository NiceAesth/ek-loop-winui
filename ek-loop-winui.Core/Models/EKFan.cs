namespace ek_loop_winui.Core.Models;
public class EKFan
{
    public int ID { get; set; } = -1;


    public int PWM { get; set; } = -1;

    public int RPM { get; set; } = -1;

    public string Name => $"Fan {ID + 1}";

    public string ShortDescription => $"{Name} ({ID}) - {RPM}RPM ({PWM}%)";
}
