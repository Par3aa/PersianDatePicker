using System.Globalization;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Par3aa.PersianDatePicker;

namespace Par3aa;

/// <summary>
/// A Blazor component that provides a Persian (Jalali) date picker using <see cref="System.Globalization.PersianCalendar"/>.
/// Supports binding to <see cref="DateTime"/> and <see cref="DateTime?"/>.
/// </summary>
/// <typeparam name="TValue">The bound value type. Must be <see cref="DateTime"/> or <see cref="DateTime?"/>.</typeparam>
public partial class PersianDatePicker<TValue> : ComponentBase
{
    [Parameter] public TValue Value { get; set; } = default!;
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
    [Parameter] public Expression<Func<TValue>>? ValueExpression { get; set; }
    [Parameter] public EventCallback<TValue> Change { get; set; }

    [Parameter] public string Class { get; set; } = Defaults.Class;
    [Parameter] public string ButtonClass { get; set; } = Defaults.ButtonClass;
    [Parameter] public string InputClass { get; set; } = Defaults.InputClass;

    [Parameter] public string Style { get; set; } = Defaults.Style;
    [Parameter] public string ButtonStyle { get; set; } = Defaults.ButtonStyle;
    [Parameter] public string InputStyle { get; set; } = Defaults.InputStyle;

    [Parameter] public string YearTooltip { get; set; } = Defaults.YearTooltip;
    [Parameter] public string MonthTooltip { get; set; } = Defaults.MonthTooltip;
    [Parameter] public string DayTooltip { get; set; } = Defaults.DayTooltip;

    [Parameter] public string ClearButtonTooltip { get; set; } = Defaults.ClearButtonTooltip;
    [Parameter] public string TodayButtonTooltip { get; set; } = Defaults.TodayButtonTooltip;

    [Parameter] public string SeparatorCharacter { get; set; } = Defaults.SeparatorCharacter;
    [Parameter] public SelectionMode SelectionMode { get; set; } = Defaults.SelectionMode;

    [Parameter] public bool ReadOnly { get; set; } = Defaults.ReadOnly;
    [Parameter] public bool Disabled { get; set; } = Defaults.Disabled;

    /// <summary>
    /// Manually sets the button/icon color for this instance. If not set, a CSS variable fallback chain is used.
    /// </summary>
    [Parameter] public string? Color { get; set; } = Defaults.Color;

    /// <summary>Inline SVG markup for the Today button icon.</summary>
    [Parameter] public string TodayIcon { get; set; } = Defaults.TodayIcon;

    /// <summary>Inline SVG markup for the Clear button icon.</summary>
    [Parameter] public string ClearIcon { get; set; } = Defaults.ClearIcon;

    protected bool IsInteractive => !(Disabled || ReadOnly);
    protected int? TabIndex => IsInteractive ? null : -1;

    [Parameter] public int? SelectedYear { get; set; }
    [Parameter] public EventCallback<int?> SelectedYearChanged { get; set; }

    [Parameter] public int? SelectedMonth { get; set; }
    [Parameter] public EventCallback<int?> SelectedMonthChanged { get; set; }

    [Parameter] public int? SelectedDay { get; set; }
    [Parameter] public EventCallback<int?> SelectedDayChanged { get; set; }

    [Parameter] public bool ShowClearButton { get; set; } = Defaults.ShowClearButton;
    [Parameter] public bool ShowTodayButton { get; set; } = Defaults.ShowTodayButton;

    [Parameter] public int StartYear { get; set; } = Defaults.StartYear;
    [Parameter] public int EndYear { get; set; } = Defaults.EndYear;

    protected bool IsNullable => typeof(TValue) == typeof(DateTime?);

    private int[] Years = Array.Empty<int>();
    private readonly int[] Months = Enumerable.Range(1, 12).ToArray();
    private int[] Days = Array.Empty<int>();

    private readonly PersianCalendar PersianCalendar = new();

    private int? _lastYear;
    private int? _lastMonth;
    private int? _lastDay;
    private int _lastStartYear;
    private int _lastEndYear;

