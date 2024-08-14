using CitizenshipCalculator;
using System.Diagnostics.Metrics;
//https://fyi.org.nz/request/18902-what-the-term-each-12-month-period-in-the-presence-requirement-having-been-in-new-zealand-for-at-least-240-days-in-each-12-month-period-in-the-last-5-years-means-exactly-for-citizenship-by-grant
List<TravelRecord> travelRecords = new List<TravelRecord>();

bool calculateAlternative240Rule = false;
string dateFormat = "dd/MM/yyyy";
int SHORT_PERIOD_DAYS = 240; // 240 days in 12 month period
int TOTAL_DAYS = 1350; // 1350 days in 5 years

DateTime START_RESIDENCY = new DateTime(2022, 04, 26);
DateTime CITIZENSHIP_DATE = START_RESIDENCY.AddYears(5); // normal route 5 whole years
CITIZENSHIP_DATE = CITIZENSHIP_DATE.AddDays(0); // fast route, including days before cizitenship   


Console.WriteLine("---------------------");
Console.WriteLine($"{CITIZENSHIP_DATE.ToString(dateFormat)}");
Console.WriteLine("---------------------");
Console.WriteLine();

travelRecords.Add(new TravelRecord(new DateTime(2022, 12, 14), new DateTime(2023, 01, 04))); // have travelled
travelRecords.Add(new TravelRecord(new DateTime(2023, 07, 06), new DateTime(2023, 08, 12))); // have travelled
travelRecords.Add(new TravelRecord(new DateTime(2023, 12, 18), new DateTime(2024, 02, 24))); // have travelled


travelRecords.Add(new TravelRecord(new DateTime(2024, 09, 20), new DateTime(2024, 10, 31))); // booked

travelRecords.Add(new TravelRecord(new DateTime(2025, 1, 10), new DateTime(2025, 02, 20)));

travelRecords.Add(new TravelRecord(new DateTime(2025, 06, 10), new DateTime(2025, 08, 4)));
travelRecords.Add(new TravelRecord(new DateTime(2025, 12, 20), new DateTime(2026, 02, 13)));
travelRecords.Add(new TravelRecord(new DateTime(2026, 06, 10), new DateTime(2026, 08, 4)));
travelRecords.Add(new TravelRecord(new DateTime(2026, 12, 20), new DateTime(2027, 02, 13)));



//travelRecords.Add(new TravelRecord(new DateTime(2027, 7, 10), new DateTime(2027, 08, 20)));




int totalDaysAllowedToTravel = (CITIZENSHIP_DATE.AddDays(-1) - CITIZENSHIP_DATE.AddYears(-5)).Days + 1 - TOTAL_DAYS;
int totalDaysTravelled = travelRecords.Sum(x => x.DaysTravelledWithinPeriod(CITIZENSHIP_DATE.AddYears(-5), CITIZENSHIP_DATE.AddDays(-1)));

if (CITIZENSHIP_DATE.AddYears(-5) < START_RESIDENCY)
{
    totalDaysTravelled += (START_RESIDENCY - CITIZENSHIP_DATE.AddYears(-5)).Days;
}


int totalDaysLeftToTravel = totalDaysAllowedToTravel - totalDaysTravelled;

// calculation for days travelled and 1350 rule
Console.WriteLine($"TRAVEL RECORDS");
foreach (var record in travelRecords)
{
    Console.WriteLine(record);
}

Console.WriteLine();
Console.WriteLine("1350 RULE CONDITIONS");
Console.WriteLine($"Total days allowed: {totalDaysAllowedToTravel}");
Console.WriteLine($"Days travelled: {totalDaysTravelled}");
Console.WriteLine($"Days left to travel: {totalDaysLeftToTravel}");

Console.WriteLine();

// Calculate rule 240. This assumes 5 12-month periods, and not ANY 12-month period
Console.WriteLine("240 RULE CONDITIONS COUNTING BACKWARDS");

DateTime startPeriod = CITIZENSHIP_DATE.AddYears(-5).AddDays(0);
DateTime endPeriod = startPeriod.AddYears(1).AddDays(-1);

int totalDaysTravelledUptoPeriod = 0;
while (true)
{
    int daysTravelledThisPeriod = 0;
    int tripsThisPeriod = 0;
    foreach (var record in travelRecords)
    {
        var travelledDays = record.DaysTravelledWithinPeriod(startPeriod, endPeriod);
        daysTravelledThisPeriod += travelledDays;
        if (travelledDays > 0)
        {
            tripsThisPeriod++;
        }
    }

    if (startPeriod < START_RESIDENCY)
    {
       daysTravelledThisPeriod += (START_RESIDENCY - startPeriod).Days;

    }

    totalDaysTravelledUptoPeriod += daysTravelledThisPeriod;
    int daysThisPeriod = (endPeriod - startPeriod).Days + 1;
    int daysAllowedThisPeriod = daysThisPeriod - SHORT_PERIOD_DAYS;
    int daysLeftToTravel = Math.Min(totalDaysAllowedToTravel - totalDaysTravelledUptoPeriod, daysAllowedThisPeriod - daysTravelledThisPeriod);
    

    Console.WriteLine($"Period {startPeriod.ToString(dateFormat)} - {endPeriod.ToString(dateFormat)}. Days in period: {daysThisPeriod}. Days travelled this period: {daysTravelledThisPeriod}. Days left to travel: {daysLeftToTravel}. Trips in this period (incl. partial trips): {tripsThisPeriod}");
    startPeriod = endPeriod.AddDays(1);
    endPeriod = startPeriod.AddYears(1).AddDays(-1);

    if (startPeriod >= CITIZENSHIP_DATE)
    {
        break;
    }
}

int DaysInNZYear(DateTime d1, DateTime end)
{
    int days = 0;
    
    while (d1 <= end)
    {
        if (d1 < START_RESIDENCY) 
        { 
            
        }
        else if (travelRecords.All(x => x.InNZ(d1)))
        {
            days++;
        }
        d1=d1.AddDays(1);
    }

    return days;
}
Console.WriteLine();

int startYear = -5;
int totalDaysinNZ = 0;
while (startYear < 0)
{
    DateTime start = CITIZENSHIP_DATE.AddYears(startYear);
    DateTime end = start.AddYears(1).AddDays(-1);
    int days = DaysInNZYear(start, end);
    totalDaysinNZ += days;
    Console.WriteLine($"Period {start.ToString(dateFormat)} - {end.ToString(dateFormat)}. Days in NZ - {days}. Days to travel: {days - SHORT_PERIOD_DAYS}");
    startYear++;
}
Console.WriteLine($"Days total in NZ: {totalDaysinNZ}");


DateTime test1 = new DateTime(2022, 04, 22);
DateTime test2 = test1.AddYears(1);
