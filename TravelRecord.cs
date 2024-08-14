using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenshipCalculator
{
    public class TravelRecord
    {
        public TravelRecord(DateTime start, DateTime end, string note = null)
        {
            Start = start;
            End = end;
            Note = note;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        public string Note { get; set; }

        public bool InNZ(DateTime d)
        {
            if (d > Start && d < End)
            {
                return false;
            }

            return true;
        }

        public int DaysTravelled()
        {
            return (End - Start).Days - 1;
        }

        public int DaysTravelledWithinPeriod(DateTime periodStart, DateTime periodEnd)
        {
            if (periodStart > End)
            {
                return 0;
            }

            if (periodEnd < Start)
            {
                return 0;
            }

            DateTime tmpStart = periodStart >= Start ? periodStart : Start;
            DateTime tmpEnd = periodEnd >= End ? End : periodEnd;

            int days =  (tmpEnd - tmpStart).Days - 1;

            if (periodStart > Start)
            {
                days++;
            }

            if (periodEnd < End)
            {
                days++;
            }

            return days;

        }

        public override string ToString()
        {
            return $"{Start:dd/MM/yyyy} - {End:dd/MM/yyyy}. Days {DaysTravelled()}. Note: {Note}";
        }
    }
}
