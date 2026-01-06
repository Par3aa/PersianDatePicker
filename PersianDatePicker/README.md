# PersianDatePicker

A Blazor Razor Class Library providing a Persian (Jalali) date picker built on `System.Globalization.PersianCalendar`. It supports binding to `DateTime` and `DateTime?`, two input modes (dropdowns or numeric inputs), and global defaults configured once per application.

## Features

- 📅 Persian (Jalali) calendar via `System.Globalization.PersianCalendar`
- 🔁 Two-way binding for `DateTime` and `DateTime?`
- 🔽 DropDown mode or 🔢 TextBox mode
- 📆 Configurable year range with automatic normalization
- 🧰 Optional Today and Clear actions
- ⚙️ Global defaults (`PersianDatePickerDefaults`) so parameters aren’t repeated in every instance
- 🎨 Minimal CSS; integrates cleanly with popular UI libraries
- ♿ Accessibility-conscious (ARIA and focus handling)
- 🧩 Customizable SVG icons

---

## Installation

```bash
dotnet add package PersianDatePicker
```

---

## Getting Started

Add the namespace:

```razor
@using Par3aa.PersianDatePicker.Components
```

Bind to `DateTime`:

```razor
<PersianDatePicker TValue="DateTime"
                   @bind-Value="MyDate"
                   SelectionMode="PersianDatePickerSelectionMode.DropDown"
                   ShowTodayButton="true"
                   StartYear="1350"
                   EndYear="1450" />
```

Bind to `DateTime?` (nullable):

```razor
<PersianDatePicker TValue="DateTime?"
                   @bind-Value="MyNullableDate"
                   SelectionMode="PersianDatePickerSelectionMode.TextBox"
                   ShowClearButton="true"
                   ShowTodayButton="true" />
```

> `ShowClearButton` is valid only when bound to `DateTime?`.

---

## Configure Once, Use Everywhere

Set global defaults at application startup:

```csharp
using Par3aa.PersianDatePicker.Options;

// Example defaults
PersianDatePickerDefaults.SelectionMode = PersianDatePickerSelectionMode.DropDown;
PersianDatePickerDefaults.ShowTodayButton = true;
PersianDatePickerDefaults.ShowClearButton = true;
PersianDatePickerDefaults.StartYear = 1320;
PersianDatePickerDefaults.EndYear = PersianCalendar.GetYear(DateTime.Today);

// Optional styling defaults
PersianDatePickerDefaults.InputClass = "my-input";
PersianDatePickerDefaults.ButtonClass = "my-button";
PersianDatePickerDefaults.Style = "border: 1px solid #e6e6e6; border-radius: 6px;";
PersianDatePickerDefaults.Color = "slategray";
```

Any instance parameter overrides the corresponding default.

---

## Color and Theming

Button and icon color is driven by a CSS variable with a broad fallback chain across popular frameworks:

```css
:root {
  --button-color:
    var(--rz-primary,               /* Radzen */
    var(--mud-palette-primary,      /* MudBlazor */
    var(--kendo-color-primary,      /* Kendo/Telerik */
    var(--e-primary-color,          /* Syncfusion */
    var(--dxbl-primary,             /* DevExpress Blazor */
    var(--bs-primary,               /* Blazorise / Bootstrap */
    #7f00ff                         /* Fallback (Blazor violet) */
  )))));
}
```

Icons use `fill="currentColor"`, so they inherit `--button-color`.
You can add `fill="currentColor"` to your own SVG files to have them inherit the color dynamically too.

- Set per instance:
  ```razor
  <PersianDatePicker TValue="DateTime?"
                     @bind-Value="MyNullableDate"
                     Color="#2d6a4f"  <!-- overrides --button-color -->
                     ShowClearButton="true"
                     ShowTodayButton="true" />
  ```

- Set globally:
  ```csharp
  PersianDatePickerDefaults.Color = "#2d6a4f";
  ```

- Customize icons (globally or per instance) by supplying inline SVG:
  ```csharp
  PersianDatePickerDefaults.TodayIcon = "<svg ... fill=\"currentColor\">...</svg>";
  PersianDatePickerDefaults.ClearIcon = "<svg ... fill=\"currentColor\">...</svg>";
  ```

---

## Accessibility

- `aria-label` and `aria-disabled` on interactive elements
- `tabindex` set to `-1` when not interactive
- Native inputs that support keyboard navigation
- Invalid date combinations revert to last valid value

---

## API Overview

Key parameters:

| Parameter | Type | Default | Description |
|---|---|---|---|
| `TValue` | Either `DateTime` or `DateTime?` | — | Bound value type. Can be ignored, it is auto-detected from `Value`. |
| `Value` | `TValue` | — | Bound date value. |
| `ValueChanged` | `EventCallback<TValue>` | — | Invoked on value changes. |
| `Change` | `EventCallback<TValue>` | — | Alias event for value changes. |
| `SelectionMode` | `PersianDatePickerSelectionMode` | `TextBox` | DropDown or TextBox inputs. |
| `StartYear` | `int` | `1300` | Minimum selectable Persian year. If the bound `Value` is earlier than this, the range automatically extends to include it. |
| `EndYear`   | `int` | `1500` | Maximum selectable Persian year. If the bound `Value` is later than this, the range automatically extends to include it. |
| `ShowTodayButton` | `bool` | `false` | Shows a 📆 button that sets `Value` to `DateTime.Today`. |
| `ShowClearButton` | `bool` | `false` | Shows a ❌ button that sets `Value` to `null` (nullable `DateTime?` only). |
| `SeparatorCharacter` | `string` | `"-"` | Separator between inputs. (e.g. 1404 - 01 - 27) |
| `ReadOnly` | `bool` | `false` | Prevent interaction but show value. |
| `Disabled` | `bool` | `false` | Disable control. |
| `Color` | `string?` | `null` | Overrides `--button-color` for this instance. |
| `TodayIcon` | `string` | bundled | Inline SVG for `Today Button` (add `fill=\"currentColor\"` to your custom icons). |
| `ClearIcon` | `string` | bundled | Inline SVG for `Clear Button` (add `fill=\"currentColor\"` to your custom icons). |
| `Class`, `Style` | `string` | `""` | Root element styling. |
| `ButtonClass`, `ButtonStyle` | `string` | `""` | Button styling. |
| `InputClass`, `InputStyle` | `string` | `""` | Input styling. (used for both `TextBox` and `DropDown` SelectionModes) |

Selected parts (two-way binding):

| Parameter | Type | Description |
|---|---|---|
| `SelectedYear` | `int?` | Current Persian year. |
| `SelectedMonth` | `int?` | Current Persian month (1–12). |
| `SelectedDay` | `int?` | Current Persian day-of-month. |

Tooltips:

| Parameter | Default |
|---|---|
| `YearTooltip` | `Year` |
| `MonthTooltip` | `Month` |
| `DayTooltip` | `Day` |

---

## How To Use

### Option 1: Build and pack locally
```bash
dotnet pack -c Release
```

### Option 2: Install from NuGet
```bash
dotnet add package Par3aa.PersianDateTime
```

---

## License

MIT License file available in root directory.