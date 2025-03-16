using System;
using System.Collections.Generic;
using Card;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	[Serializable]
	public class GameSave
	{
		
		
		[JsonConverter(typeof(DictionaryConverter))]
		public Dictionary<Vector2Int, BuildType> BuildMap = new();
	}
	
	public class DictionaryConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType == typeof(Dictionary<Vector2Int, BuildType>);

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var dict = new Dictionary<Vector2Int, BuildType>();
			var tempDict = serializer.Deserialize<Dictionary<string, BuildType>>(reader);

			foreach (var pair in tempDict)
			{
				var vector = JsonConvert.DeserializeObject<Vector2Int>(pair.Key);
				dict[vector] = pair.Value;
			}

			return dict;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var dict = (Dictionary<Vector2Int, BuildType>)value;
			var tempDict = new Dictionary<string, BuildType>();

			foreach (var pair in dict)
			{
				tempDict[JsonConvert.SerializeObject(pair.Key)] = pair.Value;
			}

			serializer.Serialize(writer, tempDict);
		}
	}
}