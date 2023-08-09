using CitizenshipCalculator;

List<TravelRecord> travelRecords = new List<TravelRecord>();

bool calculateAlternative240Rule = false;
string dateFormat = "dd/MM/yyyy";
int SHORT_PERIOD_DAYS = 240; // 240 days in 12 month period
int TOTAL_DAYS = 1350; // 1350 days in 5 years

DateTime START_RESIDENCY = new DateTime(2022, 04, 26);
DateTime CITIZENSHIP_DATE = START_RESIDENCY.AddYears(5); // normal route 5 whole years
CITIZENSHIP_DATE = START_RESIDENCY.AddYears(5).AddDays(-30); // fast route, including days before cizitenship   
travelRecords.Add(new TravelRecord(CITIZENSHIP_DATE.AddYears(-5), START_RESIDENCY.AddDays(1))); // any day before residency is day travelled

travelRecords.Add(new TravelRecord(new DateTime(2022, 12, 14), new DateTime(2023, 01, 04))); // have travelled
travelRecords.Add(new TravelRecord(new DateTime(2023, 07, 06), new DateTime(2023, 08, 12))); // have travelled

travelRecords.Add(new TravelRecord(new DateTime(2023, 12, 10), new DateTime(2024, 02, 20)));
travelRecords.Add(new TravelRecord(new DateTime(2024, 06, 11), new DateTime(2024, 08, 09)));
travelRecords.Add(new TravelRecord(new DateTime(2024, 12, 20), new DateTime(2025, 02, 10)));
travelRecords.Add(new TravelRecord(new DateTime(2025, 06, 10), new DateTime(2025, 08, 10)));
travelRecords.Add(new TravelRecord(new DateTime(2025, 12, 20), new DateTime(2026, 02, 10)));
travelRecords.Add(new TravelRecord(new DateTime(2026, 06, 10), new DateTime(2026, 08, 10)));
//travelRecords.Add(new TravelRecord(new DateTime(2026, 12, 20), new DateTime(2027, 02, 10)));

int totalDaysAllowedToTravel = (CITIZENSHIP_DATE - CITIZENSHIP_DATE.AddYears(-5)).Days - TOTAL_DAYS;
int totalDaysTravelled = travelRecords.Sum(x => x.DaysTravelled());
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

decimal numberOf30DayPeriods = (CITIZENSHIP_DATE - travelRecords.Last().End).Days / 30m;
Console.WriteLine($"Average days to travel PER 30-DAY PERIOD left: {Math.Round(totalDaysLeftToTravel / numberOf30DayPeriods, 2)}");

decimal numberOf356DayPeriods = (CITIZENSHIP_DATE - travelRecords.Last().End).Days / 365m;
Console.WriteLine($"Average days to travel PER 365-DAY PERIOD left: {Math.Round(totalDaysLeftToTravel / numberOf356DayPeriods, 2)}");

Console.WriteLine();

// Calculate rule 240. This assumes 5 12-month periods, and not ANY 12-month period
Console.WriteLine("240 RULE CONDITIONS");

DateTime startPeriod = CITIZENSHIP_DATE.AddYears(-5);
DateTime endPeriod = startPeriod.AddYears(1);

int totalDaysTravelledUptoPeriod = 0;
while (true)
{
    int daysTravelledThisPeriod = 0;
    foreach (var record in travelRecords)
    {
        daysTravelledThisPeriod += record.DaysTravelledWithinPeriod(startPeriod, endPeriod);
    }

    totalDaysTravelledUptoPeriod += daysTravelledThisPeriod;
    int daysAllowedThisPeriod = (endPeriod - startPeriod).Days - SHORT_PERIOD_DAYS;
    int daysLeftToTravel = Math.Min(totalDaysAllowedToTravel - totalDaysTravelledUptoPeriod, daysAllowedThisPeriod - daysTravelledThisPeriod);

    Console.WriteLine($"Period {startPeriod.ToString(dateFormat)} - {endPeriod.ToString(dateFormat)}. Days travelled: {daysTravelledThisPeriod}. Days left to travel: {daysLeftToTravel}");
    startPeriod = endPeriod.AddDays(1);
    endPeriod = startPeriod.AddYears(1).AddDays(-1);

    if (startPeriod >= CITIZENSHIP_DATE)
    {
        break;
    }
}

// just in case, calculate any 12-month period
startPeriod = CITIZENSHIP_DATE.AddYears(-5);
endPeriod = startPeriod.AddYears(1);



while (true)
{
    if (!calculateAlternative240Rule)
    {
        break;
    }

    if (endPeriod > CITIZENSHIP_DATE)
    {
        break;
    }

    int daysTravelledThisPeriod = 0;
    int daysAllowedThisPeriod = (endPeriod - startPeriod).Days - SHORT_PERIOD_DAYS;

    foreach (var record in travelRecords)
    {
        daysTravelledThisPeriod += record.DaysTravelledWithinPeriod(startPeriod, endPeriod);
    }
    
    if (daysTravelledThisPeriod > daysAllowedThisPeriod)
    {
        Console.WriteLine($"ERROR: too many days travelled {startPeriod.ToString(dateFormat)} - {endPeriod.ToString(dateFormat)}. Days {daysTravelledThisPeriod}. Allowed {daysAllowedThisPeriod}");
    }

    startPeriod = startPeriod.AddDays(1);
    endPeriod = startPeriod.AddYears(1);
}
