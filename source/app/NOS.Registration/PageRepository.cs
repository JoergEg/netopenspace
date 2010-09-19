using System;

using ScrewTurn.Wiki;
using ScrewTurn.Wiki.PluginFramework;

namespace NOS.Registration
{
	internal class PageRepository : IPageRepository
	{
		public void Save(PageInfo page, string title, string userName, string comment, string content)
		{
			Pages.ModifyPage(page, title, userName, DateTime.Now, comment, content, new string[] { }, null, SaveMode.Normal);
		}

		public PageInfo FindPage(string pageName)
		{
			return Pages.FindPage(pageName);
		}
	}
}