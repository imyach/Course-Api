using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CourseDesktopClient.UI.Converters
{
    public static class HyperlinkHelper
    {
        private static readonly Regex UrlRegex = new Regex(
            @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.RegisterAttached(
                "FormattedText",
                typeof(string),
                typeof(HyperlinkHelper),
                new FrameworkPropertyMetadata(string.Empty, OnFormattedTextChanged));

        public static void SetFormattedText(DependencyObject obj, string value)
        {
            obj.SetValue(FormattedTextProperty, value);
        }

        public static string GetFormattedText(DependencyObject obj)
        {
            return (string)obj.GetValue(FormattedTextProperty);
        }

        private static void OnFormattedTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock)
            {
                textBlock.Inlines.Clear();
                var formattedText = e.NewValue as string ?? string.Empty;

                if (string.IsNullOrEmpty(formattedText))
                    return;

                ProcessText(formattedText, textBlock);
            }
        }

        private static void ProcessText(string text, TextBlock textBlock)
        {
            var lastIndex = 0;
            var matches = UrlRegex.Matches(text);

            foreach (Match match in matches)
            {
                if (match.Index > lastIndex)
                {
                    textBlock.Inlines.Add(new Run(text.Substring(lastIndex, match.Index - lastIndex)));
                }

                var url = match.Value;
                var hyperlink = new Hyperlink(new Run(url))
                {
                    NavigateUri = new Uri(url),
                    ToolTip = "Нажмите, чтобы открыть ссылку"
                };
                hyperlink.RequestNavigate += (s, args) =>
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo(args.Uri.ToString()) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}");
                    }
                    args.Handled = true;
                };

                textBlock.Inlines.Add(hyperlink);
                lastIndex = match.Index + match.Length;
            }

            if (lastIndex < text.Length)
            {
                textBlock.Inlines.Add(new Run(text.Substring(lastIndex)));
            }
        }
    }
}