namespace lab02
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Jana Yassin" + ", " + DateTime.Now);





            // Value Types
            int age = 21;
            double height = 168D;
            bool isGraduated = false;

            // Reference Types
            string studentName = "Jana";
            int[] numbers = { 10, 20, 30 };
            List<string> courses = new List<string>
        {

            "C#",
            "Database",

        };

            Console.WriteLine($"age type: {age.GetType()}");
            Console.WriteLine($"height type: {height.GetType()}");
            Console.WriteLine($"isGraduated type: {isGraduated.GetType()}");

            Console.WriteLine($"name type: {studentName.GetType()}");
            Console.WriteLine($"array type: {numbers.GetType()}");
            Console.WriteLine($"list type: {courses.GetType()}");


            Console.WriteLine("\n=== Value Type Copy ===");
            TestValueType();


            Console.WriteLine("\n=== Reference Type Copy ===");
            TestReferenceType();


            Console.WriteLine("\n=== Grade Classifier ===");

            Console.WriteLine(GetGrade(95));
            Console.WriteLine(GetGrade(75));
            Console.WriteLine(GetGrade(55));
            Console.WriteLine(GetGrade(30));


            Console.WriteLine("\n=== Nullable Example ===");

            Console.Write("Enter your name: ");

            string? name = Console.ReadLine();

            if (name == null || name == "")
            {
                Console.WriteLine("No name entered");
            }
            else
            {
                Console.WriteLine($"Hello {name}");
            }
        }


        static void TestValueType()
        {
            int x = 10;

            int y = x;

            Console.WriteLine("Before change:");
            Console.WriteLine($"x: {x}");
            Console.WriteLine($"y: {y}");

            y = 50;

            Console.WriteLine("After changing y:");
            Console.WriteLine($"x: {x}");
            Console.WriteLine($"y: {y}");
        }


        static void TestReferenceType()
        {
            int[] x = { 1, 2, 3 };

            int[] y = x;

            Console.WriteLine("Before change:");
            Console.WriteLine($"x array value: {x[0]}");
            Console.WriteLine($"y array value: {y[0]}");


            y[0] = 100;


            Console.WriteLine("After changing y array:");
            Console.WriteLine($"x array value: {x[0]}");
            Console.WriteLine($"y array value: {y[0]}");
        }


        static string GetGrade(int score)
        {
            return score switch
            {
                >= 90 => "Excellent",
                >= 70 => "Proficient",
                >= 50 => "Developing",
                _ => "Below Standard"
            };
        }
    }
}
    

