using System;
using System.Globalization;
using System.Linq;

public static class CurrencyConverter
{
	public static float ParseValueOfFormattedPrice(string formattedPrice)
	{
		char[] numbers = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
		int num = formattedPrice.IndexOfAny(numbers);
		int num2 = formattedPrice.LastIndexOfAny(numbers);
		string text = ((num <= -1 || num2 <= -1) ? string.Empty : formattedPrice.Substring(num, num2 - num + 1));
		char separator = ((text.Length > 2 && !numbers.Contains(text[text.Length - 3])) ? text[text.Length - 3] : '\0');
		if (separator == '\0' && text.Length > 2)
		{
			separator = (from c in CultureInfo.GetCultures(CultureTypes.AllCultures)
				where !c.IsNeutralCulture && c.NumberFormat.CurrencyDecimalDigits > 2 && formattedPrice.Contains(c.NumberFormat.CurrencySymbol)
				select c.NumberFormat.CurrencyDecimalSeparator[0]).LastOrDefault();
		}
		text = new string(text.Where((char c) => numbers.Contains(c) || c == separator).ToArray());
		text = text.Replace(separator.ToString(), CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
		return Convert.ToSingle(text);
	}
}
