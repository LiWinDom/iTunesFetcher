using System;

namespace iTunesFetcher.Services;

public static class StatusService
{
    public static event EventHandler<string>? StatusTextChanged;
    public static event EventHandler<int>? ProgressBarValueChanged;
    public static event EventHandler<int>? ProgressBarMaximumChanged;
    
    private static string _statusText = "";
    public static string StatusText
    {
        get => _statusText;
        set
        {
            _statusText = value;
            StatusTextChanged?.Invoke(typeof(StatusService), _statusText);
        }
    }
    
    private static int _progressBarValue = 0;
    public static int ProgressBarValue
    {
        get => _progressBarValue;
        set
        {
            _progressBarValue = value;
            ProgressBarValueChanged?.Invoke(typeof(StatusService), _progressBarValue);
        }
    }
    
    private static int _progressBarMaximum = 0;
    public static int ProgressBarMaximum
    {
        get => _progressBarMaximum;
        set
        {
            _progressBarMaximum = value;
            ProgressBarMaximumChanged?.Invoke(typeof(StatusService), _progressBarMaximum);
        }
    }
}
