namespace ek_loop_winui.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
