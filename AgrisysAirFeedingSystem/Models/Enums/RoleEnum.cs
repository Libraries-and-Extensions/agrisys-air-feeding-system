namespace AgrisysAirFeedingSystem.Models.Enums;

public enum RoleEnum
{
    Admin,
    Manager,
    User,
    None,
}

public static class RoleEnumExtensions
{
    public static int GetRoleLevel(this RoleEnum me)
    {
        var level = 0;
        
        
        switch (me)
        {
            case RoleEnum.Admin:
                level = 3;
                break;
            case RoleEnum.Manager:
                level = 2;
                break;
            case RoleEnum.User:
                level = 1;
                break;
            case RoleEnum.None:
                level = 1;
                break;
        }

        return level;
    }
}