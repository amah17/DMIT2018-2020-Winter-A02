<Query Kind="Statements">
  <Connection>
    <ID>9df50ea2-30c7-4b92-bdc2-e939d0321a97</ID>
    <Server>.</Server>
    <Database>Schedule</Database>
  </Connection>
</Query>

var q8 = from x in Schedules.ToList()
			where x.Day.Month == 1
			group x by x.Employee into groupEmployee
			select new
			{
				Name = groupEmployee.Key.FirstName + " " + groupEmployee.Key.LastName,
				//RegularEarnings = string.Format("{0:0.00}", groupEmployee.Where(row => !row.OverTime).Sum((row =>row.HourlyWage) * (row =>row.Shift.EndTime - row.Shift.StartTime).Hours)),
				RegularEarnings = groupEmployee.Sum(y => y.OverTime? 0 : (y.Shift.EndTime.Hours - y.Shift.StartTime.Hours) * y.HourlyWage).ToString("0.00"),
				OverTimeEarnings = groupEmployee.Sum(ot => ot.OverTime?(ot.Shift.EndTime.Hours - ot.Shift.StartTime.Hours) * (ot.HourlyWage * 1.5m):0).ToString("0.00"),
				NumberOfShifts = groupEmployee.Count()
			};
q8.Dump();