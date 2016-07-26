using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelpers
{
    public static class Helpers
    {
        public static ConsoleColor HEADER_COLOR { get; set; } = ConsoleColor.White;
        public static ConsoleColor HEADER_BG { get; set; } = ConsoleColor.DarkBlue;
        public static ConsoleColor PROMPT_COLOR { get; set; } = ConsoleColor.Gray;
        public static ConsoleColor PROMPT_BG { get; set; } = ConsoleColor.Black;
        public static readonly string EOL = Environment.NewLine;
        public static void WriteWithColor(ConsoleColor textColor, ConsoleColor bgColor, string message, bool newLine)
        {
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = bgColor;
            if (newLine) Console.WriteLine(message); else Console.Write(message);
            Console.ResetColor();
        }
        public static void Blue(string message, bool newLine = true) => WriteWithColor(ConsoleColor.Cyan, ConsoleColor.Black, message, newLine);
        public static void Red(string message, bool newLine = true) => WriteWithColor(ConsoleColor.Red, ConsoleColor.Black, message, newLine);
        public static void Yellow(string message, bool newLine = true) => WriteWithColor(ConsoleColor.Yellow, ConsoleColor.Black, message, newLine);
        public static void White(string message, bool newLine = true) => WriteWithColor(ConsoleColor.White, ConsoleColor.Black, message, newLine);
        public static void Gray(string message, bool newLine = true) => WriteWithColor(ConsoleColor.Gray, ConsoleColor.Black, message, newLine);
        public static void Header(string message) => WriteWithColor(HEADER_COLOR, HEADER_BG, $"{EOL}## {message} ##", true);
        public static void Prompt(string message) => WriteWithColor(PROMPT_COLOR, PROMPT_BG, $"\t{message}: ", false);

        public class SelectedValue<T> where T : class
        {
            public T Value { get; internal set; } = default(T);
            public bool Cancelled { get; internal set; } = false;
        }

        public class SelectedIndex
        {
            public int Value { get; internal set; }
            public bool Cancelled { get; internal set; } = false;
        }

        public static SelectedIndex PromptSelect(string prompt, IEnumerable<string> choices, bool allowCancel = true, bool allowNull = false)
        {
            var message = $"{prompt}:{EOL}{string.Join(EOL, choices)}{EOL}\tEnter a number";
            if (allowCancel) message += " (-1 to cancel)";
            var str = PromptRead(message);
            if (str == null && allowNull) return null;
            int index;
            if (!int.TryParse(str, out index) || (index == -1 && !allowCancel))
            {
                return PromptSelect(prompt, choices, allowCancel, allowNull);
            }
            if (index == -1)
            {
                Yellow("\tCancelled!");
                return new SelectedIndex { Cancelled = true };
            }
            return new SelectedIndex { Value = index };
        }

        public static SelectedIndex PromptSelect(string prompt, IEnumerable<string> choices, object defaultValue, bool allowCancel = true, bool allowNull = false)
        {
            var message = $"{prompt}:{EOL}{string.Join(EOL, choices)}{EOL}\tEnter a number";
            if (allowCancel) message += " (-1 to cancel)";
            var str = PromptRead(message, defaultValue);
            if (str == null && allowNull) return null;
            int index;
            if (!int.TryParse(str, out index) || (index == -1 && !allowCancel))
            {
                return PromptSelect(prompt, choices, defaultValue, allowCancel, allowNull);
            }
            if (index == -1)
            {
                Yellow("\tCancelled!");
                return new SelectedIndex { Cancelled = true };
            }
            return new SelectedIndex { Value = index };
        }

        public static SelectedValue<T> PromptSelect<T>(string prompt, IEnumerable<T> choices, Func<T, string> mapping, bool allowCancel = true, bool allowNull = false) where T : class
        {
            var stringifiedChoices = choices.Select(mapping).Select((c, i) => $"\t\t{i} -- {c}");
            var index = PromptSelect(prompt, stringifiedChoices, allowCancel, allowNull);
            if (index.Cancelled) return new SelectedValue<T> { Cancelled = true };
            if (index == null) return null;
            var choice = choices.ElementAtOrDefault(index.Value);
            if (choice == null)
            {
                Red($"\tInvalid selection.");
                return PromptSelect(prompt, choices, mapping, allowCancel, allowNull);
            }
            return new SelectedValue<T> { Value = choice };
        }

        public static SelectedValue<T> PromptSelect<T>(string prompt, IEnumerable<T> choices, Func<T, string> mapping, T defaultValue, bool allowCancel = true, bool allowNull = false) where T : class
        {
            var defaultIndex = choices.ToList().IndexOf(defaultValue);
            var stringifiedChoices = choices.Select(mapping).Select((c, i) => $"\t\t{i} -- {c}");
            var index = PromptSelect(prompt, stringifiedChoices, defaultIndex, allowCancel, allowNull);
            if (index.Cancelled) return new SelectedValue<T> { Cancelled = true };
            if (index == null) return null;
            var choice = choices.ElementAtOrDefault(index.Value);
            if (choice == null)
            {
                Red($"\tInvalid selection.");
                return PromptSelect(prompt, choices, mapping, defaultValue, allowCancel, allowNull);
            }
            return new SelectedValue<T> { Value = choice };
        }

        public static SelectedValue<T> PromptSelect<T>(string prompt, IEnumerable<T> choices, Func<T, string> mapping, int defaultIndex, bool allowCancel = true, bool allowNull = false) where T : class
        {
            var stringifiedChoices = choices.Select(mapping).Select((c, i) => $"\t\t{i} -- {c}");
            var index = PromptSelect(prompt, stringifiedChoices, defaultIndex, allowCancel, allowNull);
            if (index.Cancelled) return new SelectedValue<T> { Cancelled = true };
            if (index == null) return null;
            var choice = choices.ElementAtOrDefault(index.Value);
            if (choice == null)
            {
                Red($"\tInvalid selection.");
                return PromptSelect(prompt, choices, mapping, defaultIndex, allowCancel, allowNull);
            }
            return new SelectedValue<T> { Value = choice };
        }

        public static string PromptRead(string prompt)
        {
            Prompt(prompt);
            int cursorTop = Console.CursorTop;
            int cursorLeft = Console.CursorLeft;
            var str = Console.ReadLine();
            if (string.IsNullOrEmpty(str))
            {
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Yellow("null");
                return null;
            }
            else
            {
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Yellow(str);
            }
            return str;
        }

        public static string PromptRead(string prompt, object defaultValue)
        {
            Prompt($"{prompt} [{defaultValue ?? "null"}]");
            int cursorTop = Console.CursorTop;
            int cursorLeft = Console.CursorLeft;
            var str = Console.ReadLine();
            if (string.IsNullOrEmpty(str))
            {
                Console.SetCursorPosition(cursorLeft, cursorTop);
                if (defaultValue == null)
                {
                    Yellow("null");
                    return null;
                }
                Yellow(defaultValue.ToString());
                return defaultValue.ToString();
            }
            else
            {
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Yellow(str);
            }
            return str;
        }
    }
}
