using System.Collections.Generic;

namespace Randomizer
{
	public class FishData
	{
		public readonly static Dictionary<int, string> DefaultStringData =
			Globals.ModRef.Helper.GameContent.Load<Dictionary<int, string>>("Data/Fish");

		/// <summary>
		/// Unfortunately, the data in DefaultStringData is not accurate - the game seems to actually
		/// use the ones in LocationData instead, so this is is a list of the accurate season 
		/// 
		/// See Carp and Super Cucumber for a couple examples
		/// 
		/// In the future, we could look it up that way instead
		/// </summary>
		public readonly static Dictionary<int, string> DefaultFishSeasons = new()
		{
			{ 128, "summer" },
			{ 129, "spring fall" },
			{ 130, "summer winter" },
			{ 131, "spring fall winter" },
			{ 132, "spring summer fall winter" },
			{ 136, "spring summer fall winter" },
			{ 137, "spring fall" },
			{ 138, "summer" },
			{ 139, "fall" },
			{ 140, "fall winter" },
			{ 141, "winter" },
			{ 142, "spring summer fall" },
			{ 143, "spring fall winter" },
			{ 144, "summer winter" },
			{ 145, "spring summer" },
			{ 146, "summer winter" },
			{ 147, "spring winter" },
			{ 148, "spring fall" },
			{ 149, "summer" },
			{ 150, "summer fall winter" },
			{ 151, "winter" },
			{ 154, "fall winter" },
			{ 155, "summer fall" },
			{ 156, "spring summer fall winter" },
			{ 158, "spring summer fall winter" },
			{ 159, "summer" },
			{ 160, "fall" },
			{ 161, "spring summer fall winter" },
			{ 162, "spring summer fall winter" },
			{ 163, "spring" },
			{ 164, "spring summer fall winter" },
			{ 165, "spring summer fall winter" },
			{ 267, "spring summer" },
			{ 269, "fall winter" },
			{ 682, "spring summer fall winter" },
			{ 698, "summer winter" },
			{ 699, "fall winter" },
			{ 700, "spring summer fall winter" },
			{ 701, "summer fall" },
			{ 702, "spring summer fall winter" },
			{ 704, "summer" },
			{ 705, "fall winter" },
			{ 706, "spring summer fall" },
			{ 707, "winter" },
			{ 708, "spring summer winter" },
			{ 734, "spring summer fall winter" },
			{ 775, "winter" },
			{ 795, "spring summer fall winter" },
			{ 796, "spring summer fall winter" },
			{ 798, "spring summer fall winter" },
			{ 799, "spring summer fall winter" },
			{ 800, "spring summer fall winter" },
		};
		public enum FishFields
		{
			Name,
			DartChance,
			BehaviorType,
			MinSize,
			MaxSize,
			Times,
			Seasons,
			Weather,
			Unused,
			MinWaterDepth,
			SpawnMultiplier,
			DepthMultiplier,
			MinFishingLevel
		}