    protected override async Task OnParametersSetAsync()
    {
        if (EndYear < StartYear)
            (StartYear, EndYear) = (EndYear, StartYear);

        var v = GetValueAsNullable();
        var valueYear = v.HasValue ? PersianCalendar.GetYear(v.Value) : (int?)null;
        if (valueYear is int vy)
        {
            if (vy < StartYear) StartYear = vy;
            if (vy > EndYear) EndYear = vy;
        }

        if (SelectedYear is int sy)
        {
            if (sy < StartYear) StartYear = sy;
            if (sy > EndYear) EndYear = sy;
        }

        if (StartYear != _lastStartYear || EndYear != _lastEndYear)
        {
            Years = Enumerable.Range(StartYear, EndYear - StartYear + 1).ToArray();
            _lastStartYear = StartYear;
            _lastEndYear = EndYear;
        }

        ValidateAllowNullConstraints();
        await SyncFromValueAndNotifyAsync();
    }

    private void ValidateAllowNullConstraints()
    {
        if (!IsNullable)
        {
            if (GetValueAsNullable() == null)
                throw new InvalidOperationException("Value cannot be null when bound to non-nullable DateTime.");
        }
    }

    private async Task SyncFromValueAndNotifyAsync()
    {
        ValidateAllowNullConstraints();

        var prevYear = SelectedYear;
        var prevMonth = SelectedMonth;
        var prevDay = SelectedDay;

        var value = GetValueAsNullable();
        if (value != null)
        {
            SelectedYear = PersianCalendar.GetYear(value.Value);
            SelectedMonth = PersianCalendar.GetMonth(value.Value);
            SelectedDay = PersianCalendar.GetDayOfMonth(value.Value);
        }
        else
        {
            SelectedYear = null;
            SelectedMonth = null;
            SelectedDay = null;
        }

        UpdateDays();

        if (SelectedYear != prevYear)
            await SelectedYearChanged.InvokeAsync(SelectedYear);
        if (SelectedMonth != prevMonth)
            await SelectedMonthChanged.InvokeAsync(SelectedMonth);
        if (SelectedDay != prevDay)
            await SelectedDayChanged.InvokeAsync(SelectedDay);

        _lastYear = SelectedYear;
        _lastMonth = SelectedMonth;
        _lastDay = SelectedDay;
    }

    private async Task SetNullAsync()
    {
        if (ReadOnly || Disabled)
            return;

        if (!IsNullable)
            throw new InvalidOperationException("Component is bound to non-nullable DateTime; cannot set null.");

        await SetValueAsync(null);
    }

    private async Task SetTodayAsync()
    {
        if (ReadOnly || Disabled)
            return;

        await SetValueAsync(DateTime.Now);
    }

    private async Task OnDatePartChanged()
    {
        if (ReadOnly || Disabled)
            return;

        CheckMinMaxes();
        UpdateDays();

        if (SelectedDay == null || SelectedMonth == null || SelectedYear == null)
        {
            if (IsNullable)
                Value = default!;
            else
                RevertValues();

            return;
        }

        DateTime composed;
        try
        {
            composed = PersianCalendar.ToDateTime(SelectedYear.Value, SelectedMonth.Value, SelectedDay.Value, 0, 0, 0, 0);
        }
        catch (Exception ex)
        {
            RevertValues();
            Console.WriteLine($"Invalid Persian date entered\r\n{ex}");
            return;
        }

        await SetValueAsync(composed);
    }

    private void CheckMinMaxes()
    {
        if (SelectedYear != null)
        {
            if (Years.Length > 0)
            {
                if (SelectedYear.Value < StartYear)
                    SelectedYear = StartYear;
                else if (SelectedYear.Value > EndYear)
                    SelectedYear = EndYear;
            }
            else
            {
                SelectedYear = null;
            }
        }

        if (SelectedMonth != null)
        {
            if (SelectedMonth.Value < 1)
                SelectedMonth = 1;
            else if (SelectedMonth.Value > 12)
                SelectedMonth = 12;
        }

        if (SelectedDay != null)
        {
            if (Days.Length > 0)
            {
                if (SelectedDay.Value < 1)
                    SelectedDay = 1;
                else if (SelectedDay.Value > Days.Last())
                    SelectedDay = Days.Last();
            }
            else
            {
                SelectedDay = null;
            }
        }
    }

