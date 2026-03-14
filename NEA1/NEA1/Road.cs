class Road : Tile
{
    private int weight;

    public Road(int x, int y, int weight) : base(x, y, "Road")
    {
        this.weight = weight;
    }
}