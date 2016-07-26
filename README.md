# Console Helpers

Collection of utilities for writing console-based applications in .NET

## Quick start ##

```c#
using static ConsoleHelpers.Helpers;

class FooBar
{
  public string foo { get; set; }
  public string bar { get; set; }
}

class Program
{
  static void Main(string[] args)
  {
    Header("This is a header");
    string input = PromptRead("This is a prompt");
    string input2 = PromptRead("You can also specify default values", "foobar");
    
    IList<string> choices = new List<string>
    {
      "foo",
      "bar",
      "baz"
    };
    
    string selected = PromptSelect("IEnumerables of strings are selectable by their index", choices);
    
    IList<FooBar> fooBarChoices = new List<FooBar>
    {
      new FooBar { foo = "foo"; bar = "bar" },
      new FooBar { foo = "bar"; bar = "foo" }
    };
    
    FooBar selectedFooBar = PromptSelect<FooBar>(
      "IEnumerables of objects are selectable too!",
      fooBarChoices,
      fooBar => $"foo = {foo}, bar = {bar}"
    );
  }
}
```

## API ##

**Classes**

* SelectedIndex - represents either:
  * the index of a selected item within a list or
  * a cancelled selection

```c#
public class SelectedIndex
{
  public int Value { get; }
  public bool Cancelled { get; } = false;
}
```

* SelectedValue&lt;T&gt; - represents either:
  * the value of a selected item (T) within a list (IEnumberable&lt;T&gt;) or
  * a cancelled selection

```c#
public class SelectedValue<T> where T : class
{
  public T Value { get; } = default(T);
  public bool Cancelled { get; } = false;
}
```

**Constants**

* string EOL
  * Equivelent to `Environment.NewLine` (just simpler to type!)

**Variables**

* ConsoleColor HEADER_COLOR
  * The color of text written by the `Header` method
  * Default: ConsoleColor.White
* ConsoleColor HEADER_BG
  * The color of the background of text written by the `Header` method
  * Default: ConsoleColor.DarkBlue
* ConsoleColor PROMPT_COLOR
  * Defualt: ConsoleColor.Gray
* ConsoleColor PROMPT_BG
  * Default: ConsoleColor.Black

**Methods**

* `void WriteWithColor(ConsoleColor textColor, ConsoleColor bgColor, string message, bool newLine)`
  * Prints the provided `message` to the console using the provided colors
  * textColor - the color of the text to print
  * bgColor - the color of the background to print
  * message - the message to print
  * newLine - whether or not to print a newline after the message
* `void Blue(string message [, bool newLine = true ])`
  * Prints message with text color of blue
  * By default, prints a newline character after the message unless `false` is provided for the `newLine` argument
* `void Red(string message [, bool newLine = true ])`
  * Prints message in red
  * By default, prints a newline character after the message unless `false` is provided for the `newLine` argument
* `void Yellow(string message [, bool newLine = true ])`
  * Prints message in yellow
  * By default, prints a newline character after the message unless `false` is provided for the `newLine` argument
* `void White(string message [, bool newLine = true ])`
  * Prints message in white
  * By default, prints a newline character after the message unless `false` is provided for the `newLine` argument
* `void Gray(string message [, bool newLine = true ])`
  * Prints message in gray
  * By default, prints a newline character after the message unless `false` is provided for the `newLine` argument
* `void Header(string message)`
  * Prints a new line followed by `"## {message} ##"`
  * Text color / background color can be changed by setting `HEADER_COLOR` and `HEADER_BG` respectively
* `void Prompt(string message)`
  * Prints a tab followed by `"{message}: "`
  * Text color / background color can be changed by setting `PROMPT_COLOR` and `PROMPT_BG` respectively

* `string PromptRead(string prompt)`
  * Prompts the user to enter a value
  * Returns `null` if the user presses enter without typing anything
  * Returns the entered string if the user types something and presses enter

* `string PromptRead(string prompt, object defaultValue)`
  * Prompts the user to enter a value
  * Returns `defaultValue` if the user presses enter without typing anything
    * If defaultValue is `null`, this method will return `null` when nothing is entered.
    * For any non-`null` value of `defaultValue`, this method will return `defaultValue.ToString()` when nothing is entered
  * Returns the entered string if the user types something and presses enter

