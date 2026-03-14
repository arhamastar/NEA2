using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Map
{
    public List<List<Tile>> map;

    public Map()
    {
        int width = 15;
        int height = 15;
        map = new List<List<Tile>>();

        for (int y = 0; y < height; y++)
        {
            List<Tile> row = new List<Tile>();
            for (int x = 0; x < width; x++)
            {
                row.Add(new Grass(x, y));
            }
            map.Add(row);
        }

        System.Diagnostics.Debug.WriteLine("Before: Map is all grass.");
        MazeGenerator.GenerateMaze(map, 0, 0);
        System.Diagnostics.Debug.WriteLine("AFTER: Maze should be generated.");
        map[0][0].Type = "Road";
        map[height - 1][width - 1].Type = "Road";
    }

    public Map(string file)
    {
        map = new List<List<Tile>>();
        string line;
        try
        {
            StreamReader sr = new StreamReader($@"C:\{file}");
            line = sr.ReadLine();
            int count = 0;
            while (line != null)
            {
                List<Tile> currentRow = new List<Tile>();
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '0')
                    {
                        currentRow.Add(new Road(i, count, Int32.Parse(line[i].ToString())));
                    }
                }
                map.Add(currentRow);
                count++;
                line = sr.ReadLine();
            }
            sr.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D tileTexture, Texture2D brickTexture, Texture2D sewerTexture, List<Tile> path = null)
    {
        int tileSize = 50;

        foreach (List<Tile> row in map)
        {
            foreach (Tile tile in row)
            {
                Rectangle destinationRectangle = new Rectangle(
                    tile.X * tileSize,
                    tile.Y * tileSize,
                    tileSize,
                    tileSize
                );

                if (tileTexture != null)
                {
                    if (path != null && path.Contains(tile))
                        spriteBatch.Draw(tileTexture, destinationRectangle, Color.Yellow);
                    else
                        spriteBatch.Draw(tileTexture, destinationRectangle, Color.Gray);
                }

                if (tile.Type == "Road")
                {
                    if (sewerTexture != null)
                        spriteBatch.Draw(sewerTexture, destinationRectangle, Color.White);
                }
                else
                {
                    if (brickTexture != null)
                        spriteBatch.Draw(brickTexture, destinationRectangle, Color.White);
                }

                if (path != null && path.Contains(tile) && sewerTexture != null)
                {
                    spriteBatch.Draw(sewerTexture, destinationRectangle, new Color(255, 255, 0, 128));
                }
            }
        }

        Tile goalTile = map[map.Count - 1][map[map.Count - 1].Count - 1];
        Rectangle goalRect = new Rectangle(goalTile.X * tileSize, goalTile.Y * tileSize, tileSize, tileSize);
        if (tileTexture != null)
            spriteBatch.Draw(tileTexture, goalRect, Color.Green);
    }

    public List<Tile> FindPath(int startX, int startY, int goalX, int goalY)
    {
        var openSet = new List<Tile>();
        var closedSet = new HashSet<Tile>();
        var cameFrom = new Dictionary<Tile, Tile>();
        var gScore = new Dictionary<Tile, int>();
        var fScore = new Dictionary<Tile, int>();

        Tile start = map[startY][startX];
        Tile goal = map[goalY][goalX];

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            Tile current = openSet.OrderBy(t => fScore.ContainsKey(t) ? fScore[t] : int.MaxValue).First();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Tile neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || !IsWalkable(neighbor))
                    continue;

                int tentativeGScore = gScore[current] + 1;

                if (!openSet.Contains(neighbor))
                    openSet.Add(neighbor);
                else if (tentativeGScore >= (gScore.ContainsKey(neighbor) ? gScore[neighbor] : int.MaxValue))
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
            }
        }
        return null;
    }

    private int Heuristic(Tile a, Tile b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    private List<Tile> GetNeighbors(Tile tile)
    {
        var neighbors = new List<Tile>();
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int nx = tile.X + dx[i];
            int ny = tile.Y + dy[i];
            if (ny >= 0 && ny < map.Count && nx >= 0 && nx < map[ny].Count)
                neighbors.Add(map[ny][nx]);
        }
        return neighbors;
    }

    private bool IsWalkable(Tile tile)
    {
        return tile.Type == "Road";
    }

    private List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile current)
    {
        var totalPath = new List<Tile> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }
}

