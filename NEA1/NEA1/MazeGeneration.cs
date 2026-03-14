using System;
using System.Collections.Generic;

class MazeGenerator
{
    public static void GenerateMaze(List<List<Tile>> grid, int startX, int startY)
    {
        CarvePassage(startX, startY, grid);
    }

    // REPLACE

    private static void CarvePassage(int x, int y, List<List<Tile>> grid)
    {
        grid[y][x].Visited = true;
        grid[y][x].Type = "Road"; 

        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { -1, 0, 1, 0 };

        int[] directions = GenerateRandomDirectionOrder();

        // UP TO HERE 

        for (int i = 0; i < directions.Length; i++)
        {
            int dir = directions[i];
            int newX = x + dx[dir];
            int newY = y + dy[dir];

            if (newX >= 0 && newY >= 0 && newX < grid[0].Count && newY < grid.Count && !grid[newY][newX].Visited)
            {
                CarvePassage(newX, newY, grid);
            }
        }
    }

    private static int[] GenerateRandomDirectionOrder()
    {
        int[] dirs = { 0, 1, 2, 3 };
        Random rand = new Random();
        for (int i = dirs.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            int temp = dirs[i];
            dirs[i] = dirs[j];
            dirs[j] = temp;
        }
        return dirs;
    }
}