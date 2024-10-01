using System;
using System.Collections.Generic;

class Program
{
    // Game grid
    static int[,] grid = new int[4, 5]
    {
        { 1, 2, 3, 2, 1 },
        { 2, 2, 1, 2, 2 },
        { 1, 3, 3, 1, 1 },
        { 1, 2, 3, 3, 1 }
    };

    static void Main(string[] args)
    {
        while (true)
        {
            PrintGrid(); // Display the current state of the grid
            Console.WriteLine("Enter coordinates to remove (row,column) or 'exit' to quit: ");
            var input = Console.ReadLine(); // Read user's input
            if (input?.ToLower() == "exit") break; // Exit condition

            // Ask user input for coordinates using Parse
            var coordinates = input?.Split(',');
            if (coordinates?.Length == 2 &&
                int.TryParse(coordinates[0], out int row) &&
                int.TryParse(coordinates[1], out int col) &&
                IsValidCoordinate(row, col)) 
            {
                RemoveNumber(row, col); // Remove number
                Console.WriteLine("Number removed. Dropping values...");
                DropNumbers(col); // Drop numbers in the specified column
                CheckAndClearMatches(); 
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter valid coordinates."); // Error message
            }
        }
    }

    //print the grid to the console
    static void PrintGrid()
    {
        Console.Clear(); // Clear the console
        for (int i = 0; i < grid.GetLength(0); i++) // rows iteration
        {
            for (int j = 0; j < grid.GetLength(1); j++) // columns iteration
            {
                Console.Write(grid[i, j] + "\t"); // Print each number with tab spacing
            }
            Console.WriteLine();
        }
    }

    // Check if the provided coordinates are valid
    static bool IsValidCoordinate(int row, int col)
    {
        return row >= 0 && row < grid.GetLength(0) && col >= 0 && col < grid.GetLength(1);
    }

    // Method to remove a number at given coordinates
    static void RemoveNumber(int row, int col)
    {
        grid[row, col] = 0; // Remove the number by setting it to 0
    }

    // Method to drop numbers in the specified column
    static void DropNumbers(int col)
    {
        DropColumn(col, 0); // Start dropping from the top
    }

    // Recursive method to drop numbers down the column
    static void DropColumn(int col, int row)
    {
        if (row >= grid.GetLength(0)) return;

        // If the current cell is 0, find a number to drop down
        if (grid[row, col] == 0)
        {
            // Search for the next number below
            for (int i = row + 1; i < grid.GetLength(0); i++)
            {
                if (grid[i, col] != 0)
                {
                    // Swap the values
                    grid[row, col] = grid[i, col];
                    grid[i, col] = 0; // Set the original position to 0
                    break; // Break the loop after the swap
                }
            }
        }

        DropColumn(col, row + 1);
    }

    // Check for matches and clear them
    static void CheckAndClearMatches()
    {
        bool matchFound;
        do
        {
            matchFound = false;
            List<(int, int)> positionsToClear = new List<(int, int)>();

            // Check horizontal matches
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j <= grid.GetLength(1) - 3; j++)
                {
                    if (grid[i, j] != 0 && grid[i, j] == grid[i, j + 1] && grid[i, j] == grid[i, j + 2])
                    {
                        positionsToClear.Add((i, j));
                        positionsToClear.Add((i, j + 1));
                        positionsToClear.Add((i, j + 2));
                        matchFound = true;
                    }
                }
            }

            // Check vertical matches
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i <= grid.GetLength(0) - 3; i++)
                {
                    if (grid[i, j] != 0 && grid[i, j] == grid[i + 1, j] && grid[i, j] == grid[i + 2, j])
                    {
                        positionsToClear.Add((i, j));
                        positionsToClear.Add((i + 1, j));
                        positionsToClear.Add((i + 2, j));
                        matchFound = true;
                    }
                }
            }

            // Clear matched positions
            foreach (var pos in positionsToClear)
            {
                grid[pos.Item1, pos.Item2] = 0; // Set matched positions to 0
            }

            // Drop numbers again after clearing matches
            foreach (int col in GetColumnsWithMatches(positionsToClear))
            {
                DropNumbers(col);
            }

        } while (matchFound); // Repeat until no more matches are found
    }

    // Get unique columns that need dropping after matches
    static HashSet<int> GetColumnsWithMatches(List<(int, int)> positions)
    {
        HashSet<int> columns = new HashSet<int>();
        foreach (var pos in positions)
        {
            columns.Add(pos.Item2);
        }
        return columns;
    }
}

