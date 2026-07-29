namespace BasicCS.Chapter16;

using System.IO;
using System.Text;

internal class FileIOStreams
{
    static void Main()
    {
        // 임시 작업 디렉터리 생성
        string tempDir = Path.Combine(Path.GetTempPath(), $"BasicCS_Ch16_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDir);
        Console.WriteLine($"=== 파일 I/O 및 스트림 예제 ===");
        Console.WriteLine($"작업 디렉터리: {tempDir}");

        try
        {
            // ──────────────────────────────────────────────
            // 1. File.WriteAllText / File.ReadAllText
            // ──────────────────────────────────────────────
            Console.WriteLine("\n--- File.WriteAllText / ReadAllText ---");
            string file1 = Path.Combine(tempDir, "sample.txt");
            string content = "안녕하세요!\nC# 파일 I/O 예제입니다.\nHello, File I/O!";
            File.WriteAllText(file1, content);
            Console.WriteLine("파일 쓰기 완료");

            string readBack = File.ReadAllText(file1);
            Console.WriteLine($"읽은 내용:\n{readBack}");

            // ──────────────────────────────────────────────
            // 2. File.ReadAllLines
            // ──────────────────────────────────────────────
            Console.WriteLine("\n--- File.ReadAllLines ---");
            string[] lines = File.ReadAllLines(file1);
            for (int i = 0; i < lines.Length; i++)
                Console.WriteLine($"  라인 {i + 1}: {lines[i]}");

            // ──────────────────────────────────────────────
            // 3. StreamReader / StreamWriter
            // ──────────────────────────────────────────────
            Console.WriteLine("\n--- StreamReader / StreamWriter ---");
            string file2 = Path.Combine(tempDir, "stream_sample.txt");
            using (StreamWriter writer = new StreamWriter(file2, false, Encoding.UTF8))
            {
                writer.WriteLine("StreamWriter로 작성된 첫 번째 줄");
                writer.WriteLine("StreamWriter로 작성된 두 번째 줄");
                writer.WriteLine("StreamWriter로 작성된 세 번째 줄");
            }
            Console.WriteLine("StreamWriter로 파일 쓰기 완료");

            using (StreamReader reader = new StreamReader(file2, Encoding.UTF8))
            {
                string? line;
                int lineNum = 1;
                while ((line = reader.ReadLine()) != null)
                    Console.WriteLine($"  읽기 {lineNum++}: {line}");
            }

            // ──────────────────────────────────────────────
            // 4. FileStream (using 구문)
            // ──────────────────────────────────────────────
            Console.WriteLine("\n--- FileStream with using ---");
            string file3 = Path.Combine(tempDir, "binary_sample.bin");
            byte[] dataToWrite = { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello"
            using (FileStream fs = new FileStream(file3, FileMode.Create, FileAccess.Write))
            {
                fs.Write(dataToWrite, 0, dataToWrite.Length);
            }
            Console.WriteLine("FileStream 바이너리 쓰기 완료");

            using (FileStream fs = new FileStream(file3, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fs.Length];
                int bytesRead = fs.Read(buffer, 0, buffer.Length);
                Console.WriteLine($"읽은 바이트 수: {bytesRead}, 데이터: {BitConverter.ToString(buffer)}");
            }

            // ──────────────────────────────────────────────
            // 5. Directory, DirectoryInfo, Path
            // ──────────────────────────────────────────────
            Console.WriteLine("\n--- Directory / DirectoryInfo / Path ---");
            string subDir = Path.Combine(tempDir, "하위폴더");
            Directory.CreateDirectory(subDir);
            Console.WriteLine($"하위 폴더 생성됨: {subDir}");
            Console.WriteLine($"폴더 존재 여부: {Directory.Exists(subDir)}");

            DirectoryInfo dirInfo = new DirectoryInfo(tempDir);
            Console.WriteLine($"디렉터리 이름: {dirInfo.Name}");
            Console.WriteLine($"전체 경로: {dirInfo.FullName}");
            Console.WriteLine($"생성 시간: {dirInfo.CreationTime}");
            Console.WriteLine($"하위 항목 수: {dirInfo.GetFileSystemInfos().Length}");

            string combineExample = Path.Combine(tempDir, "폴더A", "폴더B", "test.txt");
            Console.WriteLine($"Path.Combine 예제: {combineExample}");
            Console.WriteLine($"확장자: {Path.GetExtension(combineExample)}");
            Console.WriteLine($"파일명만: {Path.GetFileName(combineExample)}");
            Console.WriteLine($"디렉터리만: {Path.GetDirectoryName(combineExample)}");

            // ──────────────────────────────────────────────
            // 6. File.Exists / Directory.Exists
            // ──────────────────────────────────────────────
            Console.WriteLine("\n--- File.Exists / Directory.Exists ---");
            Console.WriteLine($"sample.txt 존재: {File.Exists(file1)}");
            Console.WriteLine($"존재하지 않는 파일: {File.Exists(Path.Combine(tempDir, "없는파일.txt"))}");
            Console.WriteLine($"작업 폴더 존재: {Directory.Exists(tempDir)}");
            Console.WriteLine($"존재하지 않는 폴더: {Directory.Exists(Path.Combine(tempDir, "없는폴더"))}");

            // 7. BinaryReader / BinaryWriter
            Console.WriteLine("\n--- BinaryReader / BinaryWriter ---");
            string file4 = Path.Combine(tempDir, "binary_data.dat");
            using (BinaryWriter bw = new BinaryWriter(new FileStream(file4, FileMode.Create)))
            {
                bw.Write(12345);        // int
                bw.Write(3.14159);      // double
                bw.Write("문자열 데이터"); // string
                bw.Write(true);         // bool
                bw.Write(255);          // byte (int로 쓰여짐)
            }
            Console.WriteLine("BinaryWriter로 데이터 쓰기 완료");

            using (BinaryReader br = new BinaryReader(new FileStream(file4, FileMode.Open)))
            {
                int intVal = br.ReadInt32();
                double doubleVal = br.ReadDouble();
                string strVal = br.ReadString();
                bool boolVal = br.ReadBoolean();
                // 마지막 byte는 4바이트 int로 쓰였으므로 ReadInt32
                // 하지만 BinaryWriter.Write(byte)는 실제로 byte를 씀 → ReadByte
                // Write(255)에서 255는 int 리터럴이므로 int로 쓰임
                // 여기서는 일관성을 위해 마지막 값을 int로 읽음
                // (BinaryWriter.Write(int)로 쓰였으므로)
                Console.WriteLine($"  int: {intVal}");
                Console.WriteLine($"  double: {doubleVal:F5}");
                Console.WriteLine($"  string: {strVal}");
                Console.WriteLine($"  bool: {boolVal}");
            }
        }
        finally
        {
            // 임시 디렉터리 정리
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, recursive: true);
                Console.WriteLine($"\n임시 디렉터리 삭제 완료: {tempDir}");
            }
        }

        Console.WriteLine("\n=== 파일 I/O 스트림 예제 종료 ===");
    }
}
