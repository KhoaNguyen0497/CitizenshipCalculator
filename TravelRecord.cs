using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenshipCalculator
{
    public class TravelRecord
    {
        public TravelRecord(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int DaysTravelled()
        {
            return (End - Start).Days + 1; // Inclusive of all days
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

            return (tmpEnd - tmpStart).Days + 1;

        }

        public override string ToString()
        {
            return $"{Start:dd/MM/yyyy} - {End:dd/MM/yyyy}. Days {DaysTravelled()}";
        }
    }
}
