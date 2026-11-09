
public interface IPlayerDirectionManagement
{
    bool HasFrontDirection { get; }

    void SetFrontDirection();
    void SetBackDirection();
}

public class PlayerDetails : IPlayerDirectionManagement
{
    public bool HasFrontDirection { get; private set; } = true;

    public void SetFrontDirection()
    {
        HasFrontDirection = true;
    }

    public void SetBackDirection()
    {
        HasFrontDirection = false;
    }
}