    private void RevertValues()
    {
        var oldValue = GetValueAsNullable();

        if (oldValue == null)
        {
            SelectedYear = null;
            SelectedMonth = null;
            SelectedDay = null;
            return;
        }

        SelectedYear = PersianCalendar.GetYear(oldValue.Value);
        SelectedMonth = PersianCalendar.GetMonth(oldValue.Value);
        SelectedDay = PersianCalendar.GetDayOfMonth(oldValue.Value);
    }

    private void UpdateDays()
    {
        if (SelectedMonth == null)
        {
            SelectedDay = null;
            Days = Array.Empty<int>();
            return;
        }

        int maxDay =
            SelectedYear != null
                ? PersianCalendar.GetDaysInMonth(SelectedYear.Value, SelectedMonth.Value)
                : SelectedMonth.Value <= 6 ? 31 : 30;

        if (SelectedDay > maxDay)
            SelectedDay = maxDay;

        Days = Enumerable.Range(1, maxDay).ToArray();
    }

    private DateTime? GetValueAsNullable()
    {
        if (typeof(TValue) == typeof(DateTime))
            return (DateTime)(object)Value!;
        if (typeof(TValue) == typeof(DateTime?))
            return (DateTime?)(object?)Value!;
        throw new InvalidOperationException("TValue must be DateTime or DateTime?.");
    }

    private async Task SetValueAsync(DateTime? newValue)
    {
        if (typeof(TValue) == typeof(DateTime))
        {
            if (newValue is null)
                throw new InvalidOperationException("Component is bound to non-nullable DateTime; cannot set null.");
            var cast = (TValue)(object)newValue.Value;
            Value = cast;
            await ValueChanged.InvokeAsync(cast);
            await Change.InvokeAsync(cast);
        }
        else if (typeof(TValue) == typeof(DateTime?))
        {
            var cast = (TValue)(object?)newValue!;
            Value = cast;
            await ValueChanged.InvokeAsync(cast);
            await Change.InvokeAsync(cast);
        }
        else
        {
            throw new InvalidOperationException("TValue must be DateTime or DateTime?.");
        }

        await SyncFromValueAndNotifyAsync();
    }

    private static int? ParseNullableInt(object? value)
    {
        var s = value?.ToString();
        if (string.IsNullOrWhiteSpace(s)) return null;
        if (int.TryParse(s, out var i)) return i;
        return null;
    }

    private async Task YearSelectChanged(ChangeEventArgs e)
    {
        if (ReadOnly || Disabled) return;

        SelectedYear = ParseNullableInt(e.Value);
        await SelectedYearChanged.InvokeAsync(SelectedYear);
        await OnDatePartChanged();
    }

    private async Task MonthSelectChanged(ChangeEventArgs e)
    {
        if (ReadOnly || Disabled) return;

        SelectedMonth = ParseNullableInt(e.Value);
        await SelectedMonthChanged.InvokeAsync(SelectedMonth);
        await OnDatePartChanged();
    }

    private async Task DaySelectChanged(ChangeEventArgs e)
    {
        if (ReadOnly || Disabled) return;

        SelectedDay = ParseNullableInt(e.Value);
        await SelectedDayChanged.InvokeAsync(SelectedDay);
        await OnDatePartChanged();
    }

    private async Task OnPartInputChanged(Func<Task> notifyCallback)
    {
        if (ReadOnly || Disabled) return;

        await notifyCallback();
        await OnDatePartChanged();
    }

    private Task YearInputChanged() => OnPartInputChanged(() => SelectedYearChanged.InvokeAsync(SelectedYear));
    private Task MonthInputChanged() => OnPartInputChanged(() => SelectedMonthChanged.InvokeAsync(SelectedMonth));
    private Task DayInputChanged() => OnPartInputChanged(() => SelectedDayChanged.InvokeAsync(SelectedDay));
}