* `SelectedIndex PromptSelect(string prompt, IEnumberable<string> choices [, bool allowCancel = true, bool allowNull = false])`
  * Prompts the user to choose one of the provided `choice`s
  * Each element of `choices` will be listed alongside its index within `choices`, and the user can enter the number corresponding to their choice
  * The return value `SelectedIndex` contains the users's selection
  * allowCancel - whether or not the user will be allowed to enter `-1` to cancel this selection
    * if user enters `-1`, the `SelectedIndex` that is returned will have `Cancelled = true`
  * allowNull - whether or not to allow the user to simply press enter, choosing nothing
    * if user chooses nothing, `null` will be returned

* `SelectedIndex PromptSelect(string prompt, IEnumberable<string> choices, object defaultValue [, bool allowCancel = true, bool allowNull = false])`
  * Prompts the user to choose one of the provided `choice`s
  * Each element of `choices` will be listed alongside its index within `choices`, and the user can enter the number corresponding to their choice
  * Returns the index of `defaultValue` within `choices` wrapped in a `SelectedIndex` if the user presses enter without typing anything
    * If defaultValue is `null`, this method will return `null` when nothing is entered.
    * For any non-`null` value of `defaultValue`, this method will return the index of `defaultValue.ToString()` wrapped in a `SelectedIndex` when nothing is entered
  * The return value `SelectedIndex` contains the users's selection
  * allowCancel - whether or not the user will be allowed to enter `-1` to cancel this selection
    * if user enters `-1`, the `SelectedIndex` that is returned will have `Cancelled = true`
  * allowNull - whether or not to allow the user to simply press enter, choosing nothing
    * if user chooses nothing, `null` will be returned

* `SelectedValue<T> PromptSelect<T>(string prompt, IEnumberable<T> choices, Func<T, string> mapping [, bool allowCancel = true, bool allowNull = false])`
  * Prompts the user to choose one of the provided `choice`s
  * Each element of `choices` will be mapped to a string using `mapping` and listed alongside its index within `choices`, and the user can enter the number corresponding to their choice
  * The return value `SelectedValue<T>` contains the users's selection
  * allowCancel - whether or not the user will be allowed to enter `-1` to cancel this selection
    * if user enters `-1`, the `SelectedValue<T>` that is returned will have `Cancelled = true`
  * allowNull - whether or not to allow the user to simply press enter, choosing nothing
    * if user chooses nothing, `null` will be returned

* `SelectedValue<T> PromptSelect<T>(string prompt, IEnumberable<T> choices, Func<T, string> mapping, T defaultValue [, bool allowCancel = true, bool allowNull = false])`
  * Prompts the user to choose one of the provided `choice`s
  * Each element of `choices` will be mapped to a string using `mapping` and listed alongside its index within `choices`, and the user can enter the number corresponding to their choice
  * Returns `defaultValue` wrapped in a `SelectedValue<T>` if the user presses enter without typing anything
    * If defaultValue is `null`, this method will return `null` when nothing is entered.
  * The return value `SelectedValue<T>` contains the users's selection
  * allowCancel - whether or not the user will be allowed to enter `-1` to cancel this selection
    * if user enters `-1`, the `SelectedValue<T>` that is returned will have `Cancelled = true`
  * allowNull - whether or not to allow the user to simply press enter, choosing nothing
    * if user chooses nothing, `null` will be returned

* `SelectedValue<T> PromptSelect<T>(string prompt, IEnumberable<T> choices, Func<T, string> mapping, int defaultIndex [, bool allowCancel = true, bool allowNull = false])`
  * Prompts the user to choose one of the provided `choice`s
  * Each element of `choices` will be mapped to a string using `mapping` and listed alongside its index within `choices`, and the user can enter the number corresponding to their choice
  * Returns `choices[defaultIndex]` wrapped in a `SelectedValue<T>` if the user presses enter without typing anything
    * If `choices[defaultIndex]` is `null`, this method will return `null` when nothing is entered.
  * The return value `SelectedValue<T>` contains the users's selection
  * allowCancel - whether or not the user will be allowed to enter `-1` to cancel this selection
    * if user enters `-1`, the `SelectedValue<T>` that is returned will have `Cancelled = true`
  * allowNull - whether or not to allow the user to simply press enter, choosing nothing
    * if user chooses nothing, `null` will be returned
