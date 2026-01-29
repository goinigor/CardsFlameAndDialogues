using System;

[Serializable]
public class AvatarData
{
    public string name;
    public string url;
    public string position;

    public AvatarPosition GetPosition()
    {
        if (Enum.TryParse(position, true, out AvatarPosition result))
        {
            return result;
        }

        return AvatarPosition.left;
    }
}