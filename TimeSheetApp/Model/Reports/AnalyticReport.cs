using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.Reports
{
    class AnalyticReport
    {
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string FatherName { get; set; }
		public string BlockName { get; set; }
		public string SubBlockName { get; set; }
		public string ProcessName { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public DateTime timeStart { get; set; }
		public DateTime timeEnd { get; set; }
		public int TimeSpent { get; set; }

	}
}
/*
[dbo].BusinessBlock.BusinessBlockName, [dbo].Supports.Name, [dbo].ClientWays.Name, [dbo].Escalations.Name, [dbo].Formats.Name, [dbo].Risk.riskName
from TimeSheetTable
join [dbo].Process
	on [dbo].Process.id = [dbo].TimeSheetTable._Process
join [dbo].Risk
	on [dbo].Risk.id = [dbo].TimeSheetTable._Risk
join [dbo].Supports
	on [dbo].Supports.Id = [dbo].TimeSheetTable._Support
join [dbo].Block
	on [dbo].Block.Id = [dbo].Process.Block
join [dbo].Formats
	on [dbo].Formats.Id = [dbo].TimeSheetTable._Format
join [dbo].BusinessBlock
	on [dbo].BusinessBlock.Id = [dbo].TimeSheetTable._BusinessBlock
join [dbo].ClientWays
	on [dbo].ClientWays.Id = [dbo].TimeSheetTable._ClientWay
join [dbo].Escalations
	on [dbo].Escalations.Id = [dbo].TimeSheetTable._Escalation
join [dbo].SubBlock
	on [dbo].SubBlock.Id = [dbo].Process.SubBlock
join [dbo].Analytic
	on [dbo].Analytic.id = [dbo].TimeSheetTable._Analytic
WHERE Analytic.id in ({inClause})
AND timeStart > @dateTimeStart AND timeStart < @dateTimeEnd", Connection);
*/