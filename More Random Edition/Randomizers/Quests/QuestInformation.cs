using System.Collections.Generic;

namespace Randomizer
{
	/// <summary>
	/// Contains the information required for editing quests
	/// </summary>
	public class QuestInformation
	{
		public Dictionary<int, string> QuestReplacements = new();
		public Dictionary<string, string> MailReplacements = new();

		public QuestInformation(
			Dictionary<int, string> questReplacements,
			Dictionary<string, string> mailReplacements)
		{
			QuestReplacements = questReplacements;
			MailReplacements = mailReplacements;
		}
	}
}
