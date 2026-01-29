using System;
using System.Collections.Generic;

[Serializable]
public class DialogueData
{
    public List<DialogueLine> dialogue = new List<DialogueLine>();
    public List<AvatarData> avatars = new List<AvatarData>();
}