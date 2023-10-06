public class XPManager : Singleton<XPManager>
{
    private void Awake()
    {
        currentLevel = 1;
        UpdateViewModel();

    }

    public static int currentXP;
    public static int currentLevel = 1;
    public static float statsBonus => (currentLevel - 1) * 0.1f;
    public static int XpToNextLevel => currentLevel * 5;
    public static void AddXP(int ammount)
    {
        currentXP += ammount;

        while (currentXP > XpToNextLevel)
        {
            currentLevel++;
            currentXP -= XpToNextLevel;

        }
        UpdateViewModel();
    }
    private static void UpdateViewModel()
    {
        ViewModel.instance.CurrentXp = currentXP;
        ViewModel.instance.CurrentLevel = currentLevel;
        ViewModel.instance.XpToNextLevel = XpToNextLevel;
        ViewModel.instance.StatsBonus = statsBonus;
    }
}
