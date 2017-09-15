using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace YummyConsole.Helpers
{
    public static class StringHelper
    {
	    private const string _newlineReplacePattern = @"(\r\n|\n\r|\r)";
	    private const string _newline = "\n";

		/// <summary>
		/// Splits a string of text at spaces and tries to fit the max number of characters per row without breaking a word.
		/// <para>This method assumes newlines are seperated with only \n. See <see cref="FixNewLines"/> for ensuring newlines.</para>
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="fullText"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than or equal to zero</exception>
		public static string[] WordWrap(this string fullText, int maxWidth)
        {
            if (fullText == null) throw new ArgumentNullException(nameof(fullText), "Cannot wordwrap null");
			if (maxWidth <= 0) throw new ArgumentOutOfRangeException(nameof(maxWidth), "Max width must be greater than zero");
			
			var lines = new List<string>();
	        foreach (string line in fullText.Split('\n'))
	        {
		        int length = line.Length;
		        if (length <= maxWidth)
		        {
			        lines.Add(line);
		        }
		        else
		        {
					MatchCollection matches = Regex.Matches(line, @"([^\s]*)(\s*)");
					int spaceLeft = maxWidth;
					var sb = new StringBuilder();

			        void NewLine()
			        {
						// Add new line if needed
				        lines.Add(sb.ToString());
				        sb.Clear();
				        spaceLeft = maxWidth;
					}

			        void AddWord(string word)
			        {
						int wordLen = word?.Length ?? 0;
						if (string.IsNullOrWhiteSpace(word))
				        {
							if (word != null)
							{
								// Whitespace or empty
								word = word.Substring(0, spaceLeft > wordLen ? wordLen : spaceLeft);
								sb.Append(word);
						        spaceLeft -= wordLen;
					        }
						}
						else if (wordLen <= spaceLeft)
				        {
							// Enough space
					        sb.Append(word);
					        spaceLeft -= wordLen;
				        }
				        else if (wordLen <= maxWidth)
						{
							// Too long for remaining line
							NewLine();
							sb.Append(word);
						}
						else
						{
							// Too long for single line, split it up
							string p1 = word.Substring(0, maxWidth - 1) + '-';
							string p2 = word.Substring(maxWidth - 1);

							AddWord(p1);
							AddWord(p2);
						}
						
				        if (spaceLeft <= 0)
				        {
							NewLine();
				        }
					}

					foreach (Match match in matches)
					{
						AddWord(match.Groups[1].Value); // word
						AddWord(match.Groups[2].Value); // spaces after word
					}

					string lastRow = sb.ToString();
					if (!string.IsNullOrWhiteSpace(lastRow))
						NewLine();
		        }
	        }

	        return lines.Select(
				x => 
					x.Trim()
					 .Replace("\n", string.Empty)
			).ToArray();
        }

		/// <summary>
		/// Replaces \r\n, \n\r, and \r combinations with \n characters
		/// </summary>
	    public static string FixNewLines(this string text)
	    {
		    return Regex.Replace(text, _newlineReplacePattern, _newline);
		}

	    public static string EscapeCharacterLiterals(this char c)
	    {
		    switch (c)
		    {
				case '\a': return @"\a";
				case '\b': return @"\b";
				case '\f': return @"\f";
				case '\n': return @"\n";
				case '\r': return @"\r";
				case '\t': return @"\t";
				case '\v': return @"\v";
				case '\'': return @"\'";
				case '"': return "\\\"";
				case '\\': return @"\";
				default: return c.ToString();
			}
	    }

	    public static string EscapeCharacterLiterals(this string text)
	    {
		    var sb = new StringBuilder();

		    foreach (char c in text)
			    sb.Append(c.EscapeCharacterLiterals());

		    return sb.ToString();
	    }

	    /// <summary>
	    /// Splits up a string into lines and removes lines from the top/start if there's too many rows.
	    /// <para>This method assumes newlines are seperated with only \n. See <see cref="FixNewLines"/> for ensuring newlines.</para>
	    /// </summary>
	    /// <exception cref="ArgumentNullException">Thrown if <paramref name="text"/> is null</exception>
	    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxRows"/> is less than zero</exception>
		public static string TrimRowsFromStart(this string text, int maxRows)
	    {
		    if (text == null) throw new ArgumentNullException(nameof(text), "Text cannot be null");
		    if (maxRows < 0) throw new ArgumentOutOfRangeException(nameof(maxRows), "Max number of rows must be greater than zero or equal to zero");

			// Remove lines if too many
			string[] lines = text.Split('\n');

		    if (lines.Length > maxRows)
		    {
			    string[] newLines = lines.SubArray(lines.Length - maxRows, maxRows);
			    return string.Join("\n", newLines);
		    }

		    return text;
	    }

	    /// <summary>
	    /// Splits up a string into lines and removes lines from the bottom/end if there's too many rows.
	    /// <para>This method assumes newlines are seperated with only \n. See <see cref="FixNewLines"/> for ensuring correct newlines.</para>
	    /// </summary>
	    /// <exception cref="ArgumentNullException">Thrown if <paramref name="text"/> is null</exception>
	    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxRows"/> is less than zero</exception>
	    public static string TrimRowsFromEnd(this string text, int maxRows)
	    {
		    if (text == null) throw new ArgumentNullException(nameof(text), "Text cannot be null");
		    if (maxRows < 0) throw new ArgumentOutOfRangeException(nameof(maxRows), "Max number of rows must be greater than zero or equal to zero");

		    // Remove lines if too many
		    string[] lines = text.Split('\n');

		    if (lines.Length > maxRows)
		    {
			    string[] newLines = lines.SubArray(0, maxRows);
			    return string.Join("\n", newLines);
		    }

		    return text;
	    }
	}
}