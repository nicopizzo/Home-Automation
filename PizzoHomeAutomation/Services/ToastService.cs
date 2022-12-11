using CommunityToolkit.Maui.Alerts;

namespace PizzoHomeAutomation.Services
{
    public interface IToastService
    {
        Task Show(string message);
    }

    internal class ToastService : IToastService
    {

        public async Task Show(string message)
        {
            var toast = Toast.Make(message);
            await toast.Show();
        }
    }
}
