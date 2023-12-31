﻿using System.Text; // For StringBuilder

class CPU
{
    private int spritePos; 
    private int cycle;
    private int wait;
    private int acc;
    private string[] instructions;
    private int instructionPtr;
    // Renderer
    private char[] renderView;

    public bool MoreInstructions()
    {
        return instructionPtr < instructions.Length;
    }
    public CPU(string[] inputInstructions)
    {
        spritePos = 1;
        cycle = 1;
        wait = 0;
        acc = 0;
        instructions = inputInstructions;
        instructionPtr = 0;
        renderView = new char[240];
        char fillCharacter = '.';
        for (int i = 0; i < renderView.Length; i++)
        {
            renderView[i] = fillCharacter;
        }

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

        // Render occurs DURING the cycle
        //Console.WriteLine(String.Format("Calculated sprite position as {0}\t - Cycle is {1}", rX, ((cycle - 1) % 40) + 1));
        int pixel = ((cycle - 1) % 40);
        if (Math.Abs(pixel - spritePos) <= 1)
        {
            // Currect pixel is within 1 of the sprite
            // This means render the pixel
            // Problem is communicated from 1-240
            // Execution is implemented in an array which must be 0-239
            // Hence -1
            renderView[cycle - 1] = '#';
        }
        else
        {
            renderView[cycle - 1] = '.';
        }

        // *************
        // END OF CYCLE

        // Decrement wait time
        if (wait > 0)
        {
            wait--;
        }
        // Execute if wait time is complete
        if (wait == 0)
        {
            // Resolve current instruction
            spritePos += acc;
            //Console.WriteLine("Register has changed to", spritePos);
        }





        cycle++;
    }
    public void Render()
    {
        for (int row = 0; row < 6; row++)
        {
            StringBuilder sb = new StringBuilder();
            for (int col = 0; col < 40; col++)
            {
                sb.Append(renderView[row * 40 + col]);
            }
            Console.WriteLine(sb.ToString());
        }


    }
    public void Compute()
    {
        while (MoreInstructions() || wait > 0)
        {
            // Format
            //Console.WriteLine();

            // Start
            //Console.WriteLine(String.Format("Start of cycle {0} \tX is {1}\t Acc is {2}\t Wait is {3}", cycle, rX, acc, wait));

            // Step the processor forward one cycle
            Tick();

            //Console.WriteLine(String.Format("After cycle {0} \tX is {1}\t Acc is {2}\t Wait is {3}\n======================\n\n", cycle-1, rX, acc, wait));

        }
    }
}

class Program
{
    public static void Main(string[] args)
    {
        //string[] instructions = System.IO.File.ReadAllLines(@"C:\Users\Tom\Documents\Advent\Day 10 - CRT\Day 10 - CRT\data_test.txt");
        //string[] instructions = System.IO.File.ReadAllLines(@"C:\Users\Tom\Documents\Advent\Day 10 - CRT\Day 10 - CRT\data_test_larger.txt");
        string[] instructions = System.IO.File.ReadAllLines(@"C:\Users\Tom\Documents\Advent\Day 10 - CRT\Day 10 - CRT\data_full.txt");


        CPU cpu = new CPU(instructions);

        // Start the processor
        // Read and execute instructions until none remain 
        cpu.Compute();
        
        // Output the grid of pixels
        cpu.Render();
    }
}