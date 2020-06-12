using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RealTimeTranslator.Extensions
{
	public class Translator
	{
		public static string TranslateTextViaGoogle(string input, string lang2In, string lang2Out)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}
			// Set the language from/to in the url (or pass it into this function)
			string url = String.Format
			("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
			 lang2In, lang2Out, Uri.EscapeUriString(input));
			HttpClient httpClient = new HttpClient();
			string result = httpClient.GetStringAsync(url).Result;

			// Get all json data
			var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);

			// Extract just the first array element (This is the only data we are interested in)
			var translationItems = jsonData[0];

			// Translation Data
			string translation = "";

			// Loop through the collection extracting the translated objects
			foreach (object item in translationItems)
			{
				// Convert the item array to IEnumerable
				IEnumerable translationLineObject = item as IEnumerable;

				// Convert the IEnumerable translationLineObject to a IEnumerator
				IEnumerator translationLineString = translationLineObject.GetEnumerator();

				// Get first object in IEnumerator
				translationLineString.MoveNext();

				// Save its value (translated text)
				translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
			}

			// Remove first blank character
			if (translation.Length > 1) { translation = translation.Substring(1); };

			// Return translation
			return translation;
		}

		public static string ConvertLang3To2(string lang3) => lang3 switch
		{
			"rus" => "ru",
			"eng" => "en",
			"jpn" => "ja",
			_ => throw new NotImplementedException("Язык задан неверно.")
		};
	}
}
