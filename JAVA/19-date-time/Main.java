import java.time.*;
import java.time.format.*;
import java.time.temporal.*;

public class Main {
    public static void main(String[] args) {
        // 현재 날짜/시간
        LocalDate today = LocalDate.now();
        LocalTime now = LocalTime.now();
        LocalDateTime currentDT = LocalDateTime.now();

        System.out.println("오늘: " + today);
        System.out.println("현재 시간: " + now);
        System.out.println("현재 날짜+시간: " + currentDT);

        // 특정 날짜/시간 생성
        LocalDate christmas = LocalDate.of(2026, 12, 25);
        LocalTime meeting = LocalTime.of(14, 30);
        LocalDateTime specific = LocalDateTime.of(2026, 7, 30, 10, 0);

        System.out.println("크리스마스: " + christmas);
        System.out.println("회의 시간: " + meeting);
        System.out.println("특정 시각: " + specific);

        // 날짜 연산
        LocalDate nextWeek = today.plusWeeks(1);
        LocalDate lastMonth = today.minusMonths(1);
        LocalDate nextYear = today.plusYears(1);

        System.out.println("\n=== 날짜 연산 ===");
        System.out.println("일주일 후: " + nextWeek);
        System.out.println("한 달 전: " + lastMonth);
        System.out.println("1년 후: " + nextYear);

        // 시간 연산
        LocalTime later = now.plusHours(2).plusMinutes(30);
        Duration between = Duration.between(now, later);
        System.out.println("\n=== 시간 연산 ===");
        System.out.println("2시간 30분 후: " + later);
        System.out.println("차이(분): " + between.toMinutes());

        // Period (날짜 간격)
        Period period = Period.between(today, christmas);
        System.out.println("\n=== 날짜 차이 ===");
        System.out.println("크리스마스까지: "
            + period.getYears() + "년 "
            + period.getMonths() + "월 "
            + period.getDays() + "일");

        // DayOfWeek / Month
        System.out.println("\n=== 요일/월 ===");
        System.out.println("오늘 요일: " + today.getDayOfWeek());
        System.out.println("오늘: " + today.getDayOfMonth() + "일");
        System.out.println("이번 달: " + today.getMonth());
        System.out.println("올해: " + today.getYear());

        // 포맷팅
        System.out.println("\n=== 포맷팅 ===");
        DateTimeFormatter formatter1 = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
        System.out.println("포맷: " + currentDT.format(formatter1));

        DateTimeFormatter formatter2 = DateTimeFormatter.ofPattern("yyyy년 M월 d일 (E)");
        System.out.println("한국식: " + today.format(formatter2));

        // 파싱
        String dateStr = "2026-12-25";
        LocalDate parsed = LocalDate.parse(dateStr);
        System.out.println("파싱: " + parsed);

        // ZonedDateTime
        ZonedDateTime seoul = ZonedDateTime.now(ZoneId.of("Asia/Seoul"));
        ZonedDateTime ny = ZonedDateTime.now(ZoneId.of("America/New_York"));
        System.out.println("\n=== 시간대 ===");
        System.out.println("서울: " + seoul);
        System.out.println("뉴욕: " + ny);

        // Instant (타임스탬프)
        Instant nowInstant = Instant.now();
        System.out.println("Instant: " + nowInstant);
        System.out.println("에포크 밀리초: " + nowInstant.toEpochMilli());

        // TemporalAdjusters
        LocalDate firstDayOfMonth = today.with(TemporalAdjusters.firstDayOfMonth());
        LocalDate nextMonday = today.with(TemporalAdjusters.next(DayOfWeek.MONDAY));
        System.out.println("\n=== TemporalAdjusters ===");
        System.out.println("이번 달 첫날: " + firstDayOfMonth);
        System.out.println("다음 월요일: " + nextMonday);

        // 날짜 비교
        System.out.println("\n=== 날짜 비교 ===");
        System.out.println("today.isBefore(christmas): " + today.isBefore(christmas));
        System.out.println("today.isAfter(christmas): " + today.isAfter(christmas));
        System.out.println("today.isEqual(today): " + today.isEqual(today));
    }
}
