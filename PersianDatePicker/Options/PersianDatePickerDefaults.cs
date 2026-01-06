using Par3aa.PersianDatePicker.Enums;

namespace Par3aa.PersianDatePicker.Options;

/// <summary>
/// Global defaults for the <c>PersianDatePicker</c> component.
/// Set once (e.g. at application startup) to apply across all instances that do not override parameters.
/// </summary>
/// <remarks>
/// Parameters in the component initialize from these defaults.
/// If a consumer supplies an explicit parameter value on an instance, that value overrides the default.
/// </remarks>
public static class PersianDatePickerDefaults
{
    /// <summary>Default CSS class for the root container.</summary>
    public static string Class { get; set; } = string.Empty;

    /// <summary>Default CSS class for action buttons.</summary>
    public static string ButtonClass { get; set; } = string.Empty;

    /// <summary>Default CSS class for input elements.</summary>
    public static string InputClass { get; set; } = string.Empty;

    /// <summary>Default inline style for the root container.</summary>
    public static string Style { get; set; } = string.Empty;

    /// <summary>Default inline style for action buttons.</summary>
    public static string ButtonStyle { get; set; } = string.Empty;

    /// <summary>Default inline style for input elements.</summary>
    public static string InputStyle { get; set; } = string.Empty;

    /// <summary>Default tooltip for the Year input.</summary>
    public static string YearTooltip { get; set; } = "Year";

    /// <summary>Default tooltip for the Month input.</summary>
    public static string MonthTooltip { get; set; } = "Month";

    /// <summary>Default tooltip for the Day input.</summary>
    public static string DayTooltip { get; set; } = "Day";

    /// <summary>Default tooltip for the Clear button.</summary>
    public static string ClearButtonTooltip { get; set; } = "Clear";

    /// <summary>Default tooltip for the Today button.</summary>
    public static string TodayButtonTooltip { get; set; } = "Today";

    /// <summary>Default separator character drawn between inputs.</summary>
    public static string SeparatorCharacter { get; set; } = "-";

    /// <summary>Default selection mode for the component.</summary>
    public static PersianDatePickerSelectionMode SelectionMode { get; set; } = PersianDatePickerSelectionMode.TextBox;

    /// <summary>Default read-only state.</summary>
    public static bool ReadOnly { get; set; } = false;

    /// <summary>Default disabled state.</summary>
    public static bool Disabled { get; set; } = false;

    /// <summary>Default visibility of the "Clear" (set null) button.</summary>
    public static bool ShowClearButton { get; set; } = false;

    /// <summary>Default visibility of the "Today" button.</summary>
    public static bool ShowTodayButton { get; set; } = false;

    /// <summary>Default minimum Persian year.</summary>
    public static int StartYear { get; set; } = 1300;

    /// <summary>Default maximum Persian year.</summary>
    public static int EndYear { get; set; } = 1500;

    /// <summary>
    /// Instance-level override for button/icon color. If null or empty, a CSS variable fallback chain is used.
    /// </summary>
    public static string? Color { get; set; } = null;

    /// <summary>
    /// Default inline SVG markup for the Today icon. Uses <c>currentColor</c> to inherit from CSS.
    /// </summary>
    public static string TodayIcon { get; set; } =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"20px\" viewBox=\"0 -960 960 960\" width=\"20px\" fill=\"currentColor\"><path d=\"M384.23-264Q344-264 316-291.77q-28-27.78-28-68Q288-400 315.77-428q27.78-28 68-28Q424-456 452-428.23q28 27.78 28 68Q480-320 452.23-292q-27.78 28-68 28ZM216-96q-29.7 0-50.85-21.5Q144-139 144-168v-528q0-29 21.15-50.5T216-768h72v-96h72v96h240v-96h72v96h72q29.7 0 50.85 21.5Q816-725 816-696v528q0 29-21.15 50.5T744-96H216Zm0-72h528v-360H216v360Z\"/></svg>";

    /// <summary>
    /// Default inline SVG markup for the Clear icon. Uses <c>currentColor</c> to inherit from CSS.
    /// </summary>
    public static string ClearIcon { get; set; } =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"204px\" viewBox=\"0 -960 960 960\" width=\"20px\" fill=\"currentColor\"><path d=\"m256-200-56-56 224-224-224-224 56-56 224 224 224-224 56 56-224 224 224 224-56 56-224-224-224 224Z\"/></svg>";
}