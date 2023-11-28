namespace AgrisysAirFeedingSystem.Utils;

public class QueryUtils
{
    public static List<string> getListParameter(HttpContext context, string parameterName, string seperator = ",")
    {
        var sKeys = context.Request.Query[parameterName];

        var keyList = new List<string>();

        foreach (var entry in sKeys.Where(entry => entry != null)) keyList.AddRange(entry!.Split(seperator));

        return keyList;
    }
}