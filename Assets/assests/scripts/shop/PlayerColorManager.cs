using UnityEngine;
using UnityEngine.UI;

public class PlayerColorManager : MonoBehaviour
{
    private Image _innerImage;
    private bool _isInner;
    private Image _outerImage;
    private bool _preview;
    private StorageHandler _sh;
    private MyColor _tempInner;
    private MyColor _tempOuter;

    public Toggle innerToggle;
    public ColorPicker picker;
    public Toggle previewToggle;

    // Start is called before the first frame update
    private void Start()
    {
        _isInner = true;
        _preview = true;
        _sh = new StorageHandler();
        ResetTemp();
        _outerImage = GetComponent<Image>();
        _innerImage = transform.Find("Player_Inner").GetComponent<Image>();
        picker.CurrentColor = new Color(picker.CurrentColor[0], picker.CurrentColor[1], picker.CurrentColor[2], 1);
        LoadColors();
        _outerImage.color = _tempOuter;
        _innerImage.color = _tempInner;
        picker.onValueChanged.AddListener(color =>
        {
            if (!_preview) return;
            if (_isInner) LoadColors(outerColor: _tempOuter, innerColor: picker.CurrentColor);
            else LoadColors(outerColor: picker.CurrentColor, innerColor: _tempInner);
        });
    }

    public void LoadColors()
    {
        _tempInner = _sh.LoadData("InnerColor") as MyColor;
        _tempOuter = _sh.LoadData("OuterColor") as MyColor;
        if (_tempInner == null) _tempInner = (Color) new Color32(199, 0, 1, 255); // Courtesy of Julius
        if (_tempOuter == null) _tempOuter = new Color(0, 0, 0);
    }

    public void LoadColors(Color outerColor, Color innerColor)
    {
        _outerImage.color = outerColor;
        _innerImage.color = innerColor;
    }

    public void TogglePreview()
    {
        _preview = previewToggle.isOn;
        if (!_preview)
        {
            LoadColors(_sh.LoadData("OuterColor") as MyColor, _sh.LoadData("InnerColor") as MyColor);
            picker.CurrentColor = _isInner ? _tempInner : _tempOuter;
        }
        else
        {
            if (_isInner) _innerImage.color = picker.CurrentColor;
            else _outerImage.color = picker.CurrentColor;
        }

    }

    public void ToggleInner()
    {
        _isInner = innerToggle.isOn;
        if (!_preview) return;
        if (_isInner)
        {
            _tempOuter = picker.CurrentColor;
            LoadColors(outerColor: _tempOuter, innerColor: picker.CurrentColor);
            innerToggle.GetComponentInChildren<Text>().text = "Inner Color";
            picker.CurrentColor = _tempInner;
        }
        else
        {
            _tempInner = picker.CurrentColor;
            LoadColors(outerColor: picker.CurrentColor, innerColor: _tempInner);
            innerToggle.GetComponentInChildren<Text>().text = "Outer Color";
            picker.CurrentColor = _tempOuter;
        }
    }

    public void ResetTemp()
    {
        _tempInner = _sh.LoadData("InnerColor") as MyColor;
        _tempOuter = _sh.LoadData("OuterColor") as MyColor;
        if (_tempInner == null) _tempInner = (Color) new Color32(199, 0, 1, 255); // Courtesy of Julius
        if (_tempOuter == null) _tempOuter = new Color(0, 0, 0);
    }

    public MyColor[] GetCurrentColors()
    {
        return _isInner ? new MyColor[2] {_tempOuter, picker.CurrentColor} : new MyColor[2] {picker.CurrentColor, _tempInner};
    }

    public void SetColorPickerColor(Color a)
    {
        picker.AssignColor(color: a);
    }

    public bool IsInner()
    {
        return _isInner;
    }
}