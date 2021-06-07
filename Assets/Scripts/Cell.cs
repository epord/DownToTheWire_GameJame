public class Cell
{
    public Piece piece { get;  set; }

    public bool IsEmpty()
    {
        return piece == null;
    }
}
