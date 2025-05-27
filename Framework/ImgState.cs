public class ImgState
{
    public event Action? OnBackgroundChanged;
    private bool _useAltBackground;

    public bool UseAltBackground
    {
        get => _useAltBackground;
        set
        {
            if (_useAltBackground != value)
            {
                _useAltBackground = value;
                OnBackgroundChanged?.Invoke();
            }
        }
    }
}
