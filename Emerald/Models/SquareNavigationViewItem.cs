using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Emerald.Models;

public partial class SquareNavigationViewItem : Model
{
    public SquareNavigationViewItem()
    {
        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(IsSelected) || e.PropertyName == nameof(ShowFontIcons))
            {
                InvokePropertyChanged(null);
            }
        };
    }
    public SquareNavigationViewItem(string name, bool isSelected = false, ImageSource image = null, InfoBadge infoBadge = null)
    {
        Name = name;
        IsSelected = isSelected;
        Thumbnail = image;
        InfoBadge = infoBadge;
        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(IsSelected) || e.PropertyName == nameof(ShowFontIcons))
            {
                InvokePropertyChanged(null);
            }
        };
    }
    public string Tag { get; set; }

    [ObservableProperty]
    private string _Name;

    [ObservableProperty]
    private string _FontIconGlyph;

    [ObservableProperty]
    private string _SolidFontIconGlyph;

    [ObservableProperty]
    private bool _IsSelected;

    [ObservableProperty]
    private bool _IsEnabled = true;

    [ObservableProperty]
    private ImageSource _Thumbnail;

    [ObservableProperty]
    private InfoBadge _InfoBadge;

    [ObservableProperty]
    private bool _ShowFontIcons;

    public Visibility FontIconVisibility => ShowFontIcons && !IsSelected ? Visibility.Visible : Visibility.Collapsed;
    public Visibility SolidFontIconVisibility => ShowFontIcons && IsSelected ? Visibility.Visible : Visibility.Collapsed;
    public Visibility SelectionVisibility => IsSelected ? Visibility.Collapsed : Visibility.Visible;
    
    public bool ShowThumbnail => !ShowFontIcons;
}
