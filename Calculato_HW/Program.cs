using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("CALCULATOR( exit = press q)");
        while (true)
        {
            try
            {
                Console.Write("\nFirst number");
                string input = Console.ReadLine();
                if (input.ToLower() == "q") break;
                double a = Convert.ToDouble(input);

                Console.Write(Environment.NewLine + "Second number");
                input = Console.ReadLine();
                if (input.ToLower() == "q") break;
                double b = Convert.ToDouble(input);

                Console.Write(Environment.NewLine + "Operation");
                string op = Console.ReadLine();
                if (op.ToLower() == "q") break;

                double res = 0;
                switch (op)
                {
                    case "+": res = a + b; break;
                    case "-": res = a - b; break;
                    case "*": res = a * b; break;
                    case "/":
                        if (b == 0)
                        {
                            Console.WriteLine("Error1");
                        }
                        res = a / b; break;
                    default:
                        Console.WriteLine(" Error2");
                        continue;
                }
                Console.WriteLine($"Result: {a} {op} {b} = {res}");


            }
            catch (Exception ex) {
                Console.WriteLine("Error");
            }
        }
        Console.WriteLine("Program done");

    }
}
