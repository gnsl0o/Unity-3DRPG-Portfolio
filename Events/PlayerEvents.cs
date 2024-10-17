using System;

public class PlayerEvents
{
    public event Action<int> onPlayerLevelChange;

    public void PlayerLevelUp(int level)
    {
        onPlayerLevelChange(level);
    }

    public event Action<int> onExperienceGained;

    public void ExperienceGained(int experienceGained)
    {
        onExperienceGained(experienceGained);
    }
}
