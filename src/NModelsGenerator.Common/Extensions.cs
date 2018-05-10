namespace NModelsGenerator.Common
{
    public static class Extensions
    {
        public static string ToPasacalCase(this string str)
        {
            var nm = new NamingConvention()
            {
                DbNamingConvention = NamingConvention.NamingConventions.UnderScoreCase,
                ObjectNamingConvention = NamingConvention.NamingConventions.PascalCase
            };
            str = nm.ProcessDbNamesToObjectNames(str);
            return str;
        }
    }
}
