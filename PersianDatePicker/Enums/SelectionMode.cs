namespace Par3aa.PersianDatePicker;

/// <summary>
/// Determines the input style of the picker.
/// </summary>
public enum SelectionMode
{
    /// <summary>
    /// Uses HTML select elements for Year, Month, and Day.
    /// </summary>
    DropDown,

    /// <summary>
    /// Uses numeric input fields for Year, Month, and Day.
    /// </summary>
    TextBox
}