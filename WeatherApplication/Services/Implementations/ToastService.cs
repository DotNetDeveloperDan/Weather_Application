namespace WeatherApplication.Services.Implementations
{
    public class ToastService
    {
        public event Action<string, string, string> OnShow;
        public event Action OnHide;

        public void ShowToast(string message, string heading = "", string cssClass = "")
        {
            OnShow?.Invoke(message, heading, cssClass);
        }

        public void ShowSuccess(string message, string heading = "Success")
        {
            ShowToast(message, heading, "toast-notification-success");
        }

        public void ShowError(string message, string heading = "Error")
        {
            ShowToast(message, heading, "toast-notification-error");
        }

        public void HideToast()
        {
            OnHide?.Invoke();
        }
    }
}
