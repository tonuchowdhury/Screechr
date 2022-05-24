using Newtonsoft.Json.Linq;
using System.Text;

namespace Screechr.API
{
    public static class Helper
    {
		private static readonly Random _random = new Random((int)DateTime.UtcNow.ToFileTime());

		public static string RandomString(int length = 0, bool includedSpaces = false)
		{
			var bytes = new byte[length > 0
				? length
				: RandomValue(3, 64)];

			for (var i = 0; i < bytes.Length; i++)
			{
				var randomValue = RandomValue(65, 122);

				while (randomValue > 90 && randomValue < 97 && randomValue != 32)
					randomValue = includedSpaces
						? 32
						: RandomValue(65, 122);

				bytes[i] = (byte)randomValue;
			}

			return Encoding.ASCII.GetString(bytes);
		}

		public static int RandomValue(int min = 0, int max = 0)
		{
			return min >= max
				? _random.Next()
				: _random.Next(min, max);
		}

		public static bool ValidateApiRequestBody(object json, out JToken requestBody)
		{
			requestBody = null;

			var jsonString = json?.ToString();
			if (jsonString != null)
			{
				try
				{
					var token = JToken.Parse(jsonString);

					//if (expects == typeof(JArray) && !(token is JArray)) return false;
					//if (expects == typeof(JObject) && !(token is JObject)) return false;

					if (token is JArray)
					{
						requestBody = JArray.Parse(jsonString);
						if (requestBody == null) return false;

						if (JArray.Parse(jsonString).Children().Count() > 0)
						{
							var empty = true;
							foreach (var content in requestBody)
							{
								if (!content.HasValues)
									continue;

								empty = false;
							}

							if (empty) return false;

							return true;
						}
					}

					if (token is JObject)
					{
						requestBody = JObject.Parse(jsonString);
						if (((JObject)requestBody)?.Children().Count() != 0) return true;
					}
				}
				catch { }
			}

			return false;
		}

	}
}
