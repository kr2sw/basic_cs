using System;
using System.Diagnostics;

namespace BasicCS.Chapter15;

internal class DateTimeDemo
{
    static void Main()
    {
        // ---- 1. DateTime 기본 ----
        Console.WriteLine("=== DateTime 기본 ===\n");

        // 현재 시간
        DateTime now = DateTime.Now;
        DateTime utcNow = DateTime.UtcNow;
        DateTime today = DateTime.Today;

        Console.WriteLine($"DateTime.Now  (로컬 현재): {now}");
        Console.WriteLine($"DateTime.UtcNow  (UTC): {utcNow}");
        Console.WriteLine($"DateTime.Today   (오늘 날짜): {today}");

        // DateTime 생성
        DateTime specific = new DateTime(2026, 12, 25, 10, 30, 0);
        Console.WriteLine($"Specific date: {specific}");

        // ---- 2. DateTime 속성 ----
        Console.WriteLine("\n=== DateTime 속성 ===");
        Console.WriteLine($"Year: {now.Year}");
        Console.WriteLine($"Month: {now.Month}");
        Console.WriteLine($"Day: {now.Day}");
        Console.WriteLine($"Hour: {now.Hour}");
        Console.WriteLine($"Minute: {now.Minute}");
        Console.WriteLine($"Second: {now.Second}");
        Console.WriteLine($"DayOfWeek: {now.DayOfWeek}");
        Console.WriteLine($"DayOfYear: {now.DayOfYear}");
        Console.WriteLine($"Kind: {now.Kind} (로컬) / {utcNow.Kind} (UTC)");

        // ---- 3. DateTime 연산 (AddDays, AddMonths 등) ----
        Console.WriteLine("\n=== DateTime 연산 ===");
        DateTime baseDate = new DateTime(2026, 1, 1);
        Console.WriteLine($"기준: {baseDate:yyyy-MM-dd}");
        Console.WriteLine($"AddDays(10):   {baseDate.AddDays(10):yyyy-MM-dd}");
        Console.WriteLine($"AddDays(-5):   {baseDate.AddDays(-5):yyyy-MM-dd}");
        Console.WriteLine($"AddMonths(3):  {baseDate.AddMonths(3):yyyy-MM-dd}");
        Console.WriteLine($"AddYears(1):   {baseDate.AddYears(1):yyyy-MM-dd}");
        Console.WriteLine($"AddHours(25):  {baseDate.AddHours(25):yyyy-MM-dd HH:mm}");
        Console.WriteLine($"AddMinutes(90):{baseDate.AddMinutes(90):yyyy-MM-dd HH:mm}");

        // ---- 4. TimeSpan 계산 ----
        Console.WriteLine("\n=== TimeSpan 계산 ===");
        DateTime start = new DateTime(2026, 1, 1, 9, 0, 0);
        DateTime end = new DateTime(2026, 1, 5, 17, 30, 0);

        TimeSpan duration = end - start;
        Console.WriteLine($"From {start} to {end}");
        Console.WriteLine($"Duration: {duration}");
        Console.WriteLine($"  Days: {duration.Days}");
        Console.WriteLine($"  Hours: {duration.Hours}");
        Console.WriteLine($"  Minutes: {duration.Minutes}");
        Console.WriteLine($"  TotalHours: {duration.TotalHours:F2}");
        Console.WriteLine($"  TotalMinutes: {duration.TotalMinutes:F0}");

        // TimeSpan 직접 생성
        TimeSpan ts1 = new TimeSpan(2, 30, 0);       // 2시간 30분
        TimeSpan ts2 = TimeSpan.FromHours(3.5);      // 3.5시간
        TimeSpan ts3 = TimeSpan.FromMinutes(90);      // 90분
        Console.WriteLine($"\nTimeSpan(2,30,0): {ts1}");
        Console.WriteLine($"TimeSpan.FromHours(3.5): {ts2}");
        Console.WriteLine($"TimeSpan.FromMinutes(90): {ts3}");

        TimeSpan tsSum = ts1 + ts2;
        Console.WriteLine($"ts1 + ts2 = {tsSum}");

        // ---- 5. Parse / TryParse / ParseExact ----
        Console.WriteLine("\n=== DateTime 파싱 ===");

        // Parse
        DateTime parsed1 = DateTime.Parse("2026-12-25");
        Console.WriteLine($"Parse(\"2026-12-25\"): {parsed1:yyyy-MM-dd}");

        DateTime parsed2 = DateTime.Parse("01/15/2026");
        Console.WriteLine($"Parse(\"01/15/2026\"): {parsed2:yyyy-MM-dd}");

        // TryParse
        string[] dateStrings = { "2026-07-04", "invalid-date", "2026/12/31" };
        foreach (string ds in dateStrings)
        {
            if (DateTime.TryParse(ds, out DateTime dt))
                Console.WriteLine($"TryParse(\"{ds}\"): {dt:yyyy-MM-dd}");
            else
                Console.WriteLine($"TryParse(\"{ds}\"): FAILED");
        }

        // ParseExact (정확한 형식 지정)
        string exactInput = "2026.08.15 14:30:00";
        DateTime exactParsed = DateTime.ParseExact(
            exactInput,
            "yyyy.MM.dd HH:mm:ss",
            null);
        Console.WriteLine($"ParseExact(\"{exactInput}\"): {exactParsed}");

        // ---- 6. DateTimeOffset ----
        Console.WriteLine("\n=== DateTimeOffset (시간대 오프셋 포함) ===");
        DateTimeOffset dtoNow = DateTimeOffset.Now;
        DateTimeOffset dtoUtc = DateTimeOffset.UtcNow;

        Console.WriteLine($"DateTimeOffset.Now:       {dtoNow}");
        Console.WriteLine($"DateTimeOffset.UtcNow:    {dtoUtc}");
        Console.WriteLine($"Offset:                   {dtoNow.Offset}");

        // 특정 시간대의 DateTimeOffset
        DateTimeOffset seoul = new DateTimeOffset(2026, 7, 29, 14, 0, 0, TimeSpan.FromHours(9));
        Console.WriteLine($"Seoul time: {seoul} (KST, +09:00)");

        // UTC 변환
        Console.WriteLine($"Seoul in UTC: {seoul.ToUniversalTime()}");

        // ---- 7. TimeZoneInfo ----
        Console.WriteLine("\n=== TimeZoneInfo (시간대 변환) ===");
        try
        {
            TimeZoneInfo seoulZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
            TimeZoneInfo nyZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime kstNow = DateTime.Now; // 로컬 시간
            DateTime nyTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(kstNow, "Korea Standard Time", "Eastern Standard Time");
            Console.WriteLine($"KST: {kstNow:HH:mm:ss} -> NY: {nyTime:HH:mm:ss}");

            // UTC to specific time zone
            DateTime utc = DateTime.UtcNow;
            DateTime kstFromUtc = TimeZoneInfo.ConvertTimeFromUtc(utc, seoulZone);
            Console.WriteLine($"UTC: {utc:HH:mm:ss} -> KST: {kstFromUtc:HH:mm:ss}");
        }
        catch (TimeZoneNotFoundException)
        {
            Console.WriteLine("Time zone not found on this system.");
        }

        // ---- 8. DateOnly (.NET 6+) ----
        Console.WriteLine("\n=== DateOnly (.NET 6+) ===");
        DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
        Console.WriteLine($"DateOnly.FromDateTime(Now): {dateOnly}");
        Console.WriteLine($"Year: {dateOnly.Year}, Month: {dateOnly.Month}, Day: {dateOnly.Day}");

        DateOnly independence = new DateOnly(2026, 7, 4);
        Console.WriteLine($"Independence Day: {independence}");

        int diffDays = dateOnly.DayNumber - independence.DayNumber;
        Console.WriteLine($"Difference in days from July 4: {diffDays}");

        // ---- 9. TimeOnly (.NET 6+) ----
        Console.WriteLine("\n=== TimeOnly (.NET 6+) ===");
        TimeOnly timeOnly = TimeOnly.FromDateTime(DateTime.Now);
        Console.WriteLine($"TimeOnly.FromDateTime(Now): {timeOnly}");
        Console.WriteLine($"Hour: {timeOnly.Hour}, Minute: {timeOnly.Minute}, Second: {timeOnly.Second}");

        TimeOnly lunchTime = new TimeOnly(12, 30, 0);
        TimeOnly nowTime = TimeOnly.FromDateTime(DateTime.Now);
        TimeSpan tillLunch = lunchTime - nowTime;
        Console.WriteLine($"Lunch time: {lunchTime}");
        Console.WriteLine($"Time until lunch: {tillLunch}");

        if (nowTime.IsBetween(new TimeOnly(9, 0), new TimeOnly(12, 0)))
            Console.WriteLine("It's morning working hours.");

        // ---- 10. Stopwatch (성능 측정) ----
        Console.WriteLine("\n=== Stopwatch 성능 측정 ===");
        Stopwatch sw = Stopwatch.StartNew();

        long sum = 0;
        for (int i = 0; i < 1_000_000; i++)
            sum += i;

        sw.Stop();
        Console.WriteLine($"\nSum 0..999999 = {sum}");
        Console.WriteLine($"Elapsed: {sw.Elapsed}");
        Console.WriteLine($"ElapsedMilliseconds: {sw.ElapsedMilliseconds}ms");
        Console.WriteLine($"ElapsedTicks: {sw.ElapsedTicks} ticks");

        // Stopwatch 여러 번 사용
        sw.Restart();
        System.Threading.Thread.Sleep(100);
        sw.Stop();
        Console.WriteLine($"\nSleep(100ms) measured: {sw.ElapsedMilliseconds}ms");

        // ---- 11. 문화권별 날짜 형식 ----
        Console.WriteLine("\n=== 문화권별 날짜 형식 ===");
        DateTime sample = new DateTime(2026, 7, 29, 14, 30, 0);
        Console.WriteLine($"기본 형식: {sample}");
        Console.WriteLine($"ToShortDateString: {sample.ToShortDateString()}");
        Console.WriteLine($"ToLongDateString:  {sample.ToLongDateString()}");
        Console.WriteLine($"ToShortTimeString: {sample.ToShortTimeString()}");
        Console.WriteLine($"ToLongTimeString:  {sample.ToLongTimeString()}");

        // 커스텀 형식
        Console.WriteLine($"yyyy-MM-dd:        {sample:yyyy-MM-dd}");
        Console.WriteLine($"yyyy/MM/dd HH:mm:  {sample:yyyy/MM/dd HH:mm}");
        Console.WriteLine($"dddd, MMMM dd:     {sample:dddd, MMMM dd}");

        Console.WriteLine("\n=== All DateTime examples completed ===");
    }
}