		/// <summary>
		/// Populates the given fish with the default info
		/// </summary>
		/// <param name="fish">The fish</param>
		public static void FillDefaultFishInfo(FishItem fish)
		{
			string input = DefaultStringData[fish.Id];

			string[] fields = input.Split('/');
			if (fields.Length != 13)
			{
				Globals.ConsoleError($"Incorrect number of fields when parsing fish with input: {input}");
				return;
			}

            // Name
            // Skipped because it's computed from the id

            // Dart Chance
            if (!int.TryParse(fields[(int)FishFields.DartChance], out int dartChance))
			{
				Globals.ConsoleError($"Could not parse the dart chance when parsing fish with input: {input}");
				return;
			}
			fish.DartChance = dartChance;

			// Behavior type
			string behaviorTypeString = fields[(int)FishFields.BehaviorType];
			switch (behaviorTypeString)
			{
				case "mixed":
					fish.BehaviorType = FishBehaviorType.Mixed;
					break;
				case "dart":
					fish.BehaviorType = FishBehaviorType.Dart;
					break;
				case "smooth":
					fish.BehaviorType = FishBehaviorType.Smooth;
					break;
				case "floater":
					fish.BehaviorType = FishBehaviorType.Floater;
					break;
				case "sinker":
					fish.BehaviorType = FishBehaviorType.Sinker;
					break;
				default:
					Globals.ConsoleError($"Fish behavior type {behaviorTypeString} not found when parsing fish with input: {input}");
					return;
			}

			// Min Size
			if (!int.TryParse(fields[(int)FishFields.MinSize], out int minSize))
			{
				Globals.ConsoleError($"Could not parse the min size when parsing fish with input: {input}");
				return;
			}
			fish.MinSize = minSize;

			// Max Size
			if (!int.TryParse(fields[(int)FishFields.MaxSize], out int maxSize))
			{
				Globals.ConsoleError($"Could not parse the max size when parsing fish with input: {input}");
				return;
			}
			fish.MaxSize = maxSize;

			// Times
			List<int> times = ParseTimes(fields[(int)FishFields.Times]);
			if (times.Count == 2)
			{
				fish.Times = new Range(times[0], times[1]);
			}
			else if (times.Count == 4)
			{
				if (times[0] < times[1] && times[1] < times[2] && times[2] < times[3])
				{
					fish.Times = new Range(times[0], times[3]);
					fish.ExcludedTimes = new Range(times[1], times[2]);
				}
				else
				{
					Globals.ConsoleError($"Times are not in chronological order when parsing fish with input: {input}");
				}
			}

			// Seasons
			string[] seasonStrings = DefaultFishSeasons[fish.Id].Split(' ');
			foreach (string seasonString in seasonStrings)
			{
				switch (seasonString.ToLower())
				{
					case "spring":
						fish.AvailableSeasons.Add(Seasons.Spring);
						break;
					case "summer":
						fish.AvailableSeasons.Add(Seasons.Summer);
						break;
					case "fall":
						fish.AvailableSeasons.Add(Seasons.Fall);
						break;
					case "winter":
						fish.AvailableSeasons.Add(Seasons.Winter);
						break;
					default:
						Globals.ConsoleError($"Tried to parse {seasonString} into a season when parsing fish with input: {input}");
						return;
				}
			}

			// Weather
			string weather = fields[(int)FishFields.Weather];
			switch (weather)
			{
				case "sunny":
					fish.Weathers.Add(Weather.Sunny);
					break;
				case "rainy":
					fish.Weathers.Add(Weather.Rainy);
					break;
				case "both":
					fish.Weathers.Add(Weather.Sunny);
					fish.Weathers.Add(Weather.Rainy);
					break;
				default:
					Globals.ConsoleError($"Unexpected weather string when parsing fish with input: {input}");
					break;
			}

			// Unused
			fish.UnusedData = fields[(int)FishFields.Unused];

			// Min Water Depth,
			if (!int.TryParse(fields[(int)FishFields.MinWaterDepth], out int minWaterDepth))
			{
				Globals.ConsoleError($"Could not parse the min water depth when parsing fish with input: {input}");
				return;
			}
			fish.MinWaterDepth = minWaterDepth;

			// Spawn Multiplier,
			if (!double.TryParse(fields[(int)FishFields.SpawnMultiplier], out double spawnMultiplier))
			{
				Globals.ConsoleError($"Could not parse the spawn multiplier when parsing fish with input: {input}");
				return;
			}
			fish.SpawnMultiplier = spawnMultiplier;

			// Depth Multiplier,
			if (!double.TryParse(fields[(int)FishFields.DepthMultiplier], out double depthMultiplier))
			{
				Globals.ConsoleError($"Could not parse the depth multiplier when parsing fish with input: {input}");
				return;
			}
			fish.DepthMultiplier = depthMultiplier;

			// Min Fishing Level
			if (!int.TryParse(fields[(int)FishFields.MinFishingLevel], out int minFishingLevel))
			{
				Globals.ConsoleError($"Could not parse the min fishing level when parsing fish with input: {input}");
				return;
			}
			fish.MinFishingLevel = minFishingLevel;
		}

		/// <summary>
		/// Parses the given time string into a list of integers
		/// </summary>
		/// <param name="timeString">The time string</param>
		/// <returns />
		private static List<int> ParseTimes(string timeString)
		{
			string[] timeStringParts = timeString.Split(' ');
			List<int> times = new List<int>();

			foreach (string time in timeStringParts)
			{
				if (!int.TryParse(time, out int intTime))
				{
					Globals.ConsoleError($"Could not convert time to integer in {timeString}");
					return null;
				}
				times.Add(intTime);
			}

			return times;
		}
	}
}
