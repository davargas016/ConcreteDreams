public class QuestInstance
{
    public QuestDefinition def;
    public bool objectiveDone;

    public QuestInstance(QuestDefinition def)
    {
        this.def = def;
        objectiveDone = false;
    }
}
