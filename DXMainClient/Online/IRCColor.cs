namespace DTAClient.Online;

public class IRCColor
{
    public IRCColor(string name, bool selectable, int ircColorId)
    {
        Name = name;
        Selectable = selectable;
        IrcColorId = ircColorId;
    }

    public string Name { get; private set; }
    public bool Selectable { get; private set; }
    public int IrcColorId { get; private set; }
}