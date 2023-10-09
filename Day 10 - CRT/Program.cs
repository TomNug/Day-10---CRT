class CPU
{
    private int rX; // register
    private int cycle;
    private int wait;
    private int acc;
    string[] instructions;
    int instructionPtr;
    public bool MoreInstructions()
    {
        return instructionPtr < instructions.Length;
    }
    public CPU(string[] inputInstructions)
    {
        rX = 1;
        cycle = 1;
        wait = 0;
        acc = 0;
        instructions = inputInstructions;
        instructionPtr = 0;
    }
    public void FetchAndDecode()
    {
        string fetchedInstruction = instructions[instructionPtr];
        instructionPtr++;
        //Console.WriteLine(String.Format("Fetched {0}", fetchedInstruction));

        // noop
        // noop does nothing, except increment the cycle
        if (fetchedInstruction[0] == 'n')
        {
            acc = 0;
            wait = 1;
        } 
        // an addx instruction, which adds the value after 2 cycles
        else
        {
            string[] instructionSplit = fetchedInstruction.Split(' ');
            acc = int.Parse(instructionSplit[1]);
            wait = 2;
        }
    }
    public void Tick()
    {
        // Do I need another instruction?
        if (wait == 0)
        {
            // Fetch and decode new instruction
            if (MoreInstructions())
            {
                FetchAndDecode();
            }
        }
        // Decrement wait time
        if (wait > 0)
        {
            wait--;
        }
        // Execute if wait time is complete
        if (wait == 0)
        {
            // Resolve current instruction
            rX += acc;
        }
        cycle++;
    }

    public void Compute()
    {
        while (MoreInstructions() || wait > 0)
        {
            // Format
            //Console.WriteLine();

            // Start
            //Console.WriteLine(String.Format("Start of cycle {0} \tX is {1}\t Acc is {2}\t Wait is {3}", cycle, rX, acc, wait));
            
            // Report the signal strengths at the given times
            if (cycle == 20 || (cycle-20) % 40 == 0)
            {
                Console.WriteLine(String.Format("### {0}th cycle\tSignal Strength: {1}", cycle, cycle * rX));
            }

            Tick();
            //Console.WriteLine(String.Format("After of cycle {0} \tX is {1}\t Acc is {2}\t Wait is {3}", cycle-1, rX, acc, wait));

        }
    }
}

class Program
{
    public static void Main(string[] args)
    {
        //string[] instructions = System.IO.File.ReadAllLines(@"C:\Users\Tom\Documents\Advent\Day 10 - CRT\Day 10 - CRT\data_test.txt");
        string[] instructions = System.IO.File.ReadAllLines(@"C:\Users\Tom\Documents\Advent\Day 10 - CRT\Day 10 - CRT\data_test_larger.txt");

        CPU cpu = new CPU(instructions);

        cpu.Compute();
    }
